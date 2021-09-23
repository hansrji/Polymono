using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using OpenTK.Mathematics;
using Polymono.Components;

namespace Polymono.Systems
{
    [With(typeof(Velocity))]
    [WithEither(typeof(Position), typeof(Rotation), typeof(Scale))]
    class SimpleMovement : AEntitySetSystem<PolyFrameEventArgs>
    {
        public SimpleMovement(World world, IParallelRunner runner) : base(world, runner)
        {
            
        }

        protected override void Update(PolyFrameEventArgs state, in Entity entity)
        {
            //Debug.WriteLine($"SimpleMovement: Entity[{entity.GetHashCode()}]");
            ref Velocity velocity = ref entity.Get<Velocity>();
            float time = (float)state.Time;
            if (velocity.Scale != Vector3.Zero)
            {
                if (entity.Has<Scale>())
                {
                    ref Scale scale = ref entity.Get<Scale>();
                    scale.Value += velocity.Scale * time;
                    //Debug.WriteLine($"SimpleMovement: Updating scale [{scale.Scale}] by [{velocity.Scale}]");
                }
            }
            if (velocity.Rotation != Vector3.Zero)
            {
                if (entity.Has<Rotation>())
                {
                    ref Rotation rotation = ref entity.Get<Rotation>();
                    rotation.Value += velocity.Rotation * time;
                    //Debug.WriteLine($"SimpleMovement: Updating rotation [{rotation.Rotation}] by [{velocity.Rotation}]");
                }
            }
            if (velocity.Position != Vector3.Zero)
            {
                if (entity.Has<Position>())
                {
                    ref Position position = ref entity.Get<Position>();
                    position.Value += velocity.Position * time;
                    //Debug.WriteLine($"SimpleMovement: Updating rotation [{position.Position}] by [{velocity.Position}]");
                }
            }
        }
    }
}
