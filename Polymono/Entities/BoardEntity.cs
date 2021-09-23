using DefaultEcs;
using Polymono.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymono.Entities
{
    class BoardEntity : AEntity<PolyFrameEventArgs>
    {
        public BoardEntity(World world, Entity camera)
            : base(world, camera)
        {

        }

        public override void Create(PolyFrameEventArgs state)
        {
            Entity = World.CreateEntity();
            Board board = new();
            board.Properties = new Property[40];
            Entity.Set(board);

            const uint number_of_properties = 36;
            for (uint i = 0; i < number_of_properties; i++)
            {
                Property property = new(i, PropertyName(i));
                board.Properties[i] = property;
            }
        }

        public static string PropertyName(uint index) => index switch
        {
            0 => "Go",
            1 => "Old Kent Road",
            2 => "Community Chest",
            3 => "Whitechaple Road",
            4 => "Income Tax",
            5 => "Kings Cross Station",
            6 => "The Angel Islington",
            7 => "Chance",
            8 => "Euston Road",
            9 => "Pentonville Road",
            10 => "Jail",
            11 => "Pall Mall",
            12 => "Electric Company",
            13 => "Whitehall",
            14 => "Northumberl'd Avenue",
            15 => "Marylebone Station",
            16 => "Bow Street",
            17 => "Community Chest",
            18 => "Marborough Street",
            19 => "Vine Street",
            20 => "Free Parking",
            21 => "Strand",
            22 => "Chance",
            23 => "Fleet Street",
            24 => "Trafalgar Square",
            25 => "Fenchurch St. Station",
            26 => "Leicester Square",
            27 => "Coventry Street",
            28 => "Water Works",
            29 => "Piccadilly",
            30 => "Go To Jail",
            31 => "Regent Street",
            32 => "Oxford Street",
            33 => "Community Chest",
            34 => "Bond Street",
            35 => "Liverpool St. Station",
            36 => "Chance",
            37 => "Park Lane",
            38 => "Luxury Tax",
            39 => "Mayfair",
            _ => "Error",
        };
    }

    public enum PropertyGroup
    {
        Brown,
        Cyan,
        Pink,
        Orange,
        Red,
        Yellow,
        Green,
        Blue,
        Railroad,
        Utility,
        Community,
        Chance,
        Go,
        Jail,
        Parking,
        Police,
        Tax
    }
}
