using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using OpenTK.Mathematics;
using Polymono.Components;

namespace Polymono.Systems
{
    [With(typeof(Drawable))]
    [WithEither(typeof(Position), typeof(Rotation), typeof(Scale))]
    class PreDrawSystem : AEntitySetSystem<PolyFrameEventArgs>
    {
        public PreDrawSystem(World world, IParallelRunner runner) : base(world, runner)
        {

        }

        protected override void Update(PolyFrameEventArgs state, in Entity entity)
        {
            ref Drawable drawable = ref entity.Get<Drawable>();
            Matrix4 model = Matrix4.Identity;
            // Got references to model matrix.
            if (entity.Has<Scale>())
                model *= Matrix4.CreateScale(entity.Get<Scale>().Value);
            if (entity.Has<Rotation>())
            {
                Vector3 rotation = entity.Get<Rotation>().Value;
                model *= Matrix4.CreateRotationX(rotation.X);
                model *= Matrix4.CreateRotationY(rotation.Y);
                model *= Matrix4.CreateRotationZ(rotation.Z);
            }
            if (entity.Has<Position>())
                model *= Matrix4.CreateTranslation(entity.Get<Position>().Value);
            drawable.ModelMatrix = model;
        }
    }
}
