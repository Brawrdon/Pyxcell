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
        private Image<Rgba32> _image;

        public Decoder(Image<Rgba32> image)
        {
            _gridNumber = 0;
            _image = image; 
        } 

        public PyxcellImage GeneratePyxcellImage(string base64)
        {
            var characterPatternMap = DecodeCharacterGrids();
            var colours = DecodeColourGrids();
            var keywordColours = DecodeColourGrids();
            var (message, keywords) = DecodeMessage(characterPatternMap, keywordColours);

            var pyxcellImage = new PyxcellImage(base64)
            {
                Message = message,
                Keywords = keywords,
                CharacterPatternMap = characterPatternMap,
                Colours = colours
            };

            return pyxcellImage;
        }

        private Dictionary<char, int[]> DecodeCharacterGrids()
        {
            var characterPatternMap = new Dictionary<char, int[]>();
            for (int i = Constraints.StartCharacter; i <= Constraints.EndCharacter; i++)
            {
                var character = (char) i;
                var pattern = DecodeGridPattern();

                characterPatternMap.Add(character, pattern);

                if(i == Constraints.EndCharacter)
                    _delChar = new CharacterGrid(character) { Pattern = pattern};

                _gridNumber++;
            }
            return characterPatternMap;
        }

        private List<Color> DecodeColourGrids()
        {
            var colours = new List<Color>();
            var colourDataGrid = new ColourDataGrid(DecodeGridColour()) { Pattern = DecodeGridPattern() };
            while (!colourDataGrid.Pattern.SequenceEqual(_delChar.Pattern))
            {
                colours.Add(colourDataGrid.Colour);
                _gridNumber++;
                colourDataGrid = new ColourDataGrid(DecodeGridColour()) { Pattern = DecodeGridPattern() };
            }       
            
            _gridNumber++; // Skips the return character
            return colours; 
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

        private (string, Dictionary<string, Color>) DecodeMessage(Dictionary<char, int[]> characterPatternMap, List<Color> keywordColours)
        {
            var message = string.Empty;
            var keywords = new Dictionary<string, Color>();
            var pattern = DecodeGridPattern();
            while(!pattern.SequenceEqual(_delChar.Pattern))
            {
                var character = characterPatternMap.Single(x => x.Value.SequenceEqual(pattern)).Key;
                var colour = DecodeGridColour();

                if(keywordColours.Contains(colour))
                {
                    var keyword = "" + character;
                    _gridNumber++;
                    pattern = DecodeGridPattern();
                    character = characterPatternMap.Single(x => x.Value.SequenceEqual(pattern)).Key;
                    var nextColour = DecodeGridColour();

                    while (nextColour == colour)
                    {
                        keyword += character;
                        _gridNumber++;
                        pattern = DecodeGridPattern();
                        character = characterPatternMap.Single(x => x.Value.SequenceEqual(pattern)).Key;
                        nextColour = DecodeGridColour();
                    } 

                    message += keyword;
                    keywords.Add(keyword, colour);
                }

                message += character;
                
                _gridNumber++;
                pattern = DecodeGridPattern();
            }

            return (message, keywords);        
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