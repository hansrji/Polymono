namespace Polymono.Managers
{
    readonly struct ModelInfo
    {
        public readonly string Path;

        public ModelInfo(string path)
        {
            Path = path;
        }

        public override bool Equals(object obj)
        {
            if (obj is ModelInfo info)
                return Path == info.Path;
            return false;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}