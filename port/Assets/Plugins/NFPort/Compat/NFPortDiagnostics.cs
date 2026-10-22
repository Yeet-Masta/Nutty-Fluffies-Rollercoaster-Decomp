#if !NFPORT_NO_DIAGNOSTICS
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// On-screen error log for testing the port on devices without adb.
///
/// When the game logs an error or exception, a small red badge appears in
/// the top-left corner; tap it (or press F12, or tap with four fingers) to
/// open a panel with the recent errors and what the last tap/click hit.
/// Define NFPORT_NO_DIAGNOSTICS in Player Settings to remove it.
/// </summary>
public class NFPortDiagnostics : MonoBehaviour
{
    const int MaxEntries = 12;

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
        var go = new GameObject("NFPort Diagnostics");
        DontDestroyOnLoad(go);
        go.AddComponent<NFPortDiagnostics>();
    }

    void OnEnable() => Application.logMessageReceivedThreaded += OnLog;
    void OnDisable() => Application.logMessageReceivedThreaded -= OnLog;

    void OnLog(string message, string stackTrace, LogType type)
    {
        if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
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

        if (Input.GetMouseButtonDown(0)) _lastTap = DescribeTap(Input.mousePosition);
    }

    static string DescribeTap(Vector3 position)
    {
        var text = $"({position.x:0},{position.y:0}) of {Screen.width}x{Screen.height}";
        var cam = Camera.main;
        if (cam == null) return text + ": no main camera";
        var ray = cam.ScreenPointToRay(position);
        if (!Physics.Raycast(ray, out var hit, 1000f)) return text + ": hit nothing";
        var go = hit.collider.gameObject;
        var scripts = new List<string>();
        foreach (var mb in go.GetComponents<MonoBehaviour>())
            if (mb != null) scripts.Add(mb.GetType().Name + (mb.enabled ? "" : " (disabled)"));
        return $"{text}: hit '{go.name}' [{string.Join(", ", scripts)}]";
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
        GUILayout.Label("Last tap " + _lastTap, _label);
        _scroll = GUILayout.BeginScrollView(_scroll);
        lock (_lock)
            for (var i = _entries.Count - 1; i >= 0; i--) GUILayout.Label(_entries[i], _label);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
#endif
