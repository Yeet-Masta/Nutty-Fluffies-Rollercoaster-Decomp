#!/usr/bin/env python3
"""Source fix-ups for decompiler artifacts that stop the ripped scripts from
compiling as C# in Unity 2022.3.

The rules are generic (they match constructs ILSpy emits for UnityScript/Boo
output) rather than edits to specific lines, so they keep working if the
decompiler output shifts slightly.

Usage: codemods.py <unity-project-dir>
"""
import pathlib
import re
import sys


def strip_interface_virtual(text):
    """UnityScript interfaces decompile with `virtual` on their members."""
    out, depth, in_iface = [], 0, False
    for line in text.split("\n"):
        if re.match(r"\s*(public |internal )?interface \w+", line):
            in_iface, depth = True, 0
        if in_iface:
            line = re.sub(r"^(\s+)virtual (?=.*\);\s*$)", r"\1", line)
            depth += line.count("{") - line.count("}")
            if depth <= 0 and "}" in line:
                in_iface = False
        out.append(line)
    return "\n".join(out)


def rename_members_named_like_type(text, renames):
    """UnityScript allowed `static function DrawLine` inside `class DrawLine`."""
    m = re.search(r"\bclass (\w+)\b", text)
    if not m:
        return text
    cls = m.group(1)
    decl = re.compile(r"\b(?:public|private|internal|protected)\s+(?:static\s+)?[\w<>\[\],]+\s+%s\(" % cls)
    if not decl.search(text):
        return text
    new = cls + "Impl"
    renames[cls] = new
    return re.sub(r"(?<![\w.])(?<!new )%s(?=\()" % cls, new, text)


def fix_boo_enumerators(text):
    """Boo generator expressions decompile to an Enumerator class with
    `override` members that have nothing to override."""
    if "System_002ECollections_002EIEnumerator_002ECurrent" not in text:
        return text
    text = text.replace(
        "public override object System_002ECollections_002EIEnumerator_002ECurrent",
        "object System.Collections.IEnumerator.Current",
    )
    text = re.sub(r"public override (\w+) Current\b", r"public \1 Current", text)
    text = text.replace("return get_Current();", "return Current;")
    text = text.replace("public virtual object get_Current()", "public virtual object GetCurrentObject()")
    return text


# Straight textual API replacements for Unity 4 APIs that were removed without
# an automatic upgrade path.
API_RULES = [
    # AddComponent("Foo") -> AddComponent<Foo>()
    (re.compile(r"\.AddComponent\(\"(\w+)\"\)"), r".AddComponent<\1>()"),
    # iOS-only device query; on Android it always reported Unknown. Mapped to
    # NFPortLegacyDevice rather than UnityEngine.iOS, which only exists when
    # building for iOS.
    (re.compile(r"\biPhoneSettings\.generation\b"), "NFPortLegacyDevice.generation"),
    (re.compile(r"(?<![\w.])iPhoneGeneration\."), "NFPortLegacyDeviceGeneration."),
    # Moved from TouchScreenKeyboard to Screen.
    (re.compile(r"\bTouchScreenKeyboard\.(autorotateTo\w+)"), r"Screen.\1"),
    # GameObject.SampleAnimation(clip, t) -> clip.SampleAnimation(gameObject, t)
    (re.compile(r"\b(gameObject)\.SampleAnimation\((.+?), ([\w.]+)\);"), r"\2.SampleAnimation(\1, \3);"),
    # Only needed for the removed native ad SDK calling back into Java; the
    # Android JNI module isn't enabled (and doesn't exist off Android).
    (re.compile(r"(?m)^(\s*)AndroidJNI\.(?:Attach|Detach)CurrentThread\(\);"), r"\1// NFPort: AndroidJNI thread attach removed with the ad SDK"),
    # Cursor visibility.
    (re.compile(r"\bScreen\.(?:visible|showCursor)\b"), "Cursor.visible"),
    (re.compile(r"\bScreen\.lockCursor\s*=\s*true\b"), "Cursor.lockState = CursorLockMode.Locked"),
    (re.compile(r"\bScreen\.lockCursor\s*=\s*false\b"), "Cursor.lockState = CursorLockMode.None"),
]


# Unity 5 moved spring/damper from SoftJointLimit to SoftJointLimitSpring.
# ILSpy emits each property write as a read-modify-write of the struct:
#   SoftJointLimit a = j.linearLimit; float n = (a.spring = v); SoftJointLimit b = (j.linearLimit = a);
JOINT_SPRING = re.compile(
    r"SoftJointLimit (\w+) = ([\w.]+)\.(linearLimit|angularYLimit|angularZLimit|lowAngularXLimit|highAngularXLimit);"
    r"(\s*)float (\w+) = \(\1\.(spring|damper) = ([^;]+)\);"
    r"(\s*)SoftJointLimit (\w+) = \(\2\.\3 = \1\);"
)
SPRING_PROPERTY = {
    "linearLimit": "linearLimitSpring",
    "angularYLimit": "angularYZLimitSpring",
    "angularZLimit": "angularYZLimitSpring",
    "lowAngularXLimit": "angularXLimitSpring",
    "highAngularXLimit": "angularXLimitSpring",
}


def fix_joint_springs(text):
    def repl(m):
        local, joint, limit, ws1, num, field, value, ws2, copy = m.groups()
        prop = SPRING_PROPERTY[limit]
        return (
            "SoftJointLimitSpring %s = %s.%s;%sfloat %s = (%s.%s = %s);%sSoftJointLimitSpring %s = (%s.%s = %s);"
            % (local, joint, prop, ws1, num, local, field, value, ws2, copy, joint, prop, local)
        )

    return JOINT_SPRING.sub(repl, text)


def apply_api_rules(text):
    for pattern, repl in API_RULES:
        text = pattern.sub(repl, text)
    return fix_joint_springs(text)


# The Android build read assets/GameResources straight out of the APK:
#   new WWW("jar:file://" + Application.dataPath + "!/assets/" + path)  ... while (!w.isDone) {}
# Route those reads through NFPortAssetFile (a synchronous, WWW-shaped reader
# over the pack tools/nfport.py builds), which works on every platform.
APK_READ = re.compile(r'new WWW\("jar:file://" \+ Application\.dataPath \+ "!/assets/')

# The Android build hard-wired the touch input path; pick the Mac version's
# mouse path (still present in the scenes) on desktop, web and in the editor.
INPUT_RULES = [
    # FingerGesturesInitializer: gesture prefab per platform.
    (re.compile(r"\(\(!Application\.isEditor\) \? androidGestures : editorGestures\)"),
     "(NFPortInput.UseTouch ? androidGestures : (Application.isEditor ? editorGestures : desktopGestures))"),
    # GameManager: the cart controller (both live on the same GameObject).
    (re.compile(r"(\w+)\.GetComponent<TouchScreenController>\(\) as Controller(?!\))"),
     r"(NFPortInput.UseTouch ? \1.GetComponent<TouchScreenController>() as Controller : \1.GetComponent<KeyboardController>() as Controller)"),
    # "tap to continue" checks that only looked at touches.
    (re.compile(r"\(Input\.touchCount > 0 && Input\.GetTouch\(0\)\.phase == TouchPhase\.Ended\)"),
     "NFPortInput.TapEnded()"),
]


def retarget_platform(text):
    if APK_READ.search(text):
        text = APK_READ.sub('NFPortAssetFile.Open("', text)
        if "new WWW(" not in text:
            # Every WWW in this file held an APK read; retype them all.
            text = re.sub(r"(?<![\w.])WWW\b", "NFPortAssetFile", text)
        else:
            text = re.sub(r"\bWWW (\w+) = NFPortAssetFile\.Open\(", r"NFPortAssetFile \1 = NFPortAssetFile.Open(", text)
    for pattern, repl in INPUT_RULES:
        text = pattern.sub(repl, text)
    return text


def main(project):
    assets = pathlib.Path(project, "Assets")
    scripts = assets / "Scripts"

    # Both decompiled assemblies are merged into Assembly-CSharp, so their
    # AssemblyInfo attributes would clash.
    for info in scripts.glob("*/Properties/AssemblyInfo.cs"):
        info.unlink()
        meta = info.with_name(info.name + ".meta")
        if meta.exists():
            meta.unlink()

    renames = {}
    changed = 0
    files = [p for p in assets.rglob("*.cs") if "NFPort" not in p.parts]
    for path in files:
        text = path.read_text(encoding="utf-8-sig")
        new = strip_interface_virtual(text)
        new = rename_members_named_like_type(new, renames)
        new = fix_boo_enumerators(new)
        new = apply_api_rules(new)
        new = retarget_platform(new)
        if new != text:
            path.write_text(new, encoding="utf-8")
            changed += 1
    # Qualified calls from other files, e.g. DrawLine.DrawLine(...)
    for cls, new_name in renames.items():
        pattern = re.compile(r"\b%s\.%s\(" % (cls, cls))
        for path in files:
            text = path.read_text(encoding="utf-8-sig")
            if pattern.search(text):
                path.write_text(pattern.sub("%s.%s(" % (cls, new_name), text), encoding="utf-8")
                changed += 1
    print("codemods: %d files changed" % changed)


if __name__ == "__main__":
    if len(sys.argv) != 2:
        sys.exit(__doc__)
    main(sys.argv[1])
