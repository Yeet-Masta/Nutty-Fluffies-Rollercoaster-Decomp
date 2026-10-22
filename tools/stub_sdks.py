#!/usr/bin/env python3
"""Rip out the defunct third-party SDKs (Amazon IAP, Google IAB, Twitter,
Facebook, Flurry, Tapjoy, Pushwoosh, Game Center, Prime31 Etcetera, Ubisoft
cross-promotion/AppPerse...).

Their Java/Objective-C halves are not part of the port (and were ARMv7-only
or dead services anyway), so every member body in the C# binding classes is
replaced with a no-op that returns a default value, and `extern` P/Invokes
become managed stubs. The public API is kept, so the game code that calls
these bindings compiles unchanged and simply gets "not available" answers.

Usage: stub_sdks.py <unity-project-dir>
"""
import pathlib
import re
import sys

FIRSTPASS = "Assets/Plugins/Assembly-CSharp-firstpass"
SDK_FILES = [
    "AmazonIAP.cs",
    "AppPerseBinding.cs",
    "CrossPromotionAndroid.cs",
    "CrossPromotionBinding.cs",
    "EtceteraAndroid.cs",
    "Facebook.cs",
    "FacebookAndroid.cs",
    "FlurryAndroid.cs",
    "FlurryBinding.cs",
    "GameCenterBinding.cs",
    "GoogleIAB.cs",
    "IABAndroid.cs",
    "IABUIManager.cs",
    "LoadingDialog.cs",
    "PushNotificationsAndroid.cs",
    "TapjoyPluginAndroid.cs",
    "TwitterAndroid.cs",
    "TwitterBinding.cs",
]
MARKER = "// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py\n"

TYPE_DECL = re.compile(r"\b(class|struct|interface|enum|namespace)\b")
ACCESSOR = re.compile(r"^(?:\[[^\]]*\]\s*)*(?:(?:public|private|protected|internal)\s+)*(get|set|add|remove)$")
MODIFIERS = {
    "public", "private", "protected", "internal", "static", "virtual", "override",
    "abstract", "sealed", "extern", "unsafe", "new", "readonly", "async", "partial",
}


def scan_blocks(text):
    """Yield (open_index, close_index) for every brace pair, skipping braces
    inside string/char literals and comments."""
    stack, i, n = [], 0, len(text)
    while i < n:
        c = text[i]
        if text.startswith("//", i):
            i = text.find("\n", i)
            i = n if i < 0 else i
            continue
        if text.startswith("/*", i):
            i = text.find("*/", i + 2)
            i = n if i < 0 else i + 2
            continue
        if c == "@" and i + 1 < n and text[i + 1] == '"':
            i += 2
            while i < n:
                if text[i] == '"' and i + 1 < n and text[i + 1] == '"':
                    i += 2
                    continue
                if text[i] == '"':
                    break
                i += 1
        elif c in "\"'":
            i += 1
            while i < n and text[i] != c:
                i += 2 if text[i] == "\\" else 1
        elif c == "{":
            stack.append(i)
        elif c == "}" and stack:
            yield stack.pop(), i
        i += 1


def header_before(text, open_idx):
    """Text of the declaration that owns the block starting at open_idx."""
    j = open_idx - 1
    while j >= 0 and text[j] not in ";{}":
        j -= 1
    header = text[j + 1:open_idx]
    # Drop leading attributes/comments for classification.
    lines = [l.strip() for l in header.strip().split("\n")]
    lines = [l for l in lines if l and not l.startswith("//") and not l.startswith("[")]
    return " ".join(lines)


def return_type(header):
    """Best-effort return type of a method header, None for constructors."""
    before = header.split("(", 1)[0].strip()
    tokens = re.findall(r"[\w.]+(?:<[^()]*?>)?(?:\[\])*\??", before)
    tokens = [t for t in tokens if t not in MODIFIERS]
    if len(tokens) < 2:
        return None  # constructor / static constructor
    return tokens[-2]


def out_params(header):
    params = header.split("(", 1)[1].rsplit(")", 1)[0] if "(" in header else ""
    return re.findall(r"\bout\s+[\w.<>\[\],]+\s+(\w+)", params)


def stub_for(header):
    acc = ACCESSOR.match(header)
    if acc:
        return "{ return default; }" if acc.group(1) == "get" else "{ }"
    if "(" not in header or TYPE_DECL.search(header.split("(", 1)[0]):
        return None
    if re.search(r"\b(if|for|foreach|while|switch|using|lock|catch|try|else|do|fixed|checked|unchecked)\b\s*\(?", header.split("(", 1)[0].strip()) and not re.search(r"\b(public|private|protected|internal|static)\b", header):
        return None
    if "=>" in header or header.endswith("=") or "new " in header.split("(", 1)[0]:
        return None
    ret = return_type(header)
    assigns = "".join("%s = default; " % p for p in out_params(header))
    if ret is None or ret == "void":
        return "{ %s}" % assigns if assigns else "{ }"
    if ret in ("IEnumerator", "IEnumerable") or ret.startswith(("IEnumerator<", "IEnumerable<")):
        return "{ %syield break; }" % assigns
    return "{ %sreturn default; }" % assigns


def stub_text(text):
    blocks = sorted(scan_blocks(text))
    # Only stub outermost member bodies: skip blocks nested inside a stubbed one.
    edits, covered_until = [], -1
    for open_idx, close_idx in blocks:
        if open_idx < covered_until:
            continue
        stub = stub_for(header_before(text, open_idx))
        if stub is not None:
            edits.append((open_idx, close_idx, stub))
            covered_until = close_idx
    for open_idx, close_idx, stub in reversed(edits):
        text = text[:open_idx] + stub + text[close_idx + 1:]

    # [DllImport("__Internal")] static extern T f(...);  ->  static T f(...) { stub }
    def extern_repl(m):
        indent, decl = m.group(1), m.group(2)
        header = decl.replace(" extern", "")
        return "%s%s %s" % (indent, header, stub_for(header) or "{ }")

    text = re.sub(r"(?m)^\s*\[DllImport\([^\]]*\)\]\s*\n", "", text)
    text = re.sub(r"(?m)^(\s*)((?:\w+\s+)*extern\s+[^;]*\))\s*;", extern_repl, text)
    # Expression-bodied members that reach into the SDK.
    text = re.sub(r"(?m)^(\s*(?:(?:public|private|protected|internal|static)\s+)+[\w.<>\[\]]+\s+\w+\s*(?:\([^)]*\))?\s*)=>\s*[^;]+;",
                  r"\1=> default;", text)
    # The leftover fields/signatures typed AndroidJavaObject/AndroidJavaClass
    # would need Unity's optional Android JNI module (and only exist to talk
    # to the removed Java SDKs), so they become plain objects.
    text = re.sub(r"\b(?:UnityEngine\.)?AndroidJava(?:Object|Class)\b", "object", text)
    return text


def main(project):
    root = pathlib.Path(project, FIRSTPASS)
    for name in SDK_FILES:
        path = root / name
        if not path.exists():
            print("  missing (skipped): %s" % name)
            continue
        text = path.read_text(encoding="utf-8-sig")
        if text.startswith(MARKER):
            continue
        path.write_text(MARKER + stub_text(text), encoding="utf-8")
        print("  stubbed %s" % name)


if __name__ == "__main__":
    if len(sys.argv) != 2:
        sys.exit(__doc__)
    main(sys.argv[1])
