using DefaultEcs;
using System.Diagnostics;

namespace Polymono.Entities
{
    class MenuClose : UIButton
    {
        public MenuClose(World world, Entity camera,
            string texture = @"Resources\MinaReally.png",
            int x = 0, int y = 0, int width = 50, int height = 20)
            : base(world, camera, texture, x, y, width, height)
        {
            OnClick = () =>
            {
                Debug.WriteLine("MenuClose clicked.");
            };
        }
    }
}
