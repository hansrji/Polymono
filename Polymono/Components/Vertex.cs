namespace Polymono.Components
{
    public struct Vertex<T1>
        where T1 : struct
    {
        public T1 Data1;

        public Vertex(T1 t1)
        {
            Data1 = t1;
        }
    }

    public struct Vertex<T1, T2>
        where T1 : struct
        where T2 : struct
    {
        public T1 Data1;
        public T2 Data2;

        public Vertex(T1 position, T2 texture)
        {
            Data1 = position;
            Data2 = texture;
        }
    }

    public struct Vertex<T1, T2, T3>
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        public T1 Data1;
        public T2 Data2;
        public T3 Data3;

        public Vertex(T1 t1, T2 t2, T3 t3)
        {
            Data1 = t1;
            Data2 = t2;
            Data3 = t3;
        }
    }
}
