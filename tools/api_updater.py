#!/usr/bin/env python3
"""Compiler-driven Unity API updater.

Reads Roslyn diagnostics (from `dotnet build` of the compile-check projects in
tools/compilecheck) and rewrites the exact tokens Unity marks as
"(UnityUpgradable)", the same way Unity's own Script Updater does:

  * "Property renderer has been deprecated. Use GetComponent<Renderer>() instead."
        foo.renderer  ->  foo.GetComponent<Renderer>()
  * "... (UnityUpgradable) -> newName"
        foo.oldName   ->  foo.newName

It also fixes a few UnityScript-isms that the C# compiler rejects:

  * Component.active (removed)            -> component.gameObject.active
  * protected member accessed through a base-typed qualifier (allowed by
    UnityScript)                          -> member made public
  * char == "x" (UnityScript compared a char with a 1-char string)
                                          -> c.ToString() == "x"
  * go.AddComponent(someString)           -> go.AddComponentByName(someString)

Usage: api_updater.py <project-root> <build-log> [<build-log> ...]
Prints the number of edits applied; run the build again until it reports 0.
"""
import collections
import pathlib
import re
import sys

DIAG = re.compile(
    r"^(?P<file>.+?\.cs)\((?P<line>\d+),(?P<col>\d+)\): error (?P<code>CS\d+): (?P<msg>.*?)(?: \[.*\])?$"
)
GETCOMPONENT = re.compile(r"Use GetComponent<(\w+)>\(\) instead")
RENAME = re.compile(r"\(UnityUpgradable\) -> (?:\[[\w.]+\] )?(?:[\w.]+\.)?(\w+)$")
OLDNAME = re.compile(r"^'(?:[\w.<>]+\.)?(\w+)' is obsolete")


NO_ACTIVE = re.compile(r"^'(\w+)' does not contain a definition for 'active'")
PROTECTED = re.compile(r"^Cannot access protected member '(\w+)\.(\w+)\(")
CHAR_STRING = re.compile(r"^Operator '(==|!=)' cannot be applied to operands of type 'char' and 'string'")


def collect(logs):
    edits = collections.defaultdict(dict)
    publicize = set()
    for log in logs:
        for raw in pathlib.Path(log).read_text(encoding="utf-8", errors="replace").splitlines():
            m = DIAG.match(raw.strip())
            if not m:
                continue
            msg = m["msg"]
            path = pathlib.Path(m["file"])
            pos = (int(m["line"]), int(m["col"]))
            if m["code"] == "CS1061" and NO_ACTIVE.match(msg) and NO_ACTIVE.match(msg).group(1) != "GameObject":
                edits[path][pos + ("active",)] = "gameObject.active"
                continue
            if m["code"] == "CS1540" and PROTECTED.match(msg):
                publicize.add(PROTECTED.match(msg).groups())
                continue
            if m["code"] == "CS0019" and CHAR_STRING.match(msg):
                edits[path][pos + (CHAR_STRING.match(msg).group(1),)] = "CHAR_STRING"
                continue
            if m["code"] == "CS0619" and msg.startswith("'GameObject.AddComponent(string)'"):
                # Runtime-chosen script names; resolved by NFPortCompat.
                edits[path][pos + ("AddComponent",)] = "AddComponentByName"
                continue
            if m["code"] != "CS0619" or "UnityUpgradable" not in msg:
                continue
            gc = GETCOMPONENT.search(msg)
            rn = RENAME.search(msg.rstrip("'"))
            if gc:
                repl = "GetComponent<%s>()" % gc.group(1)
            elif rn and "(*)" not in msg:
                repl = rn.group(1)
            else:
                continue
            old = OLDNAME.match(msg)
            if not old or old.group(1) == repl:
                continue
            # Roslyn reports the start of the whole member-access expression,
            # so remember the old member name and locate it from there.
            key = (int(m["line"]), int(m["col"]), old.group(1))
            edits[path][key] = repl
    return edits, publicize


def make_public(root, publicize):
    count = 0
    for cls, member in publicize:
        for path in pathlib.Path(root, "Assets").rglob("*.cs"):
            text = path.read_text(encoding="utf-8-sig")
            # The declaring class, plus any overrides (which must match its access).
            if re.search(r"\bclass %s\b" % cls, text):
                new = re.sub(r"\bprotected (?=(?:virtual |override |static )?[\w<>\[\],]+ %s\()" % member, "public ", text)
            else:
                new = re.sub(r"\bprotected (?=override [\w<>\[\],]+ %s\()" % member, "public ", text)
            if new != text:
                path.write_text(new, encoding="utf-8")
                count += 1
    return count


def apply(edits):
    count = 0
    for path, by_pos in edits.items():
        text = path.read_text(encoding="utf-8-sig")
        starts = [0]
        for ln in text.split("\n"):
            starts.append(starts[-1] + len(ln) + 1)
        for (line, col, old), repl in sorted(by_pos.items(), reverse=True):
            if repl == "CHAR_STRING":
                # Roslyn points at the left operand; wrap it in ToString().
                lhs = re.compile(r"[\w.\[\]]+(?=\s*%s)" % re.escape(old)).match(text, starts[line - 1] + col - 1)
                if lhs:
                    text = text[:lhs.end()] + ".ToString()" + text[lhs.end():]
                    count += 1
                continue
            tok = re.compile(r"(?<![\w.])%s\b|(?<=\.)\s*%s\b" % (old, old)).search(text, starts[line - 1] + col - 1)
            if not tok:
                continue
            a = tok.start() + len(tok.group()) - len(old)
            text = text[:a] + repl + text[a + len(old):]
            count += 1
        path.write_text(text, encoding="utf-8")
    return count


if __name__ == "__main__":
    if len(sys.argv) < 3:
        sys.exit(__doc__)
    edits, publicize = collect(sys.argv[2:])
    print(apply(edits) + make_public(sys.argv[1], publicize))
