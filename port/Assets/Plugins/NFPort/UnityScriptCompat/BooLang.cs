// IL2CPP-safe stand-in for the parts of Boo.Lang.dll that the decompiled
// UnityScript code links against. The original assembly relies on
// System.Reflection.Emit for dynamic dispatch, which does not exist on
// AOT/IL2CPP targets (and ARM64 Android requires IL2CPP).
using System;
using System.Collections;
using System.Collections.Generic;

namespace Boo.Lang
{
    public interface ICallable
    {
        object Call(object[] args);
    }

    /// <summary>Base class of compiled UnityScript coroutines/generators.</summary>
    [Serializable]
    public abstract class GenericGenerator<T> : IEnumerable<T>
    {
        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString() => $"generator({typeof(T)})";
    }

    /// <summary>State machine base used by compiled UnityScript generators.</summary>
    [Serializable]
    public abstract class GenericGeneratorEnumerator<T> : IEnumerator<T>
    {
        protected T _current;
        public int _state;

        public T Current => _current;
        object IEnumerator.Current => _current;

        public abstract bool MoveNext();

        public virtual void Dispose() { }

        public void Reset() => throw new NotSupportedException();

        protected bool Yield(int state, T value)
        {
            _state = state;
            _current = value;
            return true;
        }

        protected bool YieldDefault(int state)
        {
            _state = state;
            _current = default;
            return true;
        }
    }

    public static class Builtins
    {
        public static object[] array(IEnumerable enumerable)
        {
            var items = new List<object>();
            foreach (var item in enumerable) items.Add(item);
            return items.ToArray();
        }

        public static System.Array array(Type elementType, ICollection collection)
        {
            var result = System.Array.CreateInstance(elementType, collection.Count);
            collection.CopyTo(result, 0);
            return result;
        }

        public static System.Array array(Type elementType, IEnumerable enumerable)
        {
            if (enumerable is ICollection collection) return array(elementType, collection);
            var items = new ArrayList();
            foreach (var item in enumerable) items.Add(item);
            return items.ToArray(elementType);
        }

        public static System.Array array(Type elementType, int length) =>
            System.Array.CreateInstance(elementType, length);

        public static T[] array<T>(int length) => new T[length];
    }
}
