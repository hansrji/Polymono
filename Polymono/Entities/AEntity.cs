using DefaultEcs;

namespace Polymono.Entities
{
    public abstract class AEntity<T> : ICreatable<T>
    {
        public World World { get; protected set; }
        public Entity Camera { get; protected set; }
        public Entity Entity { get; protected set; }

        protected AEntity(World world, Entity camera)
        {
            World = world;
            Camera = camera;
        }

        public abstract void Create(T state);
    }
}
