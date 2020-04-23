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
        private char[] _message;

        public Encoder(IColourPalette colourPalette)
        {
            _random = new Random();
            _variations = new List<int[]>();
            _letters = new List<Letter>();

            _colourPalette = colourPalette ?? throw new ArgumentNullException(nameof(colourPalette));

            GenerateVariations();
            MapLetters();
        }

        public string Generate(string message)
        {
            if (message.Length > MaxCharacters)
                throw new ArgumentException("Message should be 2406 characters or less.");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));

            _message = message.ToCharArray();

            return Draw();
        }

        private string Draw()
        {
            const int width = 700;
            const int height = 700;

            using var image = new Image<Rgba32>(width, height);

            DrawLetterMappings(image);
            DrawLetters(image);

            using var outputStream = new MemoryStream();
            image.SaveAsPng(outputStream);
            var bytes = outputStream.ToArray();
            return Convert.ToBase64String(bytes);
        }

        private void DrawLetters(Image<Rgba32> image)
        {
            // yOffset starts at one due to the letter mappings taking 1 and a haf lines.
            var yOffset = 2;
            for (var i = 0; i < _message.Length; i++)
            {
                var xOffset = i % 50;
                if (i != 0 && i % 50 == 0)
                    yOffset++;

                var colour = _colourPalette.SelectRandomColour();
                for (var y = 0 + yOffset * 14; y < 14 + yOffset * 14; y++)
                {
                    var pixelRowSpan = image.GetPixelRowSpan(y);

                    for (var x = 0 + xOffset * 14; x < 14 + xOffset * 14; x++)
                    {
                        // Reset x to be between 0 and 14 so we can use it as the index when accessing
                        // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                        // within the bounds of the fill array which has a length of 7.
                        var xWithoutOffset = x - xOffset * 14;
                        var indexForFill = xWithoutOffset / 2;

                        // Find the mapping for the character
                        var indexQuery = i;
                        var letter = _letters.First(c => c.Char == _message[indexQuery]);
                        if (letter.Fill[indexForFill] == 1)
                            pixelRowSpan[x] = colour;
                    }
                }
            }
        }

        private void DrawLetterMappings(Image<Rgba32> image)
        {
            var yOffset = 0;
            for (var i = 0; i < _letters.Count; i++)
            {
                var xOffset = i % 50;
                if (i != 0 && i % 50 == 0)
                    yOffset++;

                var colour = _colourPalette.SelectRandomColour();
                for (var y = 0 + yOffset * 14; y < 14 + yOffset * 14; y++)
                {
                    var pixelRowSpan = image.GetPixelRowSpan(y);

                    for (var x = 0 + xOffset * 14; x < 14 + xOffset * 14; x++)
                    {
                        // Reset x to be between 0 and 14 so we can use it as the index when accessing
                        // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                        // within the bounds of the fill array which has a length of 7.
                        var xWithoutOffset = x - xOffset * 14;
                        var indexForFill = xWithoutOffset / 2;

                        if (_letters[i].Fill[indexForFill] == 1)
                            pixelRowSpan[x] = colour;
                    }
                }
            }
        }


        private void GenerateVariations()
        {
            // We are supporting ASCII characters 32 - 126 giving us a total of 94 characters.
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

            for (var i = 32; i <= 126; i++)
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