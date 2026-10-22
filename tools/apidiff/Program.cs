// Lists Unity API members referenced by the original (Unity 4.0) game assemblies whose
// signature differs in Unity 2022: the recompiled decompiled code may silently change meaning.
using Mono.Cecil;
using Mono.Cecil.Cil;

var gameDir = args[0]; var modulesDir = args[1];
var current = new Dictionary<string, TypeDefinition>();
foreach (var f in Directory.GetFiles(modulesDir, "UnityEngine*.dll"))
{
    var m = ModuleDefinition.ReadModule(f);
    foreach (var t in m.GetTypes()) if (t.IsPublic || t.IsNestedPublic) current.TryAdd(t.FullName, t);
}
string Sig(MethodReference m) => m.ReturnType.FullName + " " + m.Name + "(" + string.Join(",", m.Parameters.Select(p => p.ParameterType.FullName)) + ")";
var seen = new Dictionary<string, List<string>>();
foreach (var f in new[] { "Assembly-UnityScript.dll", "Assembly-CSharp.dll", "Assembly-CSharp-firstpass.dll", "Assembly-UnityScript-firstpass.dll" })
{
    var path = Path.Combine(gameDir, f); if (!File.Exists(path)) continue;
    var mod = ModuleDefinition.ReadModule(path);
    foreach (var type in mod.GetTypes())
    foreach (var method in type.Methods.Where(x => x.HasBody))
    foreach (var ins in method.Body.Instructions)
    {
        string key = null, problem = null;
        if (ins.Operand is MethodReference mr && mr.DeclaringType.Scope.Name.StartsWith("UnityEngine"))
        {
            var dt = mr.DeclaringType.GetElementType().FullName;
            key = dt + "::" + Sig(mr);
            if (!current.TryGetValue(dt, out var ct)) problem = "type gone";
            else
            {
                var same = ct.Methods.Where(x => x.Name == mr.Name && x.Parameters.Count == mr.Parameters.Count).ToList();
                // walk base types for inherited members
                for (var b = ct.BaseType; same.Count == 0 && b != null && current.TryGetValue(b.FullName, out var bt); b = bt.BaseType)
                    same = bt.Methods.Where(x => x.Name == mr.Name && x.Parameters.Count == mr.Parameters.Count).ToList();
                var exact = same.FirstOrDefault(x => Enumerable.Range(0, x.Parameters.Count).All(i => x.Parameters[i].ParameterType.FullName == mr.Parameters[i].ParameterType.FullName));
                if (same.Count == 0) problem = "member gone";
                else if (exact == null) problem = "params now: " + string.Join(" | ", same.Select(Sig));
                else if (exact.ReturnType.FullName != mr.ReturnType.FullName && !exact.ReturnType.IsGenericParameter) problem = "returns now " + exact.ReturnType.FullName;
            }
        }
        else if (ins.Operand is FieldReference fr && fr.DeclaringType.Scope.Name.StartsWith("UnityEngine"))
        {
            var dt = fr.DeclaringType.GetElementType().FullName;
            key = dt + "::field " + fr.FieldType.FullName + " " + fr.Name;
            if (!current.TryGetValue(dt, out var ct)) problem = "type gone";
            else
            {
                var cf = ct.Fields.FirstOrDefault(x => x.Name == fr.Name);
                var cp = ct.Properties.FirstOrDefault(x => x.Name == fr.Name);
                if (cf != null && cf.FieldType.FullName != fr.FieldType.FullName) problem = "field type now " + cf.FieldType.FullName;
                else if (cf == null && cp != null && cp.PropertyType.FullName != fr.FieldType.FullName) problem = "now property " + cp.PropertyType.FullName;
                else if (cf == null && cp == null) problem = "member gone";
            }
        }
        if (problem == null) continue;
        if (!seen.TryGetValue(key + "  =>  " + problem, out var where)) seen[key + "  =>  " + problem] = where = new List<string>();
        var loc = type.Name + "." + method.Name;
        if (!where.Contains(loc)) where.Add(loc);
    }
}
foreach (var kv in seen.OrderBy(k => k.Key))
    Console.WriteLine(kv.Key + "\n      used in: " + string.Join(", ", kv.Value.Take(8)) + (kv.Value.Count > 8 ? $" (+{kv.Value.Count - 8})" : ""));
