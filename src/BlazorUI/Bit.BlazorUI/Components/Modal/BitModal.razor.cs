﻿namespace Bit.BlazorUI;

public partial class BitModal : IDisposable
{
    private bool IsOpenHasBeenSet;
    private bool isOpen;

    private bool _disposed;
    private int _offsetTop;
    private bool _isAlertRole;
    private bool _internalIsOpen;
    private string _containerId = default!;

    [Inject] private IJSRuntime _js { get; set; } = default!;


    /// <summary>
    /// Enables the auto scrollbar toggle behavior of the Modal.
    /// </summary>
    [Parameter] public bool AutoToggleScroll { get; set; } = true;

    /// <summary>
    /// When true, the Modal will be positioned absolute instead of fixed.
    /// </summary>
    [Parameter] public bool AbsolutePosition { get; set; }

    /// <summary>
    /// The content of the Modal, it can be any custom tag or text.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Custom CSS classes for different parts of the BitModal component.
    /// </summary>
    [Parameter] public BitModalClassStyles? Classes { get; set; }

    /// <summary>
    /// The CSS selector of the drag element. by default it's the Modal container.
    /// </summary>
    [Parameter] public string? DragElementSelector { get; set; }

    /// <summary>
    /// Determines the ARIA role of the Modal (alertdialog/dialog). If this is set, it will override the ARIA role determined by IsBlocking and IsModeless.
    /// </summary>
    [Parameter]
    public bool? IsAlert
    {
        get => _isAlertRole;
        set
        {
            _isAlertRole = value ?? (IsBlocking && !IsModeless);
        }
    }

    /// <summary>
    /// Whether the modal can be light dismissed by clicking outside the Modal (on the overlay).
    /// </summary>
    [Parameter] public bool IsBlocking { get; set; }

    /// <summary>
    /// Whether the Modal can be dragged around.
    /// </summary>
    [Parameter] public bool IsDraggable { get; set; }

    /// <summary>
    /// Whether the Modal should be modeless (e.g. not dismiss when focusing/clicking outside of the Modal). if true: IsBlocking is ignored, there will be no overlay.
    /// </summary>
    [Parameter] public bool IsModeless { get; set; }

    /// <summary>
    /// Whether the Modal is displayed.
    /// </summary>
    [Parameter]
    public bool IsOpen
    {
        get => isOpen;
        set
        {
            if (value == isOpen) return;

            isOpen = value;

            _ = IsOpenChanged.InvokeAsync(value);
        }
    }

    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// A callback function for when the Modal is dismissed light dismiss, before the animation completes.
    /// </summary>
    [Parameter] public EventCallback<MouseEventArgs> OnDismiss { get; set; }

    /// <summary>
    /// Position of the modal on the screen.
    /// </summary>
    [Parameter] public BitModalPosition Position { get; set; } = BitModalPosition.Center;

    /// <summary>
    /// Set the element selector for which the Modal disables its scroll if applicable.
    /// </summary>
    [Parameter] public string ScrollerSelector { get; set; } = "body";

    /// <summary>
    /// Custom CSS styles for different parts of the BitModal component.
    /// </summary>
    [Parameter] public BitModalClassStyles? Styles { get; set; }

    /// <summary>
    /// ARIA id for the subtitle of the Modal, if any.
    /// </summary>
    [Parameter] public string? SubtitleAriaId { get; set; }

    /// <summary>
    /// ARIA id for the title of the Modal, if any.
    /// </summary>
    [Parameter] public string? TitleAriaId { get; set; }


    protected override string RootElementClass => "bit-mdl";

    protected override void RegisterCssClasses()
    {
        ClassBuilder.Register(() => Classes?.Root);

        ClassBuilder.Register(() => AbsolutePosition ? $"{RootElementClass}-abs" : string.Empty);
    }

    protected override void RegisterCssStyles()
    {
        StyleBuilder.Register(() => Styles?.Root);

        StyleBuilder.Register(() => _offsetTop > 0 ? $"top:{_offsetTop}px" : string.Empty);
    }

    protected override Task OnInitializedAsync()
    {
        _containerId = $"BitModal-{UniqueId}-container";

        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (_internalIsOpen == IsOpen) return;

        _internalIsOpen = IsOpen;

        if (IsOpen)
        {
            if (IsDraggable)
            {
                _ = _js.SetupDragDrop(_containerId, GetDragElementSelector());
            }
            else
            {
                _ = _js.RemoveDragDrop(_containerId, GetDragElementSelector());
            }
        }
        else
        {
            _ = _js.RemoveDragDrop(_containerId, GetDragElementSelector());
        }

        _offsetTop = 0;

        if (AutoToggleScroll is false) return;
        
        _offsetTop = await _js.ToggleOverflow(ScrollerSelector, IsOpen);

        if (AbsolutePosition is false) return;

        StyleBuilder.Reset();
        StateHasChanged();
    }

    private void CloseModal(MouseEventArgs e)
    {
        if (IsEnabled is false) return;
        if (IsBlocking is not false) return;
        if (IsOpenHasBeenSet && IsOpenChanged.HasDelegate is false) return;

        IsOpen = false;

        _ = OnDismiss.InvokeAsync(e);
    }

    private string GetPositionClass() => Position switch
    {
        BitModalPosition.Center => $"{RootElementClass}-ctr",

        BitModalPosition.TopLeft => $"{RootElementClass}-tl",
        BitModalPosition.TopCenter => $"{RootElementClass}-tc",
        BitModalPosition.TopRight => $"{RootElementClass}-tr",

        BitModalPosition.CenterLeft => $"{RootElementClass}-cl",
        BitModalPosition.CenterRight => $"{RootElementClass}-cr",

        BitModalPosition.BottomLeft => $"{RootElementClass}-bl",
        BitModalPosition.BottomCenter => $"{RootElementClass}-bc",
        BitModalPosition.BottomRight => $"{RootElementClass}-br",

        _ => $"{RootElementClass}-ctr",
    };

    private string GetDragElementSelector() => DragElementSelector ?? $"#{_containerId}";


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed || disposing is false) return;

        _ = _js.RemoveDragDrop(_containerId, GetDragElementSelector());

        _disposed = true;
    }
}
