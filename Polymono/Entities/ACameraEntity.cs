using DefaultEcs;

namespace Polymono.Entities
{
    public abstract class ACameraEntity<T> : ICreatable<T>
    {
        public World World { get; protected set; }
        public Entity Entity { get; protected set; }

        protected ACameraEntity(World world)
        {
            World = world;
        }

        public abstract void Create(T state);
    }
}
