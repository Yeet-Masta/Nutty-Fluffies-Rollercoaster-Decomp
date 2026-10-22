using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Cross-platform replacement for the game's level/theme/language file reads.
///
/// The Android build read <c>assets/GameResources/...</c> straight out of the
/// APK with <c>new WWW("jar:file://" + Application.dataPath + "!/assets/" + path)</c>
/// and spun on <c>isDone</c>. That URL only exists on Android, and spinning on
/// a download never finishes in a browser (WebGL is single-threaded), so the
/// files are packed into one TextAsset by tools/nfport.py
/// (Assets/Resources/NFPort/GameResources.bytes) and read synchronously here.
/// The members mirror the WWW API the game code uses.
/// </summary>
public sealed class NFPortAssetFile
{
    const string PackResource = "NFPort/GameResources";

    static byte[] _pack;
    static Dictionary<string, KeyValuePair<int, int>> _index;
    static Dictionary<string, string> _caseInsensitive;

    readonly byte[] _bytes;
    string _text;

    NFPortAssetFile(byte[] bytes, string error)
    {
        _bytes = bytes;
        this.error = error;
    }

    public bool isDone => true;
    public float progress => 1f;
    public string error { get; }
    public byte[] bytes => _bytes;
    public string text => _text ?? (_text = _bytes == null ? null : Decode(_bytes));

    /// <param name="path">Path relative to the APK's assets/ folder, e.g. "GameResources/language_de.txt".</param>
    public static NFPortAssetFile Open(string path)
    {
        if (!EnsureIndex())
            return new NFPortAssetFile(null, "NFPort: " + PackResource + " resource is missing; re-run tools/nfport.py port");

        var key = Normalize(path);
        if (!_index.TryGetValue(key, out var entry) &&
            !(_caseInsensitive.TryGetValue(key.ToLowerInvariant(), out var actual) && _index.TryGetValue(actual, out entry)))
            return new NFPortAssetFile(null, "NFPort: file not found: " + path);

        var data = new byte[entry.Value];
        Buffer.BlockCopy(_pack, entry.Key, data, 0, entry.Value);
        return new NFPortAssetFile(data, null);
    }

    public static bool Exists(string path) => EnsureIndex() && _index.ContainsKey(Normalize(path));

    static string Normalize(string path)
    {
        var p = (path ?? string.Empty).Replace('\\', '/');
        while (p.Contains("//")) p = p.Replace("//", "/");
        while (p.StartsWith("./", StringComparison.Ordinal)) p = p.Substring(2);
        return p.TrimStart('/');
    }

    static string Decode(byte[] data)
    {
        // UTF-8, skipping a byte-order mark like WWW.text did.
        if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            return Encoding.UTF8.GetString(data, 3, data.Length - 3);
        if (data.Length >= 2 && data[0] == 0xFF && data[1] == 0xFE)
            return Encoding.Unicode.GetString(data, 2, data.Length - 2);
        if (data.Length >= 2 && data[0] == 0xFE && data[1] == 0xFF)
            return Encoding.BigEndianUnicode.GetString(data, 2, data.Length - 2);
        return Encoding.UTF8.GetString(data);
    }

    /// <summary>
    /// Pack layout (little-endian): "NFPK", uint32 version (1), uint32 count,
    /// then per entry: uint16 path length, UTF-8 path, uint32 offset, uint32
    /// length; offsets are relative to the data that follows the table.
    /// </summary>
    static bool EnsureIndex()
    {
        if (_index != null) return true;

        var asset = Resources.Load<TextAsset>(PackResource);
        if (asset == null)
        {
            Debug.LogError("NFPort: missing Resources/" + PackResource + ".bytes");
            return false;
        }
        var pack = asset.bytes;
        Resources.UnloadAsset(asset);

        if (pack.Length < 12 || pack[0] != 'N' || pack[1] != 'F' || pack[2] != 'P' || pack[3] != 'K' || ReadUInt32(pack, 4) != 1)
        {
            Debug.LogError("NFPort: " + PackResource + " is not a valid pack");
            return false;
        }

        var count = (int)ReadUInt32(pack, 8);
        var entries = new List<KeyValuePair<string, KeyValuePair<int, int>>>(count);
        var pos = 12;
        for (var i = 0; i < count; i++)
        {
            int nameLength = pack[pos] | pack[pos + 1] << 8;
            pos += 2;
            var name = Encoding.UTF8.GetString(pack, pos, nameLength);
            pos += nameLength;
            var offset = (int)ReadUInt32(pack, pos);
            var length = (int)ReadUInt32(pack, pos + 4);
            pos += 8;
            entries.Add(new KeyValuePair<string, KeyValuePair<int, int>>(name, new KeyValuePair<int, int>(offset, length)));
        }

        var index = new Dictionary<string, KeyValuePair<int, int>>(count, StringComparer.Ordinal);
        var caseInsensitive = new Dictionary<string, string>(count, StringComparer.Ordinal);
        foreach (var e in entries)
        {
            index[e.Key] = new KeyValuePair<int, int>(pos + e.Value.Key, e.Value.Value);
            caseInsensitive[e.Key.ToLowerInvariant()] = e.Key;
        }
        _pack = pack;
        _caseInsensitive = caseInsensitive;
        _index = index;
        return true;
    }

    static uint ReadUInt32(byte[] b, int i) =>
        (uint)(b[i] | b[i + 1] << 8 | b[i + 2] << 16 | b[i + 3] << 24);
}
