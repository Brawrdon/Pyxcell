using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    public abstract class Grid
    {
        public Rgba32 Rgba32 { get; }

        public Grid(Rgba32 colour)
        {
            Rgba32 = colour;
        }
    }

    public class CharacterGrid : Grid
    {
        public char Character { get; }

        public CharacterGrid(Rgba32 colour, char character) : base(colour)
        {
            Character = character;
        }
    }

    public class KeywordGrids : Grid
    {
        public List<CharacterGrid> CharacterGrids { get; }
        public string Keyword { get; }

        public KeywordGrids(Rgba32 colour, string keyword) : base(colour)
        {
            Keyword = keyword;

            foreach (var character in keyword.ToCharArray())
            {
                CharacterGrids.Add(new CharacterGrid(colour, character));
            }
        }
    }

}