using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartResults.Json;

internal abstract class ResultJsonConverterBase<TResult> : JsonConverter<TResult>
    where TResult : IResult<TResult>
{
    protected static JsonSerializerOptions ErrorOptions = new() { TypeInfoResolver = new PolymorphicTypeResolver() };

}
