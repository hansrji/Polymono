using OpenTK.Mathematics;
using System;

namespace Polymono.Components
{
    struct Viewable
    {
        public bool IsMoveable;
        public bool HasDepth;
        public Vector2 Size;
        // Those vectors are directions pointing outwards from the camera to define how it rotated
        private Vector3 front;
        public Vector3 Front => front;

        private Vector3 up;
        public Vector3 Up => up;

        private Vector3 right;
        public Vector3 Right => right;

        // Rotation around the X axis (radians)
        private float pitch;
        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(pitch);
            set
            {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                float angle = MathHelper.Clamp(value, -89f, 89f);
                pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        // Rotation around the Y axis (radians)
        private float yaw; // Without this you would be started rotated 90 degrees right
        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(yaw);
            set
            {
                yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        // The field of view of the camera (radians)
        private float fov;
        // The field of view (FOV) is the vertical angle of the camera view, this has been discussed more in depth in a
        // previous tutorial, but in this tutorial you have also learned how we can use this to simulate a zoom feature.
        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Fov
        {
            get => MathHelper.RadiansToDegrees(fov);
            set
            {
                float angle = MathHelper.Clamp(value, 1f, 45f);
                fov = MathHelper.DegreesToRadians(angle);
            }
        }

        // This is simply the aspect ratio of the viewport, used for the projection matrix
        //public float AspectRatio { get; set; }

        public bool FirstMove;

        public Vector2 LastPos;

        public Viewable(Vector2 size, bool moveable = true, bool hasDepth = true)
        {
            front = -Vector3.UnitZ;
            up = Vector3.UnitY;
            right = Vector3.UnitX;
            pitch = 0;
            yaw = -MathHelper.PiOver2;
            fov = MathHelper.PiOver2;
            FirstMove = true;
            LastPos = default;
            Size = size;
            IsMoveable = moveable;
            HasDepth = hasDepth;
            ViewMatrix = (viewable, position) => Matrix4.LookAt(position, position + viewable.Front, viewable.Up);
            ProjectionMatrix = (viewable) => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(viewable.Fov), viewable.Size.X / viewable.Size.Y, 1.01f, 100f);
        }

        // This function is going to update the direction vertices using some of the math learned in the web tutorials
        private void UpdateVectors()
        {
            // First the front matrix is calculated using some basic trigonometry
            front.X = MathF.Cos(pitch) * MathF.Cos(yaw);
            front.Y = MathF.Sin(pitch);
            front.Z = MathF.Cos(pitch) * MathF.Sin(yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results
            front = Vector3.Normalize(front);

            // Calculate both the right and the up vector using cross product
            // Note that we are calculating the right from the global up, this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public Func<Viewable, Vector3, Matrix4> ViewMatrix { get; set; }

        public Func<Viewable, Matrix4> ProjectionMatrix { get; set; }
    }
}
