using OpenTK.Graphics.OpenGL4;

namespace Polymono.Components.Resources
{
    public interface ITexture
    {
        void Use(TextureUnit unit);
    }
}