using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    public abstract class GridColour
    {
        public Rgba32 Rgba32 {get;}

        public GridColour(Rgba32 colour)
        {
            Rgba32 = colour;
        }
    }

    public class CharacterColour : GridColour
    {
        public CharacterColour(Rgba32 colour) : base(colour)
        {
            
        }
    }

    public class KeywordColour : GridColour
    {
        public string Keyword {get;}

        public KeywordColour(Rgba32 colour, string keyword) : base(colour)
        {
            Keyword = keyword;
        }
    }

}