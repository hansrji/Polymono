using OpenTK.Mathematics;

namespace Polymono.Components.Resources
{
    public interface IShader
    {
        int Handle { get; set; }
        string VertexPath { get; set; }
        string FragmentPath { get; set; }

        void Load();
        void Use();
        void Swap();
        uint GetAttribLocation(string name);
        void BindAttribLocation(string name, ref uint index);
        void SetInt(string name, int data);
        void SetUInt(string name, uint data);
        void SetFloat(string name, float data);
        void SetVector2(string name, Vector2 data);
        void SetVector3(string name, Vector3 data);
        void SetVector4(string name, Vector4 data);
        void SetVector4(string name, Color4 data);
        void SetMatrix4(string name, Matrix4 data);
    }
}
