using System;
using System.Collections.Generic;
using UnityEditor;

public abstract class Node : XNode.Node {
    private static readonly Dictionary<Type, object> ConverterCache = new Dictionary<Type, object>();

    /// <summary>
    /// Intelligent version of the original that calls internal converters
    /// for dynamic type casting (e.g. int -> float).
    ///
    /// This also allows converting more complex types like Transforms to Quaternions.
    /// </summary>
    /// <param name="fieldName">Field name of requested input port</param>
    /// <param name="fallback">If no ports are connected, this value will be returned</param>
    public new T GetInputValue<T>(string fieldName, T fallback = default) {
        var port = GetPort(fieldName);

        if (port == null || !port.IsConnected) {
            return fallback;
        }

        var value = port.GetInputValue();

        if (value is T cast) {
            return cast;
        }

#if UNITY_2019_2_OR_NEWER
        return TryConvertValue(value, out T converted) ? converted : fallback;
    }

    private static bool TryConvertValue<T>(object input, out T output) {
        var type = typeof(Converter<,>).MakeGenericType(input.GetType(), typeof(T));

        if (!ConverterCache.TryGetValue(type, out var instance)) {
            var converters = TypeCache.GetTypesDerivedFrom(type);

            if (converters.Count <= 0) {
                output = default;
                return false;
            }

            instance = Activator.CreateInstance(converters[0]);
            ConverterCache[type] = instance;
        }

        var val = ((Converter) instance).Convert(input);
        output = (T) val;
        return true;
#else
        return fallback;
#endif
    }
}

