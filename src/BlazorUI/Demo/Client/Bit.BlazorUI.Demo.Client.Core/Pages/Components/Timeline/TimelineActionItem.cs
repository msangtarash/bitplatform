﻿namespace Bit.BlazorUI.Demo.Client.Core.Pages.Components.Timeline;

public class TimelineActionItem
{
    public string? Class { get; set; }

    public RenderFragment<TimelineActionItem>? DotContent { get; set; }

    public RenderFragment<TimelineActionItem>? FirstContent { get; set; }

    public string? FirstText { get; set; }

    public string? Icon { get; set; }

    public bool IsEnabled { get; set; } = true;

    public bool Opposite { get; set; }

    public RenderFragment<TimelineActionItem>? SecondContent { get; set; }

    public string? SecondText { get; set; }

    public string? Style { get; set; }
}
