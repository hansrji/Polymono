using DefaultEcs;
using DefaultEcs.Resource;
using OpenTK.Mathematics;
using Polymono.Components;
using Polymono.Components.Resources;
using Polymono.Managers;
using Polymono.Systems.Resources;
using System;
using System.Diagnostics;

namespace Polymono.Entities
{
    class UIButton : ICreatable<PolyFrameEventArgs>
    {
        public World World { get; }
        public Entity Entity { get; protected set; }
        public Entity Camera { get; }

        public Action OnClick { get; set; } = () => { Debug.WriteLine("Button clicked."); };

        protected readonly string Texture;
        protected readonly int X;
        protected readonly int Y;
        protected readonly int Width;
        protected readonly int Height;

        public UIButton(World world, Entity camera, 
            string texture = @"Resources\MinaReally.png",
            int x = 0, int y = 0, int width = 50, int height = 20)
        {
            World = world;
            Camera = camera;
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void Create(PolyFrameEventArgs state)
        {
            Entity = World.CreateEntity();
            Entity.Set(new Position(new Vector3(X, Y, -0.1f)));
            Entity.Set(new Scale(new Vector3(Width, Height, 0f)));
            Entity.Set(new Drawable()
            {
                ModelMatrix = Matrix4.Identity,
                Camera = Camera,
                HasLoaded = false
            });
            Entity.Set(new Clickable()
            {
                X = X - Width,
                Y = Y - Height,
                Width = Width * 2,
                Height = Height * 2,
                State = ClickState.Clicked,
                Callback = OnClick
            });
            Entity.Set(ManagedResource<Texture>.Create(Texture));
            Entity.Set(ManagedResource<Shader>.Create(
                new ShaderInfo(ShaderPath.PTNTVert, ShaderPath.PTNTFrag)));
            Entity.Set(ManagedResource<Model>.Create(
                new ModelInfo(@"Resources\square.obj")));
            IShader ttfShader = new Shader(this, "Shaders/ttf.vert", "Shaders/ttf.frag");
            FreeTypeFont font = new(this, ref ttfShader, 32);
        }
    }
}
