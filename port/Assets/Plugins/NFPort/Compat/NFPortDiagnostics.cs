#if !NFPORT_NO_DIAGNOSTICS
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Testing aid for the port.
///
/// * Writes NFPort.log: every error/exception (with stack trace), every scene
///   load, and for every click/tap all colliders under the pointer, nearest
///   first. On Windows/Linux the file is next to the game executable, in the
///   editor it is in the project folder, on phones in Application.persistentDataPath.
/// * When an error is logged a red badge appears top-left; tap it (or press
///   F12, or tap with four fingers) to see recent errors and the last tap.
/// Define NFPORT_NO_DIAGNOSTICS in Player Settings to remove all of this.
/// </summary>
public class NFPortDiagnostics : MonoBehaviour
{
    const int MaxEntries = 12;

    static StreamWriter _file;
    static readonly object FileLock = new object();
    public static string LogPath { get; private set; }

    readonly List<string> _entries = new List<string>();
    readonly object _lock = new object();
    int _errorCount;
    bool _open;
    string _lastTap = "(no tap yet)";
    Vector2 _scroll;
    GUIStyle _label;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Install()
    {
        OpenFile();
        var go = new GameObject("NFPort Diagnostics");
        DontDestroyOnLoad(go);
        go.AddComponent<NFPortDiagnostics>();
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            Write($"scene loaded: {scene.name} ({mode})");
            foreach (var cam in Camera.allCameras)
                Write("  camera " + DescribeCamera(cam));
        };
    }

    static string DescribeCamera(Camera cam) =>
        $"'{HierarchyPath(cam.transform)}' tag={cam.tag} depth={cam.depth} clear={cam.clearFlags} " +
        $"culling=0x{cam.cullingMask:X} events=0x{cam.eventMask:X} ortho={cam.orthographic} size={cam.orthographicSize} " +
        $"near={cam.nearClipPlane} far={cam.farClipPlane} pos={cam.transform.position:F3} rect={cam.pixelRect} " +
        $"sort={cam.transparencySortMode} rt={(cam.targetTexture != null ? cam.targetTexture.name : "-")}";

    static void OpenFile()
    {
        try
        {
            var dir = Application.isMobilePlatform || Application.platform == RuntimePlatform.WebGLPlayer
                ? Application.persistentDataPath
                : Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            LogPath = Path.Combine(dir, "NFPort.log");
            _file = new StreamWriter(LogPath, false) { AutoFlush = true };
            Write($"NFPort log, Unity {Application.unityVersion}, {Application.platform}, {SystemInfo.operatingSystem}, " +
                  $"{SystemInfo.graphicsDeviceType}, screen {Screen.width}x{Screen.height}");
        }
        catch (Exception e)
        {
            Debug.LogWarning("NFPort: could not open log file: " + e.Message);
        }
    }

    public static void Write(string line)
    {
        lock (FileLock)
            _file?.WriteLine($"[{DateTime.Now:HH:mm:ss.fff} f{Time.frameCount}] {line}");
    }

    void OnEnable() => Application.logMessageReceivedThreaded += OnLog;
    void OnDisable() => Application.logMessageReceivedThreaded -= OnLog;

    void OnLog(string message, string stackTrace, LogType type)
    {
        var isError = type == LogType.Error || type == LogType.Exception || type == LogType.Assert;
        Write(isError ? $"{type}: {message}\n{stackTrace}" : $"{type}: {message}");
        if (!isError) return;

        var firstFrame = string.IsNullOrEmpty(stackTrace) ? "" : stackTrace.Split('\n')[0];
        var entry = $"[{type}] {message}\n    at {firstFrame}";
        lock (_lock)
        {
            _errorCount++;
            if (_entries.Count > 0 && _entries[_entries.Count - 1] == entry) return;
            _entries.Add(entry);
            if (_entries.Count > MaxEntries) _entries.RemoveAt(0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12) || Input.touchCount == 4 && Input.GetTouch(3).phase == TouchPhase.Began)
            _open = !_open;

        if (Input.GetMouseButtonDown(0))
        {
            _lastTap = DescribeTap(Input.mousePosition);
            Write("tap " + _lastTap);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) Write("Escape pressed");
    }

    static string DescribeTap(Vector3 position)
    {
        var text = $"({position.x:0},{position.y:0}) of {Screen.width}x{Screen.height}";
        var cam = Camera.main;
        if (cam == null) return text + ": no main camera";
        var ray = cam.ScreenPointToRay(position);
        var hits = Physics.RaycastAll(ray, 1000f);
        if (hits.Length == 0) return text + ": hit nothing" + DescribeMiss(cam, ray);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        var parts = new List<string>();
        for (var i = 0; i < hits.Length && i < 5; i++)
        {
            var go = hits[i].collider.gameObject;
            var scripts = new List<string>();
            foreach (var mb in go.GetComponents<MonoBehaviour>())
                if (mb != null) scripts.Add(mb.GetType().Name + (mb.enabled ? "" : " (disabled)"));
            parts.Add($"'{HierarchyPath(go.transform)}' d={hits[i].distance:0.00} [{string.Join(", ", scripts)}]");
        }
        return text + ": " + string.Join("  >  ", parts);
    }

    /// <summary>
    /// Why did a tap hit nothing? Lists every collider whose transform says it
    /// is under the pointer, with what physics actually has for it.
    /// </summary>
    static string DescribeMiss(Camera cam, Ray ray)
    {
        var sb = new StringBuilder();
        sb.Append($"\n    camera {DescribeCamera(cam)}");
        sb.Append($"\n    ray {ray.origin:F3} dir {ray.direction:F3}; autoSyncTransforms={Physics.autoSyncTransforms} " +
                  $"timeScale={Time.timeScale} cameras={Camera.allCamerasCount}");
        var anyLayer = Physics.RaycastAll(ray, 1000f, ~0, QueryTriggerInteraction.Collide);
        if (anyLayer.Length > 0)
            sb.Append($"\n    (all layers + triggers would hit '{HierarchyPath(anyLayer[0].collider.transform)}' layer {anyLayer[0].collider.gameObject.layer})");

        var listed = 0;
        foreach (var col in UnityEngine.Object.FindObjectsOfType<Collider>(true))
        {
            if (col is BoxCollider box)
            {
                if (!ExpectedBounds(box).IntersectRay(ray)) continue;
            }
            else if (!col.bounds.IntersectRay(ray)) continue;

            if (++listed > 8) { sb.Append("\n    ..."); break; }
            var go = col.gameObject;
            var direct = col.enabled && go.activeInHierarchy && col.Raycast(ray, out _, 1000f);
            sb.Append($"\n    under pointer: '{HierarchyPath(col.transform)}' activeSelf={go.activeSelf} " +
                      $"activeInHierarchy={go.activeInHierarchy} enabled={col.enabled} trigger={col.isTrigger} " +
                      $"layer={go.layer} lossyScale={col.transform.lossyScale:F3} physicsBounds={col.bounds.center:F3}+-{col.bounds.extents:F3} " +
                      $"colliderRaycast={direct}");
            if (col is BoxCollider b)
            {
                var e = ExpectedBounds(b);
                sb.Append($" expectedBounds={e.center:F3}+-{e.extents:F3}");
            }
            for (var p = col.transform; p != null; p = p.parent)
                if (!p.gameObject.activeSelf) { sb.Append($" inactiveParent='{p.name}'"); break; }
        }
        if (listed == 0) sb.Append("\n    no collider's transform puts it under the pointer");
        return sb.ToString();
    }

    /// <summary>World AABB of a box collider computed from its transform alone.</summary>
    static Bounds ExpectedBounds(BoxCollider box)
    {
        var t = box.transform;
        var h = box.size * 0.5f;
        var bounds = new Bounds(t.TransformPoint(box.center), Vector3.zero);
        for (var i = 0; i < 8; i++)
        {
            var corner = box.center + new Vector3((i & 1) == 0 ? -h.x : h.x, (i & 2) == 0 ? -h.y : h.y, (i & 4) == 0 ? -h.z : h.z);
            bounds.Encapsulate(t.TransformPoint(corner));
        }
        return bounds;
    }

    static string HierarchyPath(Transform t)
    {
        var name = t.name;
        for (var p = t.parent; p != null && name.Length < 80; p = p.parent) name = p.name + "/" + name;
        return name;
    }

    void OnGUI()
    {
        if (_label == null)
            _label = new GUIStyle(GUI.skin.label) { fontSize = Mathf.Max(12, Screen.height / 45), wordWrap = true };

        int count;
        lock (_lock) count = _errorCount;
        var unit = Screen.height / 12f;

        if (!_open)
        {
            if (count == 0) return;
            var old = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUI.Button(new Rect(4, 4, unit * 3, unit * 0.6f), $"{count} error(s)")) _open = true;
            GUI.backgroundColor = old;
            return;
        }

        var area = new Rect(8, 8, Screen.width - 16, Screen.height * 0.6f);
        // The default skin's box is translucent; two layers keep text readable over the game.
        GUI.Box(area, GUIContent.none);
        GUI.Box(area, GUIContent.none);
        GUILayout.BeginArea(new Rect(area.x + 8, area.y + 8, area.width - 16, area.height - 16));
        GUILayout.BeginHorizontal();
        GUILayout.Label($"NFPort diagnostics: {count} error(s)", _label);
        if (GUILayout.Button("Close", GUILayout.Width(unit * 2), GUILayout.Height(unit * 0.6f))) _open = false;
        GUILayout.EndHorizontal();
        GUILayout.Label("Log file: " + (LogPath ?? "(not available)"), _label);
        GUILayout.Label("Last tap " + _lastTap, _label);
        _scroll = GUILayout.BeginScrollView(_scroll);
        lock (_lock)
            for (var i = _entries.Count - 1; i >= 0; i--) GUILayout.Label(_entries[i], _label);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
#endif
