/*
 * JsonAnyTypeSerilalizer.cs
 *     Created: 2024-06-05T13:21:08-04:00
 *    Modified: 2024-06-05T13:21:08-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Text.Json;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonAnyTypeConverter<T> : JsonConverter<T>
    where T : new()
{
    public const string DefaultDiscriminator = "$type";

    protected virtual object Construct(type t) => Activator.CreateInstance(t);

    public override T? Read(ref Utf8JsonReader reader, type typeToConvert, Jso options)
    {
        var jObject = JDoc.ParseValue(ref reader).RootElement;
        var typeName = jObject.GetProperty("$type").GetString();
        var value = (T)Construct(typeToConvert);

        foreach (var property in jObject.EnumerateObject())
        {
            if (property.Name == DefaultDiscriminator)
            {
                continue;
            }

            var propertyType = type.GetType(typeName);
            var propertyValue = Deserialize(property.Value.ToString(), propertyType, options);
            var objectProperty = typeToConvert.GetProperty(property.Name);
            objectProperty.SetValue(value, propertyValue);
        }
        return value;
    }

    public override void Write(Utf8JsonWriter writer, T value, Jso options)
    {
        writer.WriteStartObject();
        var type = value.GetType();
        writer.WriteString(DefaultDiscriminator, type.AssemblyQualifiedName);
        foreach (var property in type.GetProperties())
        {
            var propertyValue = property.GetValue(value);
            writer.WritePropertyName(property.Name);
            Serialize(writer, propertyValue, property.PropertyType, options);
        }
        writer.WriteEndObject();
    }
}
