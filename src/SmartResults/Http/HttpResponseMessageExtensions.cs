// Copyright (c) usercode
// https://github.com/usercode/DragonFly
// MIT License

using System.Net.Http.Json;
using System.Text.Json;

namespace SmartResults;

public static class HttpResponseMessageExtensions
{
    public static async Task<Result> ReadResultFromJsonAsync(this Task<HttpResponseMessage> responseTask, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return await (await responseTask.ConfigureAwait(false)).ReadResultFromJsonAsync(jsonSerializerOptions).ConfigureAwait(false);
    }

    public static async Task<Result> ReadResultFromJsonAsync(this HttpResponseMessage response, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Result result = await response.Content.ReadFromJsonAsync<Result>(jsonSerializerOptions).ConfigureAwait(false);

        return result;
    }

    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(this Task<HttpResponseMessage> responseTask, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return await (await responseTask.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(jsonSerializerOptions).ConfigureAwait(false);
    }

    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(this HttpResponseMessage response, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Result<T> result = await response.Content.ReadFromJsonAsync<Result<T>>(jsonSerializerOptions).ConfigureAwait(false);

        return result;
    }
}
