// Reflection-only replacement for Boo.Lang.Runtime.RuntimeServices.
// Only the operations used by the decompiled game are implemented, and none of
// them generate code at runtime, so they are safe under IL2CPP.
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace Boo.Lang.Runtime
{
    public interface ICoercible
    {
        object Coerce(Type toType);
    }

    public static class RuntimeServices
    {
        public static object Coerce(object value, Type toType)
        {
            if (value == null || toType == null) return value;
            if (toType.IsInstanceOfType(value)) return value;
            if (value is ICoercible coercible) return coercible.Coerce(toType);

            var target = System.Nullable.GetUnderlyingType(toType) ?? toType;
            if (target.IsEnum && IsNumeric(value))
                return Enum.ToObject(target, value);
            if (IsNumericType(target) && (IsNumeric(value) || value is char || value is bool))
                return Convert.ChangeType(value, target, CultureInfo.InvariantCulture);
            if (target == typeof(char) && IsNumeric(value))
                return Convert.ToChar(value, CultureInfo.InvariantCulture);

            var sequence = value as IEnumerable;
            if (target.IsArray && sequence != null)
            {
                var elementType = target.GetElementType();
                var items = new ArrayList();
                foreach (var item in sequence) items.Add(Coerce(item, elementType));
                return items.ToArray(elementType);
            }

            var op = FindConversion(value.GetType(), value.GetType(), target)
                     ?? FindConversion(target, value.GetType(), target);
            if (op != null) return op.Invoke(null, new[] { value });

            throw new InvalidCastException($"Cannot coerce {value.GetType()} to {toType}.");
        }

        public static bool EqualityOperator(object lhs, object rhs)
        {
            if (lhs == rhs) return true;
            // Object.Equals is overridden by UnityEngine.Object so that a
            // destroyed object compares equal to null, as UnityScript expects.
            if (lhs == null) return rhs.Equals(null);
            if (rhs == null) return lhs.Equals(null);

            if ((IsNumeric(lhs) || lhs is char) && (IsNumeric(rhs) || rhs is char))
            {
                if (IsFloating(lhs) || IsFloating(rhs))
                    return Convert.ToDouble(lhs, CultureInfo.InvariantCulture) == Convert.ToDouble(rhs, CultureInfo.InvariantCulture);
                if (lhs is ulong || rhs is ulong)
                    return Convert.ToUInt64(lhs, CultureInfo.InvariantCulture) == Convert.ToUInt64(rhs, CultureInfo.InvariantCulture);
                return Convert.ToInt64(lhs, CultureInfo.InvariantCulture) == Convert.ToInt64(rhs, CultureInfo.InvariantCulture);
            }

            if (lhs is System.Array la && rhs is System.Array ra)
            {
                if (la.Length != ra.Length) return false;
                for (var i = 0; i < la.Length; i++)
                    if (!EqualityOperator(la.GetValue(i), ra.GetValue(i))) return false;
                return true;
            }

            var op = FindOperator(lhs.GetType(), "op_Equality", lhs.GetType(), rhs.GetType())
                     ?? FindOperator(rhs.GetType(), "op_Equality", lhs.GetType(), rhs.GetType());
            if (op != null) return (bool)op.Invoke(null, new[] { lhs, rhs });

            return lhs.Equals(rhs);
        }

        public static char UnboxChar(object value)
        {
            if (value is char c) return c;
            if (value is string s && s.Length == 1) return s[0];
            return Convert.ToChar(value, CultureInfo.InvariantCulture);
        }

        public static bool ToBool(object value)
        {
            if (value == null) return false;
            if (value is bool b) return b;
            if (value is string s) return s.Length != 0;
            if (IsNumeric(value)) return Convert.ToDouble(value, CultureInfo.InvariantCulture) != 0;
            return !value.Equals(null);
        }

        static bool IsFloating(object value) => value is float || value is double || value is decimal;

        static bool IsNumeric(object value) => value != null && IsNumericType(value.GetType());

        static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return !type.IsEnum;
                default:
                    return false;
            }
        }

        static MethodInfo FindConversion(Type declaring, Type from, Type to)
        {
            foreach (var m in declaring.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if ((m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.ReturnType == to)
                {
                    var p = m.GetParameters();
                    if (p.Length == 1 && p[0].ParameterType.IsAssignableFrom(from)) return m;
                }
            }
            return null;
        }

        static MethodInfo FindOperator(Type declaring, string name, Type lhs, Type rhs)
        {
            foreach (var m in declaring.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (m.Name != name) continue;
                var p = m.GetParameters();
                if (p.Length == 2 && p[0].ParameterType.IsAssignableFrom(lhs) && p[1].ParameterType.IsAssignableFrom(rhs))
                    return m;
            }
            return null;
        }
    }
}
