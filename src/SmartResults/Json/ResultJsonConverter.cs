using System.Text.Json;

namespace SmartResults.Json;

internal class ResultJsonConverter : ResultJsonConverterBase<Result>
{
    public override Result Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        bool? succeeded = null;
        IError? error = null;

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
            return Result.Ok();
        }
        else
        {
            ArgumentNullException.ThrowIfNull(error);

            return Result.Failed(error);
        }
    }

    public override void Write(Utf8JsonWriter writer, Result value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteBoolean(options.ConvertName(JsonPropertyConstants.SucceededProperty), value.IsSucceeded);

        if (value.IsFailed)
        {
            writer.WritePropertyName(options.ConvertName(JsonPropertyConstants.ErrorProperty));
            JsonSerializer.Serialize(writer, value.Error, ErrorOptions);
        }

        writer.WriteEndObject();
    }
}
