using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;

namespace ClassLibrary1;

public interface ICanvasContextDrawer
{
    void Begin();
    void DrawElement(ElementReference elementReference, double sx, double sy, double sWidth, double sHeight, double dx, double dy, double dWidth, double dHeight);
    void End();
    ValueTask Render(int width, int height);
}
public class CanvasContextDrawer : ICanvasContextDrawer
{
    const int MAX_DRAW_CALLS = 2048;
    const float TARGET_FPS_TIMESTEP = 1 / 60f;

    private readonly Canvas2DContextState _canvasContextState;
    private readonly FPSState _fpsState;

    readonly DrawOperation[] draws;
    int drawIndex;
    int addDrawIndex;

    public CanvasContextDrawer(Canvas2DContextState canvasContextState, FPSState fpsState)
    {
        _canvasContextState = canvasContextState;

        draws = new DrawOperation[MAX_DRAW_CALLS];
        for (int i = 0; i < MAX_DRAW_CALLS; i++)
        {
            draws[i] = new DrawOperation();
        }

        drawIndex = 0;
        addDrawIndex = 0;
        _fpsState = fpsState;
    }

    private Canvas2DContext _canvasContext => _canvasContextState.CanvasContext;

    public void Begin()
    {
        drawIndex = addDrawIndex;
    }

    public void DrawElement(ElementReference elementReference, double sx, double sy, double sWidth, double sHeight, double dx, double dy, double dWidth, double dHeight)
    {
        draws[addDrawIndex].draw = true;
        draws[addDrawIndex].elementReference = elementReference;
        draws[addDrawIndex].sx = sx;
        draws[addDrawIndex].sy = sy;
        draws[addDrawIndex].sWidth = sWidth;
        draws[addDrawIndex].sHeight = sHeight;
        draws[addDrawIndex].dx = dx;
        draws[addDrawIndex].dy = dy;
        draws[addDrawIndex].dWidth = dWidth;
        draws[addDrawIndex].dHeight = dHeight;

        addDrawIndex = (addDrawIndex + 1) % MAX_DRAW_CALLS;
    }

    public void End()
    {
        draws[addDrawIndex].draw = false;
        addDrawIndex = (addDrawIndex + 1) % MAX_DRAW_CALLS;
    }

    public async ValueTask Render(int width, int height)
    {
        await _canvasContext.BeginBatchAsync();
        await _canvasContext.ClearRectAsync(0, 0, width, height);
        await _canvasContext.SetFillStyleAsync("cornflowerblue");
        await _canvasContext.FillRectAsync(0, 0, width, height);
        while (draws[drawIndex].draw)
        {
            var drawImageCall = draws[drawIndex];
            await _canvasContext.DrawImageAsync(
                elementReference: drawImageCall.elementReference,
                sx: drawImageCall.sx,
                sy: drawImageCall.sy,
                sWidth: drawImageCall.sWidth,
                sHeight: drawImageCall.sHeight,
                dx: drawImageCall.dx,
                dy: drawImageCall.dy,
                dWidth: drawImageCall.dWidth,
                dHeight: drawImageCall.dHeight
            );
            drawIndex = (drawIndex + 1) % MAX_DRAW_CALLS;
        }
        await _canvasContext.SetFillStyleAsync("white");
        await _canvasContext.SetFontAsync("11px Verdana");
        await _canvasContext.FillTextAsync($"FPS: {_fpsState.FPS}", 1, 10);
        await _canvasContext.EndBatchAsync();
    }
}
