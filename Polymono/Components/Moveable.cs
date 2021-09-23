using OpenTK.Mathematics;

namespace Polymono.Components
{
    struct Moveable
    {
        public Vector3 Velocity;

        public Moveable(Vector3 velocity = default) => Velocity = velocity;
    }
}
