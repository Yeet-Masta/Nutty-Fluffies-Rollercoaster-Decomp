#!/usr/bin/env python3
"""Nutty Fluffies Rollercoaster: Unity 4.0 (ARMv7/Mono) -> Unity 2022.3 (ARM64/IL2CPP).

Turns the original APK into an upgraded Unity project, then optionally builds
the ARM64 APK. Nothing from the game itself is stored in this repository; it
is all regenerated from your own copy of the APK.

  python tools/nfport.py all   --apk NuttyFluffies.apk --assetripper <AssetRipper.GUI.Free> --unity <Unity 2022.3 editor>
  python tools/nfport.py build --unity <Unity 2022.3 editor>

Steps (each can be run on its own, in order):
  extract  unzip the APK into work/apk
  rip      run AssetRipper headless: APK -> work/ripped (Unity 4 project + decompiled C#)
  port     copy to the output project and apply every fix:
             * overlay port/Assets (IL2CPP-safe UnityScript runtime, legacy
               particle shims, Prime31 compat, editor build scripts, link.xml)
             * level/theme packs -> Assets/StreamingAssets/GameResources
             * legacy particle components -> shim MonoBehaviours
             * dead SDK bindings (Amazon, Twitter, Facebook, Flurry, ...) stubbed
             * decompiler fix-ups + compiler-driven Unity API updater, repeated
               until both script assemblies compile against Unity 2022.3
  build    run Unity in batch mode to produce Builds/NuttyFluffies-arm64.apk

Requirements: Python 3.8+, .NET SDK 8+ (`dotnet`), AssetRipper, Unity 2022.3 LTS
with Android Build Support (incl. Android SDK/NDK and OpenJDK modules).
"""
import argparse
import os
import pathlib
import platform
import shutil
import socket
import subprocess
import sys
import time
import urllib.error
import urllib.parse
import urllib.request
import zipfile

REPO = pathlib.Path(__file__).resolve().parent.parent
TOOLS = REPO / "tools"
OVERLAY = REPO / "port" / "Assets"

# Precompiled assemblies AssetRipper leaves in Assets/Plugins that must not be
# imported: two .NET facades that clash with Unity 2022's own copies, and the
# .NET 2.0 Prime31 REST kit (replaced by port/Assets/Plugins/NFPort/Compat).
DROP_PLUGINS = [
    "P31RestKit.dll",
    "System.Runtime.CompilerServices.Unsafe.dll",
    "System.Runtime.InteropServices.dll",
]


def log(msg):
    print("[nfport] " + msg, flush=True)


def fail(msg):
    sys.exit("[nfport] error: " + msg)


def run(cmd, **kw):
    log("$ " + " ".join(str(c) for c in cmd))
    return subprocess.run([str(c) for c in cmd], check=True, **kw)


# --------------------------------------------------------------------- extract
def step_extract(args):
    apk = pathlib.Path(args.apk or "").expanduser()
    if not apk.is_file():
        fail("pass --apk <path to Nutty Fluffies Rollercoaster 1.0.5 APK>")
    dest = args.work / "apk"
    if dest.exists():
        shutil.rmtree(dest)
    with zipfile.ZipFile(apk) as z:
        z.extractall(dest)
    if not (dest / "assets/bin/Data/Managed/Assembly-UnityScript.dll").exists():
        fail("%s does not look like the Unity 4 Nutty Fluffies APK" % apk)
    shutil.copy2(apk, args.work / "original.apk")
    log("extracted %s" % apk)


# ------------------------------------------------------------------------- rip
def _free_port():
    with socket.socket() as s:
        s.bind(("127.0.0.1", 0))
        return s.getsockname()[1]


def _post(url, path, timeout):
    data = urllib.parse.urlencode({"Path": str(path)}).encode()
    req = urllib.request.Request(url, data=data, method="POST")
    try:
        urllib.request.urlopen(req, timeout=timeout).read()
    except urllib.error.HTTPError as e:
        if e.code not in (301, 302, 303):
            raise


def step_rip(args):
    exe = pathlib.Path(args.assetripper or "").expanduser()
    if exe.is_dir():
        candidates = [exe / "AssetRipper.GUI.Free.exe", exe / "AssetRipper.GUI.Free"]
        exe = next((c for c in candidates if c.exists()), exe)
    if not exe.is_file():
        fail("pass --assetripper <path to AssetRipper.GUI.Free(.exe) or its folder>")
    apk = args.work / "original.apk"
    if not apk.exists():
        fail("run the extract step first")
    out = args.work / "ripped"
    if out.exists():
        shutil.rmtree(out)

    port = _free_port()
    url = "http://127.0.0.1:%d" % port
    proc = subprocess.Popen(
        [str(exe), "--headless", "--port", str(port), "--log-path", str(args.work / "assetripper.log")],
        cwd=str(exe.parent), stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL,
    )
    try:
        for _ in range(120):
            try:
                urllib.request.urlopen(url + "/", timeout=2).read()
                break
            except (urllib.error.URLError, ConnectionError, socket.timeout):
                if proc.poll() is not None:
                    fail("AssetRipper exited early, see %s" % (args.work / "assetripper.log"))
                time.sleep(1)
        else:
            fail("AssetRipper did not start")
        log("AssetRipper: loading %s" % apk)
        _post(url + "/LoadFile", apk, timeout=3600)
        log("AssetRipper: exporting Unity project to %s" % out)
        _post(url + "/Export/UnityProject", out, timeout=3600)
    finally:
        proc.terminate()
        try:
            proc.wait(timeout=30)
        except subprocess.TimeoutExpired:
            proc.kill()
    if not (out / "ExportedProject/Assets").is_dir():
        fail("AssetRipper export failed, see %s" % (args.work / "assetripper.log"))
    log("ripped project at %s" % (out / "ExportedProject"))


# ------------------------------------------------------------------------ port
def unity_managed_dir(unity):
    """Find <Editor>/Data/Managed from an editor executable or install folder."""
    if not unity:
        return None
    p = pathlib.Path(unity).expanduser()
    for base in [p] + list(p.parents)[:4]:
        for rel in ["Data/Managed", "Editor/Data/Managed", "Contents/Managed", "Unity.app/Contents/Managed"]:
            cand = base / rel
            if (cand / "UnityEngine/UnityEngine.CoreModule.dll").exists():
                return cand
    return None


def compile_check(args, project, managed):
    """Build the runtime and editor script assemblies; returns (error_count, [log files])."""
    ccdir = args.work / "compilecheck"
    if ccdir.exists():
        shutil.rmtree(ccdir)
    shutil.copytree(TOOLS / "compilecheck", ccdir)
    logs, total = [], 0
    for name in ("firstpass", "main", "editor"):
        res = subprocess.run(
            [args.dotnet, "build", str(ccdir / name), "-nologo", "-clp:NoSummary",
             "-p:ProjectRoot=%s" % project, "-p:UnityManaged=%s" % managed],
            stdout=subprocess.PIPE, stderr=subprocess.STDOUT, text=True, encoding="utf-8", errors="replace",
        )
        errors = sorted({l.strip() for l in res.stdout.splitlines() if ": error " in l})
        out = ccdir / (name + ".log")
        out.write_text("\n".join(errors), encoding="utf-8")
        logs.append(out)
        total += len(errors)
        if errors:
            break  # each assembly depends on the previous ones
    return total, logs


def step_port(args):
    ripped = args.work / "ripped/ExportedProject"
    apk = args.work / "apk"
    if not ripped.is_dir():
        fail("run the rip step first")
    if shutil.which(args.dotnet) is None:
        fail("the .NET SDK (8 or newer) is required: https://dotnet.microsoft.com/download")
    managed = unity_managed_dir(args.unity)
    if managed is None:
        fail("pass --unity <Unity 2022.3 editor executable or install folder> (needed for its engine DLLs)")

    project = args.project
    if project.exists():
        if not args.force:
            fail("%s exists; pass --force to replace it" % project)
        shutil.rmtree(project)
    log("copying ripped project -> %s" % project)
    shutil.copytree(ripped, project)

    plugins = project / "Assets/Plugins"
    for name in DROP_PLUGINS:
        for f in (plugins / name, plugins / (name + ".meta")):
            if f.exists():
                f.unlink()
    shutil.copytree(OVERLAY, project / "Assets", dirs_exist_ok=True)

    resources = apk / "assets/GameResources"
    if resources.is_dir():
        shutil.copytree(resources, project / "Assets/StreamingAssets/GameResources", dirs_exist_ok=True)
        log("copied level/theme packs to Assets/StreamingAssets/GameResources")

    py = [sys.executable, "-I"]
    run(py + [TOOLS / "convert_legacy_particles.py", project])
    run(py + [TOOLS / "stub_sdks.py", project])
    run(py + [TOOLS / "codemods.py", project], stdout=subprocess.DEVNULL)

    log("compiling against %s and applying API updates" % managed)
    for round_no in range(1, 16):
        errors, logs = compile_check(args, project, managed)
        log("round %d: %d compile errors" % (round_no, errors))
        if errors == 0:
            break
        edits = subprocess.run(py + [TOOLS / "api_updater.py", project] + logs,
                               check=True, stdout=subprocess.PIPE, text=True).stdout.strip()
        mods = subprocess.run(py + [TOOLS / "codemods.py", project],
                              check=True, stdout=subprocess.PIPE, text=True).stdout.strip()
        if edits == "0" and mods.endswith(": 0 files changed"):
            for f in logs:
                print(f.read_text(encoding="utf-8"))
            fail("%d compile errors left that no rule handles (listed above)" % errors)
    else:
        fail("API update did not converge")
    log("scripts compile cleanly against Unity 2022.3")
    log("done: open %s with Unity 2022.3 (accept the upgrade prompt), then NFPort > Build Android ARM64 APK" % project)


# ----------------------------------------------------------------------- build
def unity_executable(unity):
    p = pathlib.Path(unity or "").expanduser()
    if p.is_file():
        return p
    names = {"Windows": ["Editor/Unity.exe", "Unity.exe"],
             "Darwin": ["Unity.app/Contents/MacOS/Unity", "Contents/MacOS/Unity"]}.get(platform.system(), ["Editor/Unity", "Unity"])
    for n in names:
        if (p / n).is_file():
            return p / n
    return None


def step_build(args):
    exe = unity_executable(args.unity)
    if exe is None:
        fail("pass --unity <Unity 2022.3 editor executable or install folder>")
    output = pathlib.Path(args.output).resolve()
    logfile = args.work / "unity-build.log"
    log("building %s (log: %s)" % (output, logfile))
    run([exe, "-batchmode", "-quit", "-projectPath", args.project.resolve(), "-buildTarget", "Android",
         "-executeMethod", "NFPort.Editor.NFPortBuild.AndroidArm64", "-nfportOutput", output,
         "-logFile", logfile])
    log("built %s" % output)


STEPS = {"extract": step_extract, "rip": step_rip, "port": step_port, "build": step_build}


def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("step", choices=list(STEPS) + ["all"])
    ap.add_argument("--apk", help="original APK (extract)")
    ap.add_argument("--assetripper", help="AssetRipper.GUI.Free executable or folder (rip)")
    ap.add_argument("--unity", help="Unity 2022.3 editor executable or install folder (port, build)")
    ap.add_argument("--dotnet", default=os.environ.get("DOTNET", "dotnet"), help="dotnet executable (port)")
    ap.add_argument("--work", type=pathlib.Path, default=REPO / "work", help="scratch folder (default: work/)")
    ap.add_argument("--project", type=pathlib.Path, default=REPO / "NuttyFluffiesPort",
                    help="upgraded Unity project (default: NuttyFluffiesPort/)")
    ap.add_argument("--output", default=str(REPO / "Builds/NuttyFluffies-arm64.apk"), help="APK to build")
    ap.add_argument("--force", action="store_true", help="replace an existing output project")
    args = ap.parse_args()
    args.work = args.work.resolve()
    args.project = args.project.resolve()
    args.work.mkdir(parents=True, exist_ok=True)

    steps = ["extract", "rip", "port"] if args.step == "all" else [args.step]
    if args.step == "all" and args.unity and unity_executable(args.unity):
        steps.append("build")
    for s in steps:
        log("== %s ==" % s)
        STEPS[s](args)


if __name__ == "__main__":
    main()
