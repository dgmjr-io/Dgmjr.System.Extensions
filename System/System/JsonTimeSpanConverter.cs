// namespace System;

// using System.Text.Json;
// using System.Text.Json.Serialization;

// public class JsonIntegerToTimeSpanConverter : JsonConverter<TimeSpan>
// {
//     public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         if (reader.TokenType == JsonTokenType.Number)
//         {
//             return TimeSpan.FromSeconds(reader.GetInt64());
//         }
//         throw new JsonException();
//     }

//     public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
//     {
//         writer.WriteNumberValue(value.TotalSeconds);
//     }
// }
