using Polymono.Components.Resources;
using System;

namespace Polymono.Components
{
    struct Clickable
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public ClickState State;
        public Action Callback;
    }
}
