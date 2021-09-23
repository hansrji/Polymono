namespace Polymono.Managers
{
    enum TextureType
    {
        Texture
    }

    readonly struct TextureInfo
    {
        public readonly TextureType Type;
        public readonly string Path;

        public TextureInfo(TextureType type, string path)
        {
            Type = type;
            Path = path;
        }

        public override bool Equals(object obj)
        {
            if (obj is TextureInfo info)
                return Path == info.Path;
            return false;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}