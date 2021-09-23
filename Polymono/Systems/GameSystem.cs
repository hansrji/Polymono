using DefaultEcs;
using DefaultEcs.System;
using OpenTK.Mathematics;
using Polymono.Entities;
using System;

namespace Polymono.Systems
{
    class GameSystem : ISystem<PolyFrameEventArgs>
    {
        public Random Random { get; }
        public World World { get; }
        public Action Exit { get; }
        public bool FirstRun { get; private set; } = true;
        public bool IsEnabled { get; set; } = true;

        public GameSystem(World world, Action exit)
        {
            World = world;
            Exit = exit;
            Random = new Random();
            Random.Next();
            Random.Next();
        }

        public void Update(PolyFrameEventArgs state)
        {
            if (FirstRun)
            {
                ProjectionCamera projectionCamera = new(World);
                projectionCamera.Create(state);

                InterfaceCamera interfaceCamera = new(World);
                interfaceCamera.Create(state);

                TestCube testCube1 = new(World, projectionCamera.Entity, 
                    new Vector3(1, 5, 4), Vector3.One / 4, Vector3.One / 4, 
                    Vector3.Zero, Vector3.UnitX / 16, Vector3.Zero);
                testCube1.Create(state);

                TestSphere testSphere = new(World, projectionCamera.Entity,
                    new Vector3(4, 3, 7), Vector3.Zero, Vector3.One / 16,
                    Vector3.Zero, Vector3.Zero, Vector3.Zero);
                testSphere.Create(state);

                // Create UI
                MenuStart menuStart = new(World, interfaceCamera.Entity, x: 150, y: 50);
                menuStart.Create(state);
                MenuClose menuClose = new(World, interfaceCamera.Entity, x: 150, y: 150);
                menuClose.OnClick = Exit;
                menuClose.Create(state);

                FirstRun = false;
            }
        }

        public void Dispose()
        {
            IsEnabled = false;
        }
    }
}