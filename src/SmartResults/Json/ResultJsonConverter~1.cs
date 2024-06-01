using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartResults.Json;

internal class ResultJsonConverter<T> : JsonConverter<Result<T>>
{
    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        bool? succeeded = null;
        string? message = null;
        T? value = default;

        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    string? property = reader.GetString();

                    if (property == options.ConvertName(JsonPropertyConstants.MessageProperty))
                    {
                        reader.Read();
                        message = reader.GetString();
                    }
                    else if (property == options.ConvertName(JsonPropertyConstants.SucceededProperty))
                    {
                        reader.Read();
                        succeeded = reader.GetBoolean();
                    }
                    else if (property == options.ConvertName(JsonPropertyConstants.ValueProperty))
                    {
                        reader.Read();
                        value = JsonSerializer.Deserialize<T>(ref reader, options);
                    }
                    break;

                case JsonTokenType.EndObject:
                    break;
            }
        }

        if (succeeded == null)
        {
            throw new ArgumentNullException(nameof(succeeded));
        }

        if (succeeded == true)
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
        writer.WriteBoolean(options.ConvertName(JsonPropertyConstants.SucceededProperty), value.IsSucceeded);

        if (value.IsSucceeded)
        {
            writer.WritePropertyName(options.ConvertName(JsonPropertyConstants.ValueProperty));
            JsonSerializer.Serialize(writer, value.Value, options);
        }
        else
        {
            writer.WriteString(options.ConvertName(JsonPropertyConstants.MessageProperty), value.Error.Message);
        }
        writer.WriteEndObject();
    }
}
