using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SmartResults.Json;

public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Type == typeof(IError))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
            };

            foreach (var error in Error.GetErrors())
            {
                jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(error.Type, error.Key));
            }
        }

        return jsonTypeInfo;
    }
}
