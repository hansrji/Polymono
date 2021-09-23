namespace Polymono.Managers
{
    readonly struct ShaderInfo
    {
        public readonly string VertexPath;
        public readonly string FragmentPath;

        public ShaderInfo(string vertexPath, string fragmentPath)
        {
            VertexPath = vertexPath;
            FragmentPath = fragmentPath;
        }

        public override bool Equals(object obj)
        {
            if (obj is ShaderInfo info)
                return VertexPath == info.VertexPath
                    && FragmentPath == info.FragmentPath;
            return false;
        }

        public override int GetHashCode()
        {
            return VertexPath.GetHashCode()
                ^ FragmentPath.GetHashCode();
        }
    }
}