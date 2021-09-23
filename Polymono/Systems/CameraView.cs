using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Polymono.Components;

namespace Polymono.Systems
{
    [With(typeof(Viewable))]
    [With(typeof(Position))]
    class CameraView : AEntitySetSystem<PolyFrameEventArgs>
    {
        public CameraView(World world, IParallelRunner runner) 
            : base(world, runner)
        {

        }

        protected override void Update(PolyFrameEventArgs state, in Entity entity)
        {
            ref Viewable viewable = ref entity.Get<Viewable>();
            ref Position position = ref entity.Get<Position>();
            if (!viewable.IsMoveable)
                return;
            float time = (float)state.Time;
            float cameraSpeed = 1.5f;
            float sensitivity = 0.2f;
            // Key state
            KeyboardState keyboard = state.KeyboardState;
            if (keyboard != null && keyboard.IsAnyKeyDown)
            {
                if (keyboard.IsKeyDown(Keys.LeftShift))
                    cameraSpeed *= 3;
                if (keyboard.IsKeyDown(Keys.W))
                    position.Value += viewable.Front * cameraSpeed * time;
                if (keyboard.IsKeyDown(Keys.A))
                    position.Value -= viewable.Right * cameraSpeed * time;
                if (keyboard.IsKeyDown(Keys.S))
                    position.Value -= viewable.Front * cameraSpeed * time;
                if (keyboard.IsKeyDown(Keys.D))
                    position.Value += viewable.Right * cameraSpeed * time;
                if (keyboard.IsKeyDown(Keys.Space))
                    position.Value += viewable.Up * cameraSpeed * time;
                if (keyboard.IsKeyDown(Keys.LeftControl))
                    position.Value -= viewable.Up * cameraSpeed * time;
            }
            if (state.IsFocused && state.IsGrabbed)
            {
                // Mouse state
                MouseState mouse = state.MouseState;
                if (viewable.FirstMove) // this bool variable is initially set to true
                {
                    viewable.LastPos = new Vector2(mouse.X, mouse.Y);
                    viewable.FirstMove = false;
                }
                else
                {
                    // Calculate the offset of the mouse position
                    float deltaX = mouse.X - viewable.LastPos.X;
                    float deltaY = mouse.Y - viewable.LastPos.Y;
                    viewable.LastPos = new Vector2(mouse.X, mouse.Y);

                    // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                    viewable.Yaw += deltaX * sensitivity;
                    viewable.Pitch -= deltaY * sensitivity; // reversed since y-coordinates range from bottom to top
                }
            }
        }

        public static (Vector3, Vector3) CastF(Vector2 mouse, Matrix4 model, Matrix4 proj, Vector2 viewport)
        {
            Vector3 start = UnProject(new Vector3(mouse.X, mouse.Y, 0.0f), model, proj, viewport);
            Vector3 end = UnProject(new Vector3(mouse.X, mouse.Y, 1.0f), model, proj, viewport);
            return (start, end);
        }

        public static Vector3 UnProject(Vector3 mouse, Matrix4 model, Matrix4 proj, Vector2 viewport)
        {
            Vector4 vector;
            vector.X = 2.0f * mouse.X / viewport.X - 1;
            vector.Y = -(2.0f * mouse.Y / viewport.Y - 1);
            vector.Z = mouse.Z;
            vector.W = 1.0f;
            Matrix4 modelInv = Matrix4.Invert(model);
            Matrix4 projInv = Matrix4.Invert(proj);
            Quaternion modelQuat = modelInv.ExtractRotation();
            Quaternion projQuat = projInv.ExtractRotation();
            vector = Vector4.Transform(vector, projQuat);
            vector = Vector4.Transform(vector, modelQuat);
            if (vector.W > 0.000001f || vector.W < 0.000001f)
            {
                vector.X /= vector.W;
                vector.Y /= vector.W;
                vector.Z /= vector.W;
            }
            return vector.Xyz;
        }
    }
}
