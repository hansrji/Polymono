using DefaultEcs;
using DefaultEcs.System;
using Polymono.Components;
using Polymono.Components.Resources;
using System.Collections.Generic;

namespace Polymono.Systems
{
    class DrawTextSystem : AComponentSystem<double, DrawableText>
    {
        // Character dictionary
        public Dictionary<uint, Character> Characters;

        public DrawTextSystem(World world) 
            : base(world)
        {

        }

        protected override void Update(double state, ref DrawableText component)
        {

        }
    }
}
