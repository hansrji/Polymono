using DefaultEcs;
using OpenTK.Mathematics;
using Polymono.Components;

namespace Polymono.Entities
{
    class InterfaceCamera : ACameraEntity<PolyFrameEventArgs>
    {
        public InterfaceCamera(World world)
            : base(world)
        {
            World = world;
        }

        public override void Create(PolyFrameEventArgs state)
        {
            Entity = World.CreateEntity();
            Entity.Set(new Position(Vector3.Zero));
            Entity.Set(new Viewable(state.Size, false, false)
            {
                ViewMatrix = (uiViewable, uiPosition) => Matrix4.LookAt(Vector3.Zero,
                    Vector3.Zero + uiViewable.Front, uiViewable.Up),
                ProjectionMatrix = (uiViewable) => Matrix4.CreateOrthographicOffCenter(0, uiViewable.Size.X,
                    uiViewable.Size.Y, 0, 0.01f, 1.01f)
            });
        }
    }
}
