using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using System.Linq;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    
    class Decoder
    {
        private int _gridNumber;
        private CharacterGrid _delChar;
        public List<Grid> Grids { get; }
        private Image<Rgba32> _image;

        public Decoder(Image<Rgba32> image)
        {
            _gridNumber = 0;
            _image = image; 
        } 

        public PyxcellImage Decode()
        {
            var characters = DecodeCharacterGrids();
            var colours = DecodeColourGrids();
            var keywordColours = DecodeColourGrids();
            var message = DecodeMessage();

            return null;
        }

        private List<CharacterGrid> DecodeCharacterGrids()
        {
            var characterGrids = new List<CharacterGrid>();
            for (int i = Constraints.StartCharacter; i <= Constraints.EndCharacter; i++)
            {
                var characterGrid = new CharacterGrid((char) i) { Pattern = DecodeGridPattern() };
                characterGrids.Add(characterGrid);
                _gridNumber++;
            }

            _delChar = characterGrids[characterGrids.Count - 1];

            return characterGrids;
        }

        private List<ColourDataGrid> DecodeColourGrids()
        {
            var colourDataGrids = new List<ColourDataGrid>();
            ColourDataGrid colourDataGrid = new ColourDataGrid(DecodeGridColour()) { Pattern = DecodeGridPattern() };
            while (!colourDataGrid.Pattern.SequenceEqual(_delChar.Pattern))
            {
                colourDataGrids.Add(colourDataGrid);
                _gridNumber++;
                colourDataGrid = new ColourDataGrid(DecodeGridColour()) { Pattern = DecodeGridPattern() };
            }       
            
            _gridNumber++; // Skips the return character
            return colourDataGrids; 
        }

        private Color DecodeGridColour()
        {
            var (row, column) = PyxcellConvert.GetGridPosition(_gridNumber);

            var pixelRow = _image.GetPixelRowSpan(row);

            Color colour = Color.Transparent;
            for (int i = 0; i < Constraints.GridSize; i++)
            {
                colour = new Rgba32(pixelRow[column + i].Rgba);
                if (colour != Color.Transparent)
                    break;     
            }

            if (colour == Color.Transparent)
                throw new Exception($"Could not find colour for grid {_gridNumber}.");

            return colour;
        }

        private object DecodeMessage()
        {
            throw new NotImplementedException();
        }

        private int[] DecodeGridPattern()
        {
            var (row, column) = PyxcellConvert.GetGridPosition(_gridNumber);

            var pixelRow = _image.GetPixelRowSpan(row);

            var pattern = new int[Constraints.GridSize];
            for (int i = 0; i < pattern.Length; i++)
            {
                Color columnFill = new Rgba32(pixelRow[column + i].Rgba);
                pattern[i] = columnFill == Color.Transparent ? 0 : 1;
            }

            return pattern;
        }

    }
}