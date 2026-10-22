// Replacement for the parts of UnityScript.Lang.dll used by the decompiled
// game scripts (the JavaScript-style Array, `for (x in y)` helpers and the
// parseInt/parseFloat built-ins).
using System;
using System.Collections;
using System.Globalization;
using System.Text;
using Boo.Lang.Runtime;

namespace UnityScript.Lang
{
    /// <summary>The dynamic, untyped JavaScript-style array of UnityScript.</summary>
    [Serializable]
    public class Array : CollectionBase, ICoercible
    {
        public delegate int Comparison(object lhs, object rhs);

        public Array() { }

        public Array(int capacity)
        {
            for (var i = 0; i < capacity; i++) InnerList.Add(null);
        }

        public Array(IEnumerable collection)
        {
            if (collection != null) foreach (var item in collection) InnerList.Add(item);
        }

        public Array(params object[] items) : this((IEnumerable)items) { }

        public static implicit operator Array(System.Array a) => new Array((IEnumerable)a);

        public int length
        {
            get => InnerList.Count;
            set
            {
                while (InnerList.Count < value) InnerList.Add(null);
                if (InnerList.Count > value) InnerList.RemoveRange(value, InnerList.Count - value);
            }
        }

        public object this[int index]
        {
            get => InnerList[index];
            set => InnerList[index] = value;
        }

        public virtual object Coerce(Type toType)
        {
            if (toType.IsArray) return ToBuiltin(toType.GetElementType());
            return this;
        }

        public System.Array ToBuiltin(Type type)
        {
            var result = System.Array.CreateInstance(type, InnerList.Count);
            for (var i = 0; i < InnerList.Count; i++)
                result.SetValue(RuntimeServices.Coerce(InnerList[i], type), i);
            return result;
        }

        public void clear() => Clear();

        public void Add(object value) => InnerList.Add(value);

        public void Add(object value, params object[] items)
        {
            InnerList.Add(value);
            InnerList.AddRange(items);
        }

        public void AddRange(IEnumerable collection)
        {
            foreach (var item in collection) InnerList.Add(item);
        }

        public int Push(object value) => InnerList.Add(value) + 1;

        public int Push(object value, params object[] items)
        {
            Add(value, items);
            return InnerList.Count;
        }

        public int push(object value) => Push(value);
        public int push(object value, params object[] items) => Push(value, items);

        public object Pop()
        {
            var last = InnerList.Count - 1;
            var value = InnerList[last];
            InnerList.RemoveAt(last);
            return value;
        }

        public object pop() => Pop();

        public object Shift()
        {
            var value = InnerList[0];
            InnerList.RemoveAt(0);
            return value;
        }

        public object shift() => Shift();

        public int Unshift(object value, params object[] items)
        {
            InnerList.InsertRange(0, items);
            InnerList.Insert(0, value);
            return InnerList.Count;
        }

        public int unshift(object value, params object[] items) => Unshift(value, items);

        public void Splice(int index, int howmany, params object[] items)
        {
            InnerList.RemoveRange(index, howmany);
            InnerList.InsertRange(index, items);
        }

        public void splice(int index, int howmany, params object[] items) => Splice(index, howmany, items);

        public Array Concat(ICollection value, params object[] items)
        {
            var result = new Array((IEnumerable)InnerList);
            result.InnerList.AddRange(value);
            foreach (var item in items)
            {
                if (item is ICollection c) result.InnerList.AddRange(c);
                else result.InnerList.Add(item);
            }
            return result;
        }

        public Array concat(ICollection value, params object[] items) => Concat(value, items);

        public Array Slice(int start) => Slice(start, InnerList.Count);

        public Array Slice(int start, int end)
        {
            if (start < 0) start += InnerList.Count;
            if (end < 0) end += InnerList.Count;
            return new Array((IEnumerable)InnerList.GetRange(start, Math.Max(0, end - start)));
        }

        public Array slice(int start) => Slice(start);
        public Array slice(int start, int end) => Slice(start, end);

        public Array Reverse()
        {
            InnerList.Reverse();
            return this;
        }

        public Array reverse() => Reverse();

        public Array Sort()
        {
            InnerList.Sort();
            return this;
        }

        public Array sort() => Sort();

        public void Sort(Comparison comparison) => InnerList.Sort(new ComparisonComparer(comparison));
        public void sort(Comparison comparison) => Sort(comparison);

        public string Join(string separator)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < InnerList.Count; i++)
            {
                if (i > 0) sb.Append(separator);
                sb.Append(InnerList[i]);
            }
            return sb.ToString();
        }

        public string join(string separator) => Join(separator);

        public void Remove(object obj) => InnerList.Remove(obj);
        public void remove(object obj) => Remove(obj);

        public override string ToString() => Join(",");
        public string toString() => ToString();

        sealed class ComparisonComparer : IComparer
        {
            readonly Comparison _comparison;
            public ComparisonComparer(Comparison comparison) => _comparison = comparison;
            public int Compare(object x, object y) => _comparison(x, y);
        }
    }

    public static class Extensions
    {
        public static int get_length(System.Array a) => a.Length;
        public static int get_length(string s) => s.Length;
    }

    public static class UnityBuiltins
    {
        // UnityScript parsed with the invariant culture in practice (Mono on
        // Android ran with it); keep that behaviour on devices with other locales.
        public static int parseInt(string value) => int.Parse(value, CultureInfo.InvariantCulture);
        public static int parseInt(float value) => checked((int)value);
        public static int parseInt(double value) => checked((int)value);
        public static int parseInt(int value) => value;
        public static float parseFloat(string value) => float.Parse(value, CultureInfo.InvariantCulture);
        public static float parseFloat(float value) => value;
        public static float parseFloat(double value) => (float)value;
        public static float parseFloat(int value) => value;
    }

    /// <summary>Enumerator over an IList that supports in-place update of the current item.</summary>
    public class ListUpdateableEnumerator : IEnumerator
    {
        readonly IList _list;
        int _index = -1;

        public ListUpdateableEnumerator(IList list) => _list = list;

        public object Current => _list[_index];
        public bool MoveNext() => ++_index < _list.Count;
        public void Reset() => _index = -1;
        public void Update(object newValue) => _list[_index] = newValue;
    }

    public static class UnityRuntimeServices
    {
        static readonly IEnumerator Empty = new object[0].GetEnumerator();

        public static IEnumerator GetEnumerator(object obj)
        {
            switch (obj)
            {
                case null:
                    return Empty;
                // Like UnityScript: only its own Array and arrays of value types
                // write loop-variable changes back; other collections use their
                // own enumerator (and Update is a no-op for them).
                case Array jsArray:
                    return new ListUpdateableEnumerator(jsArray);
                case System.Array array when array.GetType().GetElementType().IsValueType:
                    return new ListUpdateableEnumerator(array);
                case IEnumerable enumerable:
                    return enumerable.GetEnumerator();
                case IEnumerator enumerator:
                    return enumerator;
                default:
                    throw new ArgumentException($"{obj.GetType()} is not enumerable.");
            }
        }

        public static void Update(IEnumerator e, object newValue)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e is ListUpdateableEnumerator updateable) updateable.Update(newValue);
        }
    }
}
