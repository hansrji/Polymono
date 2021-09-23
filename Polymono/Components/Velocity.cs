using OpenTK.Mathematics;

namespace Polymono.Components
{
    struct Velocity
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;

        public Velocity(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
