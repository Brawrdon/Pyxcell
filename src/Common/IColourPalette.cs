using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.Common
{
    public interface IColourPalette
    {
        List<Rgba32> Colours { get; }
        void AddColour(Rgba32 colour);
        void AddColours(IEnumerable<Rgba32> colours);
        Rgba32 SelectRandomColour();
    }
}