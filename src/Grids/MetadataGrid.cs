using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.Grids
{
    public class MetadataGrid : IGrid
    {
        public Rgba32 Rgba32 { get; }
        public int[] Pattern { get; }

        public MetadataGrid(Rgba32 colour, int[] pattern)
        {
            Rgba32 = colour;
            Pattern = new int[14];
        }
    }
}