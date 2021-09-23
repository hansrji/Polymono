using DefaultEcs;
using DefaultEcs.Resource;
using Polymono.Components;
using Polymono.Components.Resources;
using System.Diagnostics;

namespace Polymono.Managers
{
    class ShaderManager<T> : AResourceManager<ShaderInfo, T> 
        where T : IShader, new()
    {
        protected override T Load(ShaderInfo info)
        {
            Debug.WriteLine($"ShaderManager: Loading shader: [{info.VertexPath}], [{info.FragmentPath}]");
            T shader = new()
            {
                VertexPath = info.VertexPath,
                FragmentPath = info.FragmentPath
            };
            shader.Load();
            Debug.WriteLine($"Finalised shader: [{info.VertexPath}], [{info.FragmentPath}]");
            return shader;
        }

        protected override void OnResourceLoaded(in Entity entity, ShaderInfo info, T resource)
        {
            Debug.WriteLine($"ShaderManager: [{entity.GetHashCode()}] Preparing.");
            if (entity.Has<Drawable>())
            {
                ref Drawable drawable = ref entity.Get<Drawable>();
                drawable.Shader = resource;
                drawable.IsShaderReady = true;
                Debug.WriteLine($"ShaderManager: [{entity.GetHashCode()}] Done.");
            }
        }
    }
}
