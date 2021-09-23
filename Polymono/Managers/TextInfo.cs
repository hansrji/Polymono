namespace Polymono.Managers
{
    enum TextType
    {
        Arial
    }

    readonly struct TextInfo
    {
        public readonly TextureType Type;
        public readonly string Path;

        public TextInfo(TextureType type, string path)
        {
            Type = type;
            Path = path;
        }

        public override bool Equals(object obj)
        {
            if (obj is TextInfo info)
                return Path == info.Path;
            return false;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}
