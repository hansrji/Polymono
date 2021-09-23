using OpenTK.Mathematics;
using Polymono.Components;
using Polymono.Components.Resources;
using Polymono.Systems.Resources;

namespace Polymono
{
    interface IVolume
    {
        uint ID { get; }

        ICamera Camera { get; set; }
        IShader Shader { get; set; }

        Matrix4 Model { get; set; }

        uint VAO { get; }
        uint VBO { get; }
        uint IBO { get; }

        Vertex<Vector3, Vector2, Vector3>[] Vertices { get; set; }
        uint[] Indices { get; set; }
        Texture Texture { get; set; }

        void CreateBuffers();
        void LoadData();
        void LoadDataFromFile(string filename);
        void LoadBuffers();
        void UseTexture();
        void UseShader();
        void SetMatrices();
    }
}
