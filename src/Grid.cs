using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    // ToDo: Look into using factory or builder pattern for grids?

    internal abstract class Grid
    {
    }

    internal class PatternGrid : Grid
    {
        private int[] _pattern;
        public int[] Pattern
        { 
            get => _pattern;
            internal set {
                if(value.Length != Constraints.GridSize)
                    throw new Exception($"Pattern array length must be {Constraints.GridSize}");
                
                _pattern = value;
            }   
        }
    }

    internal class CharacterGrid : PatternGrid
    {
        public char Character { get; }

        public CharacterGrid(char character) : base()
        {
            Character = character;
        }
    }

    internal class ColourDataGrid : PatternGrid
    {
        public Color Colour { get; }
        public ColourDataGrid(Color colour) : base()
        {
            Colour = colour;
        }
    }

    internal class KeywordGrids : Grid
    {
        public string Keyword { get; }
        public Color Colour { get; }

        public KeywordGrids(string keyword, Color colour) : base()
        {
            Keyword = keyword;
            Colour = colour;
        }
    }
}