using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Polymono
{
    public struct PolyFrameEventArgs
    {
        public double Time;
        public Vector2i Size;
        public KeyboardState KeyboardState;
        public MouseState MouseState;
        public bool IsFocused;
        public bool IsGrabbed;

        public PolyFrameEventArgs(double time, Vector2i size, KeyboardState keyboardState, MouseState mouseState,
            bool isFocused, bool isGrabbed)
        {
            Time = time;
            Size = size;
            KeyboardState = keyboardState;
            MouseState = mouseState;
            IsFocused = isFocused;
            IsGrabbed = isGrabbed;
        }
    }
}
