using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace Polymono.Systems
{
    class InputSystem : ISystem<PolyFrameEventArgs>
    {
        public bool IsEnabled { get; set; }

        public InputSystem(World world, IParallelRunner runner, 
            ref KeyEvent keyPressed)
        {
            keyPressed += (KeyboardKeyEventArgs e) =>
            {
                Debug.WriteLine($"{Util.ThreadID}: InputSystem Triggered => {e.Key}");
            };
        }

        public void Update(PolyFrameEventArgs state)
        {
            ref KeyboardState keyboard = ref state.KeyboardState;
            if (keyboard == null)
                return;
        }

        public void Dispose()
        {
            
        }
    }
}
