using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.Grids
{
    public interface IGrid
    {
        Rgba32 Rgba32 { get; }
        int[] Pattern { get; }
    }
}