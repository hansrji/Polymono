using OpenTK.Windowing.Common;
using System;
using System.Threading.Tasks;

namespace Polymono
{
    public interface IWindow
    {
        event Action Construct;
        event Action Destruct;
        event Func<PolyFrameEventArgs, Task> PreUpdate;
        event Func<PolyFrameEventArgs, Task> Update;
        event Func<PolyFrameEventArgs, Task> PostUpdate;
        event Action<PolyFrameEventArgs> PreRender;
        event Action<PolyFrameEventArgs> Render;
        event Action<PolyFrameEventArgs> PostRender;
        event Action<ResizeEventArgs> PolyResize;
        event Action<MouseWheelEventArgs> PolyMouseWheel;
    }
}
