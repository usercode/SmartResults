// Copyright (c) usercode
// https://github.com/usercode/DragonFly
// MIT License

using System.Net.Http.Json;
using System.Text.Json;

namespace SmartResults;

public static class HttpResponseMessageExtensions
{
    public static async Task<Result> ToResultAsync(this Task<HttpResponseMessage> responseTask, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return await (await responseTask).ToResultAsync(jsonSerializerOptions);
    }

    public static async Task<Result> ToResultAsync(this HttpResponseMessage response, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Result result = await response.Content.ReadFromJsonAsync<Result>(jsonSerializerOptions);

        return result;
    }

    public static async Task<Result<T>> ToResultAsync<T>(this Task<HttpResponseMessage> responseTask, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return await (await responseTask).ToResultAsync<T>(jsonSerializerOptions);
    }

    public static async Task<Result<T>> ToResultAsync<T>(this HttpResponseMessage response, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Result<T> result = await response.Content.ReadFromJsonAsync<Result<T>>(jsonSerializerOptions);

        return result;
    }
}
