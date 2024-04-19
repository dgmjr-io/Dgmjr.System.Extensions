using System;

namespace System.Text.Json;

public class JsonConverterAttribute<T>(params object[] converterParameters)
    : JConverterAttribute(typeof(T))
{
    public object[] ConverterParameters { get; init; } = converterParameters;

    public override JConverter? CreateConverter(type typeToConvert)
    {
        return Activator.CreateInstance(ConverterType, ConverterParameters) as JConverter;
    }
}
