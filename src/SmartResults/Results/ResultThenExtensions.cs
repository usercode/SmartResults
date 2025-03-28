﻿// Copyright (c) usercode
// https://github.com/usercode/DragonFly
// MIT License

namespace SmartResults;

public static class ResultThenExtensions
{
    /// <summary>
    /// If the result is succeeded, the action is executed.
    /// </summary>
    public static TResultOut Then<TResultIn, TResultOut>(this TResultIn result, Func<TResultIn, TResultOut> action)
        where TResultIn : IResult<TResultIn>
        where TResultOut : IResult<TResultOut>
    {
        if (result.IsSucceeded)
        {
            return action(result);
        }
        else
        {
            return TResultOut.Failed(result.Error);
        }
    }

    /// <summary>
    /// If the result is succeeded, the action is executed.
    /// </summary>
    public static async Task<TResultOut> ThenAsync<TResultIn, TResultOut>(this TResultIn result, Func<TResultIn, Task<TResultOut>> action)
        where TResultIn : IResult<TResultIn>
        where TResultOut : IResult<TResultOut>
    {
        if (result.IsSucceeded)
        {
            return await action(result).ConfigureAwait(false);
        }
        else
        {
            return TResultOut.Failed(result.Error);
        }
    }

    /// <summary>
    /// If the result is succeeded, the action is executed.
    /// </summary>
    public static async Task<TResultOut> ThenAsync<TResultIn, TResultOut>(this Task<TResultIn> result, Func<TResultIn, TResultOut> action)
        where TResultIn : IResult<TResultIn>
        where TResultOut : IResult<TResultOut>
    {
        TResultIn r = await result.ConfigureAwait(false);

        if (r.IsSucceeded)
        {
            return action(r);
        }
        else
        {
            return TResultOut.Failed(r.Error);
        }
    }

    /// <summary>
    /// If the result is succeeded, the action is executed.
    /// </summary>
    public static async Task<TResultOut> ThenAsync<TResultIn, TResultOut>(this Task<TResultIn> result, Func<TResultIn, Task<TResultOut>> action)
        where TResultIn : IResult<TResultIn>
        where TResultOut : IResult<TResultOut>
    {
        TResultIn r = await result.ConfigureAwait(false);

        if (r.IsSucceeded)
        {
            return await action(r).ConfigureAwait(false);
        }
        else
        {
            return TResultOut.Failed(r.Error);
        }
    }
}
