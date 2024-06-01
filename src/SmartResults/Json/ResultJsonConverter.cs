using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartResults.Json;

internal class ResultJsonConverter : JsonConverter<Result>
{
    public override Result Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        bool? succeeded = null;
        string? message = null;

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
            return Result.Ok();
        }
        else
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return Result.Failed(new Error(message));
        }
    }

    public override void Write(Utf8JsonWriter writer, Result value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteBoolean(options.ConvertName(JsonPropertyConstants.SucceededProperty), value.IsSucceeded);

        if (value.IsFailed)
        {
            writer.WriteString(options.ConvertName(JsonPropertyConstants.MessageProperty), value.Error.Message);
        }

        writer.WriteEndObject();
    }
}
