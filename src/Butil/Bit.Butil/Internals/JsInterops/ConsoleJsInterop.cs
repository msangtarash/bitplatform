﻿using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Bit.Butil;

internal static class ConsoleJsInterop
{
    internal static async Task ConsoleAssert(this IJSRuntime js, bool? condition, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.assert", [condition, .. args]);

    internal static async Task ConsoleClear(this IJSRuntime js)
        => await js.InvokeVoidAsync("BitButil.console.clear");

    internal static async Task ConsoleCount(this IJSRuntime js, string? label)
        => await (label is null ? js.InvokeVoidAsync("BitButil.console.count")
                                : js.InvokeVoidAsync("BitButil.console.count", label));

    internal static async Task ConsoleCountReset(this IJSRuntime js, string? label)
        => await (label is null ? js.InvokeVoidAsync("BitButil.console.countReset")
                                : js.InvokeVoidAsync("BitButil.console.countReset", label));

    internal static async Task ConsoleDebug(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.debug", args);

    internal static async Task ConsoleDir(this IJSRuntime js, object? item, object? options)
        => await js.InvokeVoidAsync("BitButil.console.dir", item, options);

    internal static async Task ConsoleDirxml(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.dirxml", args);

    internal static async Task ConsoleError(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.error", args);

    internal static async Task ConsoleGroup(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.group", args);

    internal static async Task ConsoleGroupCollapsed(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.groupCollapsed", args);

    internal static async Task ConsoleGroupEnd(this IJSRuntime js)
        => await js.InvokeVoidAsync("BitButil.console.groupEnd");

    internal static async Task ConsoleInfo(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.info", args);

    internal static async Task ConsoleLog(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.log", args);

    internal static async Task ConsoleProfile(this IJSRuntime js, string? name)
        => await (name is null ? js.InvokeVoidAsync("BitButil.console.profile")
                               : js.InvokeVoidAsync("BitButil.console.profile", name));

    internal static async Task ConsoleProfileEnd(this IJSRuntime js, string? name)
        => await (name is null ? js.InvokeVoidAsync("BitButil.console.profileEnd")
                               : js.InvokeVoidAsync("BitButil.console.profileEnd", name));

    internal static async Task ConsoleTable(this IJSRuntime js, object? data, object? properties)
        => await (properties is null ? js.InvokeVoidAsync("BitButil.console.table", data)
                                     : js.InvokeVoidAsync("BitButil.console.table", data, properties));

    internal static async Task ConsoleTime(this IJSRuntime js, string? label)
        => await (label is null ? js.InvokeVoidAsync("BitButil.console.time")
                                : js.InvokeVoidAsync("BitButil.console.time", label));

    internal static async Task ConsoleTimeEnd(this IJSRuntime js, string? label)
        => await (label is null ? js.InvokeVoidAsync("BitButil.console.timeEnd")
                                : js.InvokeVoidAsync("BitButil.console.timeEnd", label));

    internal static async Task ConsoleTimeLog(this IJSRuntime js, string? label, params object?[]? args)
        => await (label is null ? js.InvokeVoidAsync("BitButil.console.timeLog")
                                : js.InvokeVoidAsync("BitButil.console.timeLog", [label, .. args]));

    internal static async Task ConsoleTimeStamp(this IJSRuntime js, string? label)
        => await (label is null ? js.InvokeVoidAsync("BitButil.console.timeStamp")
                                : js.InvokeVoidAsync("BitButil.console.timeStamp", label));

    internal static async Task ConsoleTrace(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.trace", args);

    internal static async Task ConsoleWarn(this IJSRuntime js, params object?[]? args)
        => await js.InvokeVoidAsync("BitButil.console.warn", args);
}
