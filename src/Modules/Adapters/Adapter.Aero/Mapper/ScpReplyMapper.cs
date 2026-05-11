using System;
using System.Collections;
using System.Reflection;
using Adapter.Abstraction.Interfaces;

namespace Adapter.Aero.Mapper;


public sealed class ScpReplyMapper : IObjectMapper
{
    public TDestination Map<TDestination>(object source)
    {
        return (TDestination)MapObject(source, typeof(TDestination));
    }

    private object? MapObject(object? source, Type destinationType)
    {
        if (source == null)
            return null;

        var sourceType = source.GetType();

        // ⭐ Handle Nullable<T>
        destinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

        // 1) Primitive / string / decimal / DateTime / Guid
        if (IsSimple(destinationType))
            return ConvertSimple(source, destinationType);

        // 2) Enum support (source enum/int → destination enum)
        if (destinationType.IsEnum)
            return Enum.ToObject(destinationType, source);

        // 3) Arrays
        if (destinationType.IsArray)
        {
            var elementType = destinationType.GetElementType()!;
            var sourceArray = (IEnumerable)source;

            var list = new List<object?>();
            foreach (var item in sourceArray)
                list.Add(MapObject(item, elementType));

            var array = Array.CreateInstance(elementType, list.Count);
            for (int i = 0; i < list.Count; i++)
                array.SetValue(list[i], i);

            return array;
        }

        // ⭐ 4) COMPLEX / NESTED OBJECT
        var destination = Activator.CreateInstance(destinationType)!;

        var sourceMembers = sourceType
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Cast<MemberInfo>()
            .Concat(sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance));

        var destProps = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destProp in destProps)
        {
            // ⭐ MATCH BY NAME (ignore DTO suffix)
            var sourceMember = sourceMembers.FirstOrDefault(m =>
                NormalizeName(m.Name) == NormalizeName(destProp.Name));

            if (sourceMember == null)
                continue;

            object? sourceValue = sourceMember switch
            {
                FieldInfo f => f.GetValue(source),
                PropertyInfo p => p.GetValue(source),
                _ => null
            };

            if (sourceValue == null)
                continue;

            var mappedValue = MapObject(sourceValue, destProp.PropertyType);
            destProp.SetValue(destination, mappedValue);
        }

        return destination;
    }

    // ⭐ Remove "Dto" suffix for matching nested classes
    private string NormalizeName(string name)
        => name.Replace("Dto", "").ToLower();

    private bool IsSimple(Type type)
    {
        return type.IsPrimitive
            || type == typeof(string)
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(Guid);
    }

    private object ConvertSimple(object value, Type destinationType)
    {
        if (destinationType == typeof(string))
            return value.ToString()!;

        return Convert.ChangeType(value, destinationType);
    }
}