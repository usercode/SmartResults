using System.Text.Json;

namespace SmartResults.Json;

public static class JsonPropertyConstants
{
    public const string SucceededProperty = "Succeeded";
    public const string ValueProperty = "Value";
    public const string MessageProperty = "Message";

    public static string ConvertName(this JsonSerializerOptions options, string name)
    {
        if (options.PropertyNamingPolicy != null)
        {
            return options.PropertyNamingPolicy.ConvertName(name);
        }
        else
        {

            return name;
        }
    }
}
