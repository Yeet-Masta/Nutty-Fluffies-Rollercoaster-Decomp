// Minimal stand-in for the Prime31 P31RestKit.dll the game shipped with
// (a .NET 2.0 binary). Only the API the game and the stubbed SDK bindings
// reference is provided: the manager/GUI base classes, a JSON codec, and an
// inert REST client base (its only user, the Facebook client, is stubbed).
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Prime31
{
    public enum HTTPVerb
    {
        GET,
        POST,
        PUT
    }

    /// <summary>Base for SDK event managers that received UnitySendMessage callbacks.</summary>
    public class AbstractManager : MonoBehaviour
    {
        public static void initialize(Type type)
        {
            if (FindObjectOfType(type) != null) return;
            var go = new GameObject(type.Name);
            go.AddComponent(type);
            DontDestroyOnLoad(go);
        }
    }

    /// <summary>Base class of the Prime31 demo GUIs.</summary>
    public class MonoBehaviourGUI : MonoBehaviour
    {
    }

    /// <summary>Inert REST client base; the SDKs that used it were removed.</summary>
    public class P31RestKit
    {
        protected string _baseUrl;
        public bool debugRequests;
        protected bool forceJsonResponse;

        protected virtual GameObject surrogateGameObject => null;
        protected MonoBehaviour surrogateMonobehaviour => null;

        protected virtual IEnumerator send(string path, HTTPVerb httpVerb, Dictionary<string, object> parameters, Action<string, object> onComplete)
        {
            onComplete?.Invoke("not available", null);
            yield break;
        }

        public void get(string path, Action<string, object> completionHandler) => completionHandler?.Invoke("not available", null);
        public void get(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler) => get(path, completionHandler);
        public void post(string path, Action<string, object> completionHandler) => get(path, completionHandler);
        public void post(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler) => get(path, completionHandler);
        public void put(string path, Action<string, object> completionHandler) => get(path, completionHandler);
        public void put(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler) => get(path, completionHandler);
    }

    /// <summary>Null-safe event raising used by the SDK managers.</summary>
    public static class ActionExtensions
    {
        public static void fire(this Action handler) => handler?.Invoke();
        public static void fire<T>(this Action<T> handler, T param) => handler?.Invoke(param);
        public static void fire<T, U>(this Action<T, U> handler, T param1, U param2) => handler?.Invoke(param1, param2);
        public static void fire<T, U, V>(this Action<T, U, V> handler, T param1, U param2, V param3) =>
            handler?.Invoke(param1, param2, param3);
    }

    public static class Utils
    {
        public static void logObject(object obj) => Debug.Log(MiniJSON.jsonEncode(obj));
    }

    /// <summary>
    /// JSON codec. By default objects decode to Hashtable and arrays to
    /// ArrayList (what the game expects); with generic containers they decode
    /// to Dictionary&lt;string, object&gt; and List&lt;object&gt;.
    /// </summary>
    public static class MiniJSON
    {
        public static object jsonDecode(string json) => jsonDecode(json, false);

        public static object jsonDecode(string json, bool decodeUsingGenericContainers)
        {
            if (json == null) return null;
            var parser = new Parser(json, decodeUsingGenericContainers);
            try
            {
                var value = parser.ParseValue();
                parser.SkipWhitespace();
                return parser.AtEnd ? value : null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static string jsonEncode(object obj)
        {
            var sb = new StringBuilder();
            return Serialize(obj, sb) ? sb.ToString() : null;
        }

        public static object deserialize(string json) => jsonDecode(json, true);
        public static object deserialize(string json, bool useGenericContainers) => jsonDecode(json, useGenericContainers);

        static bool Serialize(object value, StringBuilder sb)
        {
            switch (value)
            {
                case null:
                    sb.Append("null");
                    return true;
                case string s:
                    SerializeString(s, sb);
                    return true;
                case bool b:
                    sb.Append(b ? "true" : "false");
                    return true;
                case char c:
                    SerializeString(c.ToString(), sb);
                    return true;
                case IDictionary dict:
                    sb.Append('{');
                    var first = true;
                    foreach (DictionaryEntry e in dict)
                    {
                        if (!first) sb.Append(',');
                        first = false;
                        SerializeString(e.Key.ToString(), sb);
                        sb.Append(':');
                        if (!Serialize(e.Value, sb)) return false;
                    }
                    sb.Append('}');
                    return true;
                case IEnumerable list:
                    sb.Append('[');
                    var firstItem = true;
                    foreach (var item in list)
                    {
                        if (!firstItem) sb.Append(',');
                        firstItem = false;
                        if (!Serialize(item, sb)) return false;
                    }
                    sb.Append(']');
                    return true;
                case float f:
                    sb.Append(f.ToString("R", CultureInfo.InvariantCulture));
                    return true;
                case double d:
                    sb.Append(d.ToString("R", CultureInfo.InvariantCulture));
                    return true;
                case IConvertible n when value.GetType().IsPrimitive || value is decimal:
                    sb.Append(n.ToString(CultureInfo.InvariantCulture));
                    return true;
                default:
                    SerializeString(value.ToString(), sb);
                    return true;
            }
        }

        static void SerializeString(string s, StringBuilder sb)
        {
            sb.Append('"');
            foreach (var c in s)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < ' ' || c > 126) sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
        }

        sealed class Parser
        {
            readonly string _json;
            readonly bool _generic;
            int _pos;

            public Parser(string json, bool generic)
            {
                _json = json;
                _generic = generic;
            }

            public bool AtEnd => _pos >= _json.Length;

            public void SkipWhitespace()
            {
                while (_pos < _json.Length && char.IsWhiteSpace(_json[_pos])) _pos++;
            }

            char Peek()
            {
                SkipWhitespace();
                if (AtEnd) throw new FormatException("Unexpected end of JSON.");
                return _json[_pos];
            }

            void Expect(char c)
            {
                if (Peek() != c) throw new FormatException($"Expected '{c}' at {_pos}.");
                _pos++;
            }

            public object ParseValue()
            {
                var c = Peek();
                switch (c)
                {
                    case '{': return ParseObject();
                    case '[': return ParseArray();
                    case '"': return ParseString();
                    case 't': return ParseLiteral("true", true);
                    case 'f': return ParseLiteral("false", false);
                    case 'n': return ParseLiteral("null", null);
                    default:
                        if (c == '-' || char.IsDigit(c)) return ParseNumber();
                        throw new FormatException($"Unexpected '{c}' at {_pos}.");
                }
            }

            object ParseLiteral(string word, object value)
            {
                if (string.CompareOrdinal(_json, _pos, word, 0, word.Length) != 0)
                    throw new FormatException($"Bad literal at {_pos}.");
                _pos += word.Length;
                return value;
            }

            object ParseObject()
            {
                Expect('{');
                IDictionary result = _generic ? (IDictionary)new Dictionary<string, object>() : new Hashtable();
                if (Peek() == '}')
                {
                    _pos++;
                    return result;
                }
                while (true)
                {
                    if (Peek() != '"') throw new FormatException($"Expected key at {_pos}.");
                    var key = ParseString();
                    Expect(':');
                    result[key] = ParseValue();
                    var c = Peek();
                    _pos++;
                    if (c == '}') return result;
                    if (c != ',') throw new FormatException($"Expected ',' or '}}' at {_pos - 1}.");
                }
            }

            object ParseArray()
            {
                Expect('[');
                IList result = _generic ? (IList)new List<object>() : new ArrayList();
                if (Peek() == ']')
                {
                    _pos++;
                    return result;
                }
                while (true)
                {
                    result.Add(ParseValue());
                    var c = Peek();
                    _pos++;
                    if (c == ']') return result;
                    if (c != ',') throw new FormatException($"Expected ',' or ']' at {_pos - 1}.");
                }
            }

            string ParseString()
            {
                Expect('"');
                var sb = new StringBuilder();
                while (_pos < _json.Length)
                {
                    var c = _json[_pos++];
                    if (c == '"') return sb.ToString();
                    if (c != '\\')
                    {
                        sb.Append(c);
                        continue;
                    }
                    if (_pos >= _json.Length) break;
                    var e = _json[_pos++];
                    switch (e)
                    {
                        case 'b': sb.Append('\b'); break;
                        case 'f': sb.Append('\f'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case 'u':
                            if (_pos + 4 > _json.Length) throw new FormatException("Bad \\u escape.");
                            sb.Append((char)int.Parse(_json.Substring(_pos, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                            _pos += 4;
                            break;
                        default: sb.Append(e); break;
                    }
                }
                throw new FormatException("Unterminated string.");
            }

            object ParseNumber()
            {
                var start = _pos;
                while (_pos < _json.Length && "+-0123456789.eE".IndexOf(_json[_pos]) >= 0) _pos++;
                var text = _json.Substring(start, _pos - start);
                // Prime31's MiniJSON returned every number as a double.
                return double.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);
            }
        }
    }

    public static class MiniJsonExtensions
    {
        public static string toJson(this IDictionary obj) => MiniJSON.jsonEncode(obj);
        public static string toJson(this IList obj) => MiniJSON.jsonEncode(obj);
        public static ArrayList arrayListFromJson(this string json) => MiniJSON.jsonDecode(json) as ArrayList;
        public static Hashtable hashtableFromJson(this string json) => MiniJSON.jsonDecode(json) as Hashtable;
        public static Dictionary<string, object> dictionaryFromJson(this string json) => MiniJSON.jsonDecode(json, true) as Dictionary<string, object>;
        public static List<object> listFromJson(this string json) => MiniJSON.jsonDecode(json, true) as List<object>;
    }
}
