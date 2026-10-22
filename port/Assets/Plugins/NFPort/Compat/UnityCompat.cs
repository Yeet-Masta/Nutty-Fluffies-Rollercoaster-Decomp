using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;

/// <summary>Helpers for Unity 4 APIs that have no direct modern equivalent.</summary>
public static class NFPortCompat
{
    static readonly Dictionary<string, Type> ComponentTypes = new Dictionary<string, Type>();

    /// <summary>
    /// Replacement for the removed GameObject.AddComponent(string). The game
    /// picks trigger/object scripts by name from its level data at runtime.
    /// </summary>
    public static Component AddComponentByName(this GameObject go, string className)
    {
        var type = FindComponentType(className);
        if (type == null)
        {
            Debug.LogError($"AddComponent: no component type named '{className}'.");
            return null;
        }
        return go.AddComponent(type);
    }

    public static Type FindComponentType(string className)
    {
        if (string.IsNullOrEmpty(className)) return null;
        lock (ComponentTypes)
        {
            if (ComponentTypes.TryGetValue(className, out var cached)) return cached;
            Type found = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch (System.Reflection.ReflectionTypeLoadException e) { types = e.Types; }
                foreach (var t in types)
                {
                    if (t == null || !typeof(Component).IsAssignableFrom(t)) continue;
                    if (t.Name == className || t.FullName == className)
                    {
                        found = t;
                        // Prefer the game's own scripts over engine types.
                        if (t.Assembly.GetName().Name.StartsWith("Assembly-CSharp")) break;
                    }
                }
                if (found != null && found.Assembly.GetName().Name.StartsWith("Assembly-CSharp")) break;
            }
            ComponentTypes[className] = found;
            return found;
        }
    }

    // The original game ran on Mono with the invariant culture; parsing of its
    // level/save data ("1.5") must not depend on the device locale.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void UseInvariantCulture()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    }
}
