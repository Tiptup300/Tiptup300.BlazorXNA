using Microsoft.AspNetCore.Components;

namespace ClassLibrary1;

public interface IGameLooper
{
    ValueTask GameLoop(float timeStamp, int width, int height);
}
public class GameLooper : IGameLooper
{
    private readonly ICanvasContextDrawer _canvasContextDrawer;
    private readonly FPSState _fpsState;
    private readonly ElementReferenceState _elementReferenceState;

    float _lastTimestamp;

    public GameLooper(ICanvasContextDrawer canvasContextDrawer, FPSState fpsState, ElementReferenceState elementReferenceState)
    {
        _canvasContextDrawer = canvasContextDrawer;
        _fpsState = fpsState;
        _elementReferenceState = elementReferenceState;
    }
    private ElementReference demoSprite
        => _elementReferenceState.State;

    public async ValueTask GameLoop(float timeStamp, int width, int height)
    {
        await update(timeStamp);
        await render(width, height);

        _fpsState.FPS = (int)Math.Round(1000 / (timeStamp - _lastTimestamp), 0);
        _lastTimestamp = timeStamp;
    }

    private async ValueTask update(float timeStep)
    {
        _canvasContextDrawer.Begin();
        _canvasContextDrawer.DrawElement(
            elementReference: demoSprite,
            sx: 32,
            sy: 32,
            sWidth: 32,
            sHeight: 32,
            dx: 125 + Math.Sin(_lastTimestamp) * 20,
            dy: 125 + Math.Cos(_lastTimestamp) * 20,
            dWidth: 32,
            dHeight: 32
        );
        _canvasContextDrawer.DrawElement(
            elementReference: demoSprite,
            sx: 64,
            sy: 0,
            sWidth: 32,
            sHeight: 32,
            dx: 23 + Math.Sin(_lastTimestamp) * 20,
            dy: 23 + Math.Cos(_lastTimestamp) * 20,
            dWidth: 32,
            dHeight: 32
        );
        _canvasContextDrawer.DrawElement(
            elementReference: demoSprite,
            sx: 32,
            sy: 0,
            sWidth: 32,
            sHeight: 32,
            dx: 75 + Math.Sin(_lastTimestamp) * 20,
            dy: 75 + Math.Cos(_lastTimestamp) * 20,
            dWidth: 32,
            dHeight: 32
        );
        _canvasContextDrawer.End();
    }

    private async ValueTask render(int width, int height)
    {
        await _canvasContextDrawer.Render(width, height);
    }

}