using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Polymono.Components;

namespace Polymono.Systems
{
    class DrawSystem : AComponentSystem<double, Drawable>
    {
        public DrawSystem(World world)
            : base(world)
        {

        }

        protected override void Update(double state, ref Drawable drawable)
        {
            if (!drawable.IsShaderReady)
                return;
            if (!drawable.IsModelReady)
                return;
            ref Entity camera = ref drawable.Camera;
            if (!camera.Has<Viewable>())
                return;
            ref Viewable viewable = ref camera.Get<Viewable>();
            if (!camera.Has<Position>())
                return;
            ref Position position = ref camera.Get<Position>();
            Matrix4 viewMatrx = viewable.ViewMatrix(viewable, position.Value);
            Matrix4 projectionMatrix = viewable.ProjectionMatrix(viewable);
            if (viewable.HasDepth)
                GL.Enable(EnableCap.DepthTest);
            else
                GL.Disable(EnableCap.DepthTest);
            drawable.Shader.Swap();
            drawable.Shader.SetMatrix4("model", drawable.ModelMatrix);
            drawable.Shader.SetMatrix4("view", viewMatrx);
            drawable.Shader.SetMatrix4("projection", projectionMatrix);
            drawable.Shader.SetVector4("colour", Color4.Aqua);
            drawable.Shader.SetFloat("time", (float)state);
            if (drawable.IsTextureReady)
                drawable.Texture.Use(TextureUnit.Texture0);
            GL.BindVertexArray(drawable.VAO);
            GL.DrawElements(PrimitiveType.Triangles, drawable.Model.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
