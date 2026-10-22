#!/usr/bin/env python3
"""Convert Unity 4 legacy particle components in scenes/prefabs to the shim
MonoBehaviours in port/Assets/Plugins/NFPort/LegacyParticles.

Unity 2018.3+ no longer knows EllipsoidParticleEmitter (class 15),
ParticleAnimator (12) or ParticleRenderer (26), so opening the ripped project
would silently drop them. This rewrites each such YAML document into a
MonoBehaviour (class 114) pointing at the matching shim script, renaming the
legacy keys that are not valid C# identifiers. Object IDs are kept, so
references from game scripts (e.g. `dustEmitter: {fileID: 1467}`) stay valid.

Usage: convert_legacy_particles.py <unity-project-dir>
"""
import pathlib
import re
import sys

SHIMS = pathlib.Path(__file__).resolve().parent.parent / "port/Assets/Plugins/NFPort/LegacyParticles"

CLASSES = {
    "15": ("EllipsoidParticleEmitter", "EllipsoidParticleEmitter"),
    "12": ("ParticleAnimator", "ParticleAnimator"),
    "26": ("ParticleRenderer", "ParticleRenderer"),
}
RENAMES = {
    "Does Animate Color?": "doesAnimateColor",
    "Simulate in Worldspace?": "simulateInWorldspace",
}
HEADER = re.compile(r"(?m)^--- !u!(\d+) &(-?\d+)(.*)$")
COMPONENT_REF = re.compile(r"(?m)^(\s*- )(\d+)(: \{fileID: (-?\d+)\})")


def script_guid(name):
    meta = (SHIMS / (name + ".cs.meta")).read_text()
    return re.search(r"guid: (\w+)", meta).group(1)


def convert_body(body, script, guid):
    lines = body.split("\n")
    out = []
    colors = []
    uv = {}
    i = 0
    while i < len(lines):
        line = lines[i]
        m = re.match(r"^  colorAnimation\[\d+\]:\s*$", line)
        if m:
            # colorAnimation[n]:\n    serializedVersion: 2\n    rgba: N
            j = i + 1
            while j < len(lines) and lines[j].startswith("    "):
                rgba = re.match(r"^\s+rgba: (\d+)", lines[j])
                if rgba:
                    colors.append(rgba.group(1))
                j += 1
            i = j
            continue
        if line.startswith("  UV Animation:"):
            j = i + 1
            while j < len(lines) and lines[j].startswith("    "):
                k, _, v = lines[j].strip().partition(": ")
                uv[k] = v
                j += 1
            i = j
            continue
        key, sep, value = line.partition(": ")
        stripped = key.strip()
        if line.startswith("  ") and not line.startswith("   ") and stripped in RENAMES:
            line = "  %s: %s" % (RENAMES[stripped], value)
        elif line.startswith("  serializedVersion:"):
            i += 1
            continue
        out.append(line)
        i += 1

    # Insert the MonoBehaviour bookkeeping right after m_GameObject.
    for idx, line in enumerate(out):
        if line.startswith("  m_GameObject:"):
            extra = []
            if not any(l.startswith("  m_Enabled:") for l in out):
                extra.append("  m_Enabled: 1")
            extra += [
                "  m_EditorHideFlags: 0",
                "  m_Script: {fileID: 11500000, guid: %s, type: 3}" % guid,
                "  m_Name: ",
                "  m_EditorClassIdentifier: ",
            ]
            out[idx + 1:idx + 1] = extra
            break
    tail = []
    if colors:
        tail.append("  colorAnimationRgba:")
        tail += ["  - %s" % c for c in colors]
    if uv:
        tail.append("  uvAnimationXTile: %s" % uv.get("x Tile", "1"))
        tail.append("  uvAnimationYTile: %s" % uv.get("y Tile", "1"))
        tail.append("  uvAnimationCycles: %s" % uv.get("cycles", "1"))
    while out and out[-1] == "":
        out.pop()
    out[0] = "MonoBehaviour:"
    return "\n".join(out + tail) + "\n"


def convert_file(path):
    text = path.read_text(encoding="utf-8")
    parts = HEADER.split(text)
    # parts = [preamble, cls, id, rest, body, cls, id, rest, body, ...]
    converted = set()
    for n in range(1, len(parts), 4):
        cls, body = parts[n], parts[n + 3]
        if cls not in CLASSES:
            continue
        legacy, script = CLASSES[cls]
        first = body.lstrip("\n").split("\n", 1)[0]
        if first.strip() != legacy + ":":
            continue
        lead = body[: len(body) - len(body.lstrip("\n"))]
        parts[n] = "114"
        parts[n + 3] = lead + convert_body(body.lstrip("\n"), script, script_guid(script))
        converted.add(parts[n + 1])
    if not converted:
        return 0
    out = parts[0]
    for n in range(1, len(parts), 4):
        out += "--- !u!%s &%s%s%s" % (parts[n], parts[n + 1], parts[n + 2], parts[n + 3])
    out = COMPONENT_REF.sub(
        lambda m: m.group(1) + ("114" if m.group(4) in converted else m.group(2)) + m.group(3), out
    )
    path.write_text(out, encoding="utf-8")
    return len(converted)


def main(project):
    total = 0
    for path in sorted(pathlib.Path(project, "Assets").rglob("*")):
        if path.suffix in (".unity", ".prefab", ".asset") and path.is_file():
            count = convert_file(path)
            if count:
                print("  %s: %d legacy particle components" % (path.relative_to(project), count))
                total += count
    print("converted %d legacy particle components" % total)


if __name__ == "__main__":
    if len(sys.argv) != 2:
        sys.exit(__doc__)
    main(sys.argv[1])
