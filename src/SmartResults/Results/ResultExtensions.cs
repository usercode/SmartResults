﻿// Copyright (c) usercode
// https://github.com/usercode/DragonFly
// MIT License

namespace SmartResults;

public static class ResultExtensions
{
    public static TResultOut Then<TResultIn, TResultOut>(this TResultIn result, Func<TResultIn, TResultOut> action)
        where TResultIn : IResult<TResultIn>
        where TResultOut : IResult<TResultOut>
    {
        if (result.IsOk)
        {
            return action(result);
        }
        else
        {
            return TResultOut.Failed(result.Error);
        }
    }

    public static async Task<TResultOut> ThenAsync<TResultIn, TResultOut>(this TResultIn result, Func<TResultIn, Task<TResultOut>> action)
        where TResultIn : IResult<TResultIn>
        where TResultOut : IResult<TResultOut>
    {
        if (result.IsOk)
        {
            return await action(result);
        }
        else
        {
            return TResultOut.Failed(result.Error);
        }
    }

    public static async Task<TResultOut> ThenAsync<TResultIn, TResultOut>(this Task<TResultIn> result, Func<TResultIn, TResultOut> action)
        where TResultIn : IResult<TResultIn>
        where TResultOut : IResult<TResultOut>
    {
        TResultIn r = await result;

        if (r.IsOk)
        {
            return action(r);
        }
        else
        {
            return TResultOut.Failed(r.Error);
        }
    }

    public static async Task<TResultOut> ThenAsync<TResultIn, TResultOut>(this Task<TResultIn> result, Func<TResultIn, Task<TResultOut>> action)
        where TResultIn : IResult<TResultIn>
        where TResultOut : IResult<TResultOut>
    {
        TResultIn r = await result;

        if (r.IsOk)
        {
            return await action(r);
        }
        else
        {
            return TResultOut.Failed(r.Error);
        }
    }
}
