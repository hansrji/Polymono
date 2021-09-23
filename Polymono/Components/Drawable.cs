using DefaultEcs;
using OpenTK.Mathematics;
using Polymono.Components.Resources;

namespace Polymono.Components
{
    struct Drawable
    {
        // GL buffer indexes
        public uint VAO;
        public uint VBO;
        public uint IBO;
        // Shader
        public IShader Shader;
        public bool IsShaderReady;
        // Texture
        public ITexture Texture;
        public bool IsTextureReady;
        // Model
        public IModel Model;
        public bool IsModelReady;
        // Camera used
        public Entity Camera;
        // Matrix
        public Matrix4 ModelMatrix;
        // Draw loading state
        public bool HasLoaded;
    }
}
