using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Polymono.Components;
using Polymono.Components.Resources;
using System.Collections.Generic;

namespace Polymono.Systems
{
    class UIInteractionSystem : AComponentSystem<PolyFrameEventArgs, Clickable>
    {
        public Queue<Vector2> ClickPositions { get; } = new();

        public UIInteractionSystem(World world, IParallelRunner runner, ref ClickEvent clickPress)
            : base(world, runner)
        {
            clickPress += (MouseButtonEventArgs e, MouseState state) =>
            {
                ClickPositions.Enqueue(state.Position);
            };
        }

        protected override void Update(PolyFrameEventArgs state, ref Clickable clickable)
        {
            foreach (Vector2 vector in ClickPositions)
            {
                Vector2 topLeft = new(clickable.X, clickable.Y);
                Vector2 topRight = new(clickable.X + clickable.Width, clickable.Y);
                Vector2 bottomRight = new(clickable.X + clickable.Width, clickable.Y + clickable.Height);
                Vector2 bottomLeft = new(clickable.X, clickable.Y + clickable.Height);
                if (IsRight(topLeft, topRight, vector)
                    && IsRight(topRight, bottomRight, vector)
                    && IsRight(bottomRight, bottomLeft, vector)
                    && IsRight(bottomLeft, topLeft, vector))
                {
                    clickable.State = ClickState.Clicked;
                    clickable.Callback();
                }
                else
                {
                    clickable.State = ClickState.Normal;
                }
            }
        }

        protected override void PostUpdate(PolyFrameEventArgs state)
        {
            ClickPositions.Clear();
        }

        public static bool IsOn(Vector2 a, Vector2 b, Vector2 c)
        {
            return CrossProduct(a, b, c) == 0;
        }

        public static bool IsRight(Vector2 a, Vector2 b, Vector2 c)
        {
            return CrossProduct(a, b, c) > 0;
        }

        public static bool IsLeft(Vector2 a, Vector2 b, Vector2 c)
        {
            return CrossProduct(a, b, c) < 0;
        }

        public static float CrossProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }
    }
}
