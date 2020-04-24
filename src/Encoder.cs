using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pyxcell.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    public class Encoder : IEncoder
    {
        private const int MaxCharacters = 2400;
        private readonly IColourPalette _colourPalette;
        private readonly IColourPalette _keywordColourPalette;
        private readonly List<Keyword> _keywords;
        private readonly List<CharacterGrid> _characters;
        private readonly Random _random;
        private List<int[]> _variations;
        private List<string> _words;
        private int _row;
        private int _column;


        public Encoder(IColourPalette colourPalette, List<Keyword> keywords = null)
        {
            _colourPalette = colourPalette ?? throw new ArgumentNullException(nameof(colourPalette));
            _keywordColourPalette = new ColourPalette();
            _keywords = keywords ?? new List<Keyword>();
            
            SetPalettes();

            _random = new Random();
            _variations = new List<int[]>();
            _characters = new List<CharacterGrid>();
            
            _column = 0;
            _row = 0;
            GenerateVariations();
            MapCharacters();
        }

        private void SetPalettes()
        {
            // A keyword colour cannot be a standard colour inside _colourPalette
            foreach (var keyword in _keywords)
            {
                _keywordColourPalette.AddColour(keyword.Colour);
                if (_colourPalette.Colours.Contains(keyword.Colour))
                    _colourPalette.Colours.Remove(keyword.Colour);
            }
        }

        public string Generate(string message)
        {
            if (message.Length > MaxCharacters)
                throw new ArgumentException("Message should be 2406 characters or less.");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));

            _words = Regex.Split(message, @"\s+|(?!^)(?=\p{P})|(?<=\p{P})(?!$)").ToList();

            return Encode();
        }

        private string Encode()
        {
            if (_colourPalette.Colours.Count == 0)
                throw new Exception("Colour palette must contain at least one colour.");
                
            const int width = 700;
            const int height = 700;

            using var image = new Image<Rgba32>(width, height);

            EncodeLetterMappings(image);
            EncodeColourPalette(image);
            EncodeText(image);

            using var outputStream = new MemoryStream();
            image.SaveAsPng(outputStream);
            var bytes = outputStream.ToArray();
            return Convert.ToBase64String(bytes);
        }

        private void EncodeText(Image<Rgba32> image)
        {
            foreach (var word in _words)
            {
                Rgba32 colour = default;
                var keyword = _keywords.FirstOrDefault(x => x.Word == word);
                if (keyword != null)
                    colour = keyword.Colour;
                        
                foreach (var letter in word.Select(character => _characters.First(x => x.Char ==  character)))
                {
                    PaintGrid(image, letter.Fill, colour);
                }
               
            }
            
            EncodeDelCharacter(image);
        }



        private void EncodeColourPalette(Image<Rgba32> image)
        {
            foreach (var colour in _colourPalette.Colours)
            {
                var choice = _random.Next(0, _variations.Count);
            
                PaintGrid(image, _variations[choice], colour);
            }
            
            EncodeDelCharacter(image);
        }

        private void SetRowAndColumn()
        {
            if (_column == 0 || _column % 50 != 0) 
                return;
           
            _row++;
            _column = 0;
        }
        
        private int GetColumn()
        {
            return _column % 50;
        }
        
        private void EncodeLetterMappings(Image<Rgba32> image)
        {
            foreach (var letter in _characters)
                PaintGrid(image, letter.Fill);
        }

        private void EncodeDelCharacter(Image<Rgba32> image)
        {
            var delCharacter = _characters.First(x => x.Char == (char) 127);
            PaintGrid(image, delCharacter.Fill);
        }

        private void PaintGrid(Image<Rgba32> image, int[] fill, Rgba32 colour = default)
        {
            if (colour == default)
                colour  = _colourPalette.SelectRandomColour();
            
            var column = GetColumn();
            SetRowAndColumn();
            
            for (var y = 0 + _row * 14; y < 14 + _row * 14; y++)
            {
                var pixelRowSpan = image.GetPixelRowSpan(y);

                // Columns take a 14 x 14 grid.
                for (var x = column * 14; x < 14 + column * 14; x++)
                {
                    // Reset x to be between 0 and 14 so we can use it as the index when accessing
                    // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                    // within the bounds of the fill array which has a length of 7.
                    var indexForFill = (x - column * 14) / 2;

                    if (fill[indexForFill] == 1)
                        pixelRowSpan[x] = colour;
                }
            }
            _column++;
        }


        private void GenerateVariations()
        {
            // We are supporting ASCII characters 32 - 127 giving us a total of 95 characters.
            // These characters are going to be randomly mapped to a shape which is defined by a 
            // 10 x 10 grid with 7 segments inside. The segments will be either on (painted) or off.
            // 127 represents the maximum value that can be represented in a 7-bit binary value;
            // We start with 1 to make sure no grid is empty.
            for (var i = 1; i <= 127; i++)
            {
                var binary = Convert.ToString(i, 2).PadLeft(7, '0');
                var binaryAsCharArray = binary.ToCharArray();
                _variations.Add(binaryAsCharArray.Select(x => int.Parse(x.ToString())).ToArray());
            }
        }

        private void MapCharacters()
        {
            // Temporary variable just to keep things tidy.
            var tmpVariations = new List<int[]>(_variations);

            for (var i = 32; i <= 127; i++)
            {
                var index = _random.Next(tmpVariations.Count - 1);
                var colour  = _colourPalette.SelectRandomColour();

                var letter = new CharacterGrid((char) i, tmpVariations[index]);

                // Removing the variation prevents us from accidentally picking the same fill more than once.
                tmpVariations.RemoveAt(index);

                _characters.Add(letter);
            }

            _variations = tmpVariations;
        }
    }
}