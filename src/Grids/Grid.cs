using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.Grids
{
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
                if(value.Length != 14)
                    throw new Exception("Pattern array length must be 14");
                
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
        public Rgba32 Colour { get; }
        public ColourDataGrid(Rgba32 colour) : base()
        {
            Colour = colour;
        }
    }

    internal class KeywordGrids : Grid
    {
        public string Keyword { get; }
        public Rgba32 Colour { get; }

        public KeywordGrids(string keyword, Rgba32 colour) : base()
        {
            Keyword = keyword;
            Colour = colour;
        }
    }
}