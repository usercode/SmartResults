using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartResults.Json;

internal class ResultJsonConverter : JsonConverter<Result>
{
    public override Result Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        bool? isSucceeded = null;
        string? message = null;

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
        writer.WriteBoolean("succeeded"u8, value.IsSucceeded);

        if (value.IsFailed)
        {
            writer.WriteString("message"u8, value.Error.Message);
        }

        writer.WriteEndObject();
    }
}
