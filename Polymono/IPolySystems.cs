using DefaultEcs.System;

namespace Polymono
{
    interface IPolySystems
    {
        SequentialSystem<PolyFrameEventArgs> UpdateSystems { get; set; }
        SequentialSystem<double> DrawSystems { get; set; }
    }
}
