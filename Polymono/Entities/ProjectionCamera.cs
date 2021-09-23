using DefaultEcs;
using OpenTK.Mathematics;
using Polymono.Components;

namespace Polymono.Entities
{
    class ProjectionCamera : ACameraEntity<PolyFrameEventArgs>
    {
        public ProjectionCamera(World world)
            : base(world)
        {

        }

        public override void Create(PolyFrameEventArgs state)
        {
            Entity = World.CreateEntity();
            Entity.Set(new Position(Vector3.UnitZ * 3));
            Entity.Set(new Viewable(state.Size));
        }
    }
}
