using DefaultEcs;
using DefaultEcs.Resource;
using Polymono.Components;
using Polymono.Systems.Resources;

namespace Polymono.Managers
{
    class TextureManager : AResourceManager<string, Texture>
    {
        protected override Texture Load(string info)
        {
            return Texture.LoadFromFile(info);
        }

        protected override void OnResourceLoaded(in Entity entity, string info, Texture resource)
        {
            if (entity.Has<Drawable>())
            {
                ref Drawable drawable = ref entity.Get<Drawable>();
                drawable.Texture = resource;
                drawable.IsTextureReady = true;
            }
        }
    }
}
