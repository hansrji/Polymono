using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using OpenTK.Graphics.OpenGL4;
using Polymono.Components;

namespace Polymono.Systems
{
    class WindowResizeSystem : AComponentSystem<PolyFrameEventArgs, Viewable>
    {
        public WindowResizeSystem(World world, IParallelRunner runner) 
            : base(world, runner)
        {
            
        }

        protected override void PreUpdate(PolyFrameEventArgs state)
        {
            GL.Viewport(0, 0, state.Size.X, state.Size.Y);
        }

        protected override void Update(PolyFrameEventArgs state, ref Viewable viewable)
        {
            viewable.Size = state.Size;
        }
    }
}
