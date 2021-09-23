using DefaultEcs;
using DefaultEcs.Resource;
using OpenTK.Mathematics;
using Polymono.Components;
using Polymono.Managers;
using Polymono.Systems.Resources;

namespace Polymono.Entities
{
    class TestSphere : AEntity<PolyFrameEventArgs>
    {
        protected readonly Vector3 Position;
        protected readonly Vector3 Rotation;
        protected readonly Vector3 Scale;
        protected readonly Vector3 VelocityPosition;
        protected readonly Vector3 VelocityRotation;
        protected readonly Vector3 VelocityScale;

        public TestSphere(World world, Entity camera, 
            Vector3 position, Vector3 rotation, Vector3 scale,
            Vector3 velocityPosition, Vector3 velocityRotation, Vector3 velocityScale)
            : base(world, camera)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            VelocityPosition = velocityPosition;
            VelocityRotation = velocityRotation;
            VelocityScale = velocityScale;
        }

        public override void Create(PolyFrameEventArgs state)
        {
            Entity = World.CreateEntity();
            Entity.Set(new Position(Position));
            Entity.Set(new Rotation(Rotation));
            Entity.Set(new Scale(Scale));
            Entity.Set(new Velocity(VelocityPosition, VelocityRotation, VelocityScale));
            Entity.Set(new Drawable()
            {
                ModelMatrix = Matrix4.Identity,
                Camera = Camera,
                HasLoaded = false
            });
            Entity.Set(ManagedResource<Shader>.Create(
                new ShaderInfo(ShaderPath.PVert, ShaderPath.PFrag)));
            Entity.Set(ManagedResource<Model>.Create(
                new ModelInfo(@"Resources\sphere.obj")));
        }
    }
}
