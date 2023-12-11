namespace NodeBeautify
{
    internal static class Configs
    {
        /// <summary>
        /// Round to the cloest multiples of this value
        /// </summary>
        public const int GRID_SIZE = 10;

        /// <summary>
        /// NodeType, (Width, Height)
        /// </summary>
        public static readonly Dictionary<string, (float, float)> FIXED_SIZE = new(){
            { "CLIPTextEncode", (400.0f, 160.0f) },
            { "PreviewImage", (420.0f, 420.0f) }
        };
    }
}
