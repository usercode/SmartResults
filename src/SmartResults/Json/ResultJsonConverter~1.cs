using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartResults.Json;

internal class ResultJsonConverter<T> : JsonConverter<Result<T>>
{
    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        bool? isSucceeded = null;
        string? message = null;
        T? value = default;

        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    if (reader.ValueSpan.SequenceEqual("message"u8))
                    {
                        reader.Read();
                        message = reader.GetString();
                    }
                    else if (reader.ValueSpan.SequenceEqual("succeeded"u8))
                    {
                        reader.Read();
                        isSucceeded = reader.GetBoolean();
                    }
                    else if (reader.ValueSpan.SequenceEqual("value"u8))
                    {
                        reader.Read();
                        value = JsonSerializer.Deserialize<T>(ref reader, options);
                    }
                    break;

                case JsonTokenType.EndObject:
                    break;
            }
        }

        if (isSucceeded == null)
        {
            throw new ArgumentNullException(nameof(isSucceeded));
        }

        if (isSucceeded == true)
        {
            return Result<T>.Ok(value!);
        }
        else
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return Result<T>.Failed(new Error(message));
        }
    }

    public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteBoolean("succeeded"u8, value.IsSucceeded);

        if (value.IsSucceeded)
        {
            writer.WritePropertyName("value"u8);
            JsonSerializer.Serialize(writer, value.Value, options);
        }
        else
        {
            writer.WriteString("message"u8, value.Error.Message);
        }
        writer.WriteEndObject();
    }
}
