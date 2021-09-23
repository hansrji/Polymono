using OpenTK.Mathematics;

namespace Polymono.Components.Resources
{
    public interface IFreeTypeFont
    {
        IShader Shader { get; set; }

        void RenderText(string text, float x, float y, float scale, Vector2 dir);
    }
}