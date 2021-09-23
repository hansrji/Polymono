using DefaultEcs;
using DefaultEcs.Resource;
using OpenTK.Mathematics;
using Polymono.Components;
using Polymono.Components.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Polymono.Managers
{
    public struct DrawableText
    {
        // GL buffer indexes
        public uint VAO;
        public uint VBO;
        // Shader
        public IShader Shader;
        public bool IsShaderReady;
        // Model
        public IText Text;
        public bool IsTextReady;
        // Camera used
        public Entity Camera;
        // Matrix
        public Matrix4 ModelMatrix;
        // Draw loading state
        public bool HasLoaded;
    }

    public interface IText
    {
        Text Load(ref DrawableText drawable);
    }

    public class Text : IText
    {
        public Text Load(ref DrawableText drawable)
        {

            return new Text();
        }
    }

    class TextManager<T> : AResourceManager<TextInfo, T>
        where T : IText, new()
    {
        protected override T Load(TextInfo info)
        {
            Debug.WriteLine($"{Util.ThreadID}: Load. [{info.Path}]");
            T text = new();
            Type type = typeof(Text);
            MethodInfo methodInfo = type.GetMethod("Load");
            Text t = text as Text;
            methodInfo.Invoke(t, default);
            return text;
        }

        protected override void OnResourceLoaded(in Entity entity, TextInfo info, T resource)
        {
            Debug.WriteLine($"{Util.ThreadID}: OnResourceLoaded. [{info.Path}]");
            if (entity.Has<DrawableText>())
            {
                ref DrawableText drawable = ref entity.Get<DrawableText>();
                resource.Load(ref drawable);
                drawable.Text = resource;
                drawable.IsTextReady = true;
            }
        }
    }
}
