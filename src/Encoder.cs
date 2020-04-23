using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly List<Letter> _letters;
        private readonly Random _random;
        private readonly List<int[]> _variations;
        private char[] _text;
        private int _row;
        private int _column;


        public Encoder(IColourPalette colourPalette)
        {
            _random = new Random();
            _variations = new List<int[]>();
            _letters = new List<Letter>();
            _colourPalette = colourPalette ?? throw new ArgumentNullException(nameof(colourPalette));
            _column = 0;
            _row = 0;
            GenerateVariations();
            MapLetters();
        }

        public string Generate(string message)
        {
            if (message.Length > MaxCharacters)
                throw new ArgumentException("Message should be 2406 characters or less.");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));

            _text = message.ToCharArray();

            return Encode();
        }

        private string Encode()
        {
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
            foreach (var character in _text)
            {
                var column = GetColumn();
                SetRowAndColumn();
                var letter = _letters.First(x => x.Char ==  character);
                PaintSquare(image, _column, letter.Fill);
            }
        }

    

        private void EncodeColourPalette(Image<Rgba32> image)
        {
            foreach (var colour in _colourPalette.Colours)
            {
                var choice = _random.Next(0, 128);
                var column = GetColumn();
                SetRowAndColumn();

                for (var y = 0 + _row * 14; y < 14 + _row * 14; y++)
                {
                    var pixelRowSpan = image.GetPixelRowSpan(y);
                    
                    // The column variable defines the column number. Multiplying it by 14 will give us
                    // the 14 x 14 grid we need.
                    for (var x = column * 14; x < 14 + column * 14; x++)
                    {
                        // Reset x to be between 0 and 14 so we can use it as the index when accessing
                        // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                        // within the bounds of the fill array which has a length of 7.
                        var indexForFill =  (x - column * 14) / 2;
                        if (_variations[choice][indexForFill] == 1)
                            pixelRowSpan[x] = colour;
                    }
                }

                _column++;
            }
        }

        private void SetRowAndColumn()
        {
            if (_column == 0 || _column % 50 != 0) 
                return;
           
            _row++;
            _column = 0;
        }

        private void EncodeLetterMappings(Image<Rgba32> image)
        {
            foreach (var letter in _letters)
            {
                var column = GetColumn();
                SetRowAndColumn();
                PaintSquare(image, column, letter.Fill);
            }
            var delCharacter = _letters.First(x => x.Char == (char) 127);
            PaintSquare(image, _column, delCharacter.Fill);
        }

        private int GetColumn()
        {
            return _column % 50;
        }


        private void PaintSquare(Image<Rgba32> image, int columnToPaint, int[] fill)
        {
            var colour = _colourPalette.SelectRandomColour();
            for (var y = 0 + _row * 14; y < 14 + _row * 14; y++)
            {
                var pixelRowSpan = image.GetPixelRowSpan(y);

                for (var x = 0 + columnToPaint * 14; x < 14 + columnToPaint * 14; x++)
                {
                    // Reset x to be between 0 and 14 so we can use it as the index when accessing
                    // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                    // within the bounds of the fill array which has a length of 7.
                    var xWithoutOffset = x - columnToPaint * 14;
                    var indexForFill = xWithoutOffset / 2;

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

            for (var i = 0; i <= 127; i++)
            {
                var binary = Convert.ToString(i, 2).PadLeft(7, '0');
                var binaryAsCharArray = binary.ToCharArray();
                _variations.Add(binaryAsCharArray.Select(x => int.Parse(x.ToString())).ToArray());
            }
        }

        private void MapLetters()
        {
            // Temporary variable just to keep things tidy.
            var tmpVariations = new List<int[]>(_variations);

            for (var i = 32; i <= 127; i++)
            {
                var index = _random.Next(tmpVariations.Count - 1);
                var letter = new Letter {Char = (char) i, Fill = tmpVariations[index]};

                // Removing the variation prevents us from accidentally picking the same fill more than once.
                tmpVariations.RemoveAt(index);

                _letters.Add(letter);
            }
        }
    }
}