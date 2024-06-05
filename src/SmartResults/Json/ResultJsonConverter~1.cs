using System.Text.Json;

namespace SmartResults.Json;

internal class ResultJsonConverter<T> : ResultJsonConverterBase<Result<T>>
{
    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        bool? succeeded = null;
        IError? error = null;
        T? value = default;

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string? property = reader.GetString();

                if (property == options.ConvertName(JsonPropertyConstants.SucceededProperty))
                {
                    reader.Read();
                    succeeded = reader.GetBoolean();
                }
                else if (property == options.ConvertName(JsonPropertyConstants.ValueProperty))
                {
                    reader.Read();
                    value = JsonSerializer.Deserialize<T>(ref reader, options);
                }
                else if (property == options.ConvertName(JsonPropertyConstants.ErrorProperty))
                {
                    reader.Read();
                    error = JsonSerializer.Deserialize<IError>(ref reader, ErrorOptions);
                }
            }
        }

        ArgumentNullException.ThrowIfNull(succeeded);

        if (succeeded == true)
        {
            return Result<T>.Ok(value!);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(error);

            return Result<T>.Failed(error);
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
            writer.WritePropertyName(options.ConvertName(JsonPropertyConstants.ErrorProperty));
            JsonSerializer.Serialize(writer, value.Error, ErrorOptions);
        }

        writer.WriteEndObject();
    }
}
