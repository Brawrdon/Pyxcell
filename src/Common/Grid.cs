using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.Common
{
    public abstract class Grid
    {
        public int[] Fill { get; }

        protected Grid(int[] fill)
        {
            Fill = fill;
        }
        
        protected Grid()
        {
            Fill = new int[7];
        }
    }

    public class CharacterGrid : Grid
    {
        public char Char { get; set; }
        
        public CharacterGrid(char character, int[] fill) : base(fill)
        {
            Char = character;
        }
        
        public CharacterGrid(char character) : base()
        {
            Char = character;
        }

    }

    public class ColourGrid : Grid
    {
        public ColourGrid(Rgba32 colourGrid, int[] fill) : base(fill)
        {
            Colour = colourGrid;
        }
        
        public ColourGrid(Rgba32 colourGrid) : base()
        {
            Colour = colourGrid;
        }
        public Rgba32 Colour { get; set; }
    }
}