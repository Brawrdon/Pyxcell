using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.Grids
{
    public abstract class Grid : IGrid
    {
        public int[] Pattern { get; }

        public Grid()
        {
            Pattern = new int[14];
        }
    }

    public class CharacterGrid : Grid
    {
        public char Character { get; }

        public CharacterGrid(char character) : base()
        {
            Character = character;
        }
    }

    public class KeywordGrids : Grid
    {
        public List<CharacterGrid> CharacterGrids { get; }
        public string Keyword { get; }
        public Rgba32 Colour { get; }

        public KeywordGrids(string keyword, Rgba32 colour) : base()
        {
            Keyword = keyword;
            Colour = colour;
            CharacterGrids = new List<CharacterGrid>();

            foreach (var character in keyword.ToCharArray())
            {
                CharacterGrids.Add(new CharacterGrid(character));
            }
        }
    }
}