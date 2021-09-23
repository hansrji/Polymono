namespace Polymono.Entities
{
    interface ICreatable<T>
    {
        void Create(T state);
    }
}
