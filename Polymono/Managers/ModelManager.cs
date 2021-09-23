using DefaultEcs;
using DefaultEcs.Resource;
using Polymono.Components;
using Polymono.Components.Resources;
using System.Diagnostics;

namespace Polymono.Managers
{
    class ModelManager<T> : AResourceManager<ModelInfo, T> 
        where T : IModel, new()
    {
        protected override T Load(ModelInfo info)
        {
            Debug.WriteLine($"{Util.ThreadID}: Load. [{info.Path}]");
            T model = new();
            model.Load(info.Path);
            return model;
        }

        protected override void OnResourceLoaded(in Entity entity, ModelInfo info, T resource)
        {
            Debug.WriteLine($"{Util.ThreadID}: OnResourceLoaded. [{info.Path}]");
            if (entity.Has<Drawable>())
            {
                ref Drawable drawable = ref entity.Get<Drawable>();
                resource.LoadBuffers(ref drawable);
                drawable.Model = resource;
                drawable.IsModelReady = true;
            }
        }
    }
}
