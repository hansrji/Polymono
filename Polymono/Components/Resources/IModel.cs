namespace Polymono.Components.Resources
{
    interface IModel
    {
        float[] Vertices { get; }
        uint[] Indices { get; }

        void Load(string path);
        void LoadBuffers(ref Drawable drawable);
    }
}