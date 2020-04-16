using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Cloud.Language.V1;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.SentimentGenerator
{
    public class SentimentGenerator
    {
        private const int MaxCharacters = 2400;
        
        private readonly List<int[]> _variations;
        private readonly List<Letter> _letters;
        private readonly ColourPalette _colourPalette;
        private readonly Random _random;
        private char[] _message;

        public SentimentGenerator()
        {
            _random = new Random();
            _variations = new List<int[]>();
            _letters = new List<Letter>();
            _colourPalette = ColourPalette.SelectPalette();

            GenerateVariations();
            MapLetters();
        }
        
        public string Generate(string message)
        {
            if (message.Length > MaxCharacters)
                throw new ArgumentException("Message should be 2406 characters or less.");
            if (message.Length == 0)
                throw new ArgumentException("Message should contain at least one character.");

            _message = message.ToCharArray();

            return Draw();

            // Run sentiment analysis
            //var client = LanguageServiceClient.Create();
//            var response = client.AnnotateText(document, new AnnotateTextRequest.Types.Features 
            // {
            //     ExtractDocumentSentiment = true, ExtractEntitySentiment = true
            // });
            //
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

                var colour = _colourPalette.SelectColour();
                for (var y = 0 + (yOffset * 14); y < 14 + (yOffset * 14); y++)
                {
                    var pixelRowSpan = image.GetPixelRowSpan(y);

                    for (var x = 0 + (xOffset * 14); x < 14 + (xOffset * 14); x++)
                    {
                        // Reset x to be between 0 and 14 so we can use it as the index when accessing
                        // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                        // within the bounds of the fill array which has a length of 7.
                        var xWithoutOffset = x - (xOffset * 14);
                        var indexForFill = xWithoutOffset / 2;

                        // Find the mapping for the character
                        var indexQuery = i;
                        var letter = _letters.First(c => c.Char == _message[indexQuery]);
                        if (letter.Fill[indexForFill] == 1)
                            pixelRowSpan[x] = colour;
                    }
                }
            }        }

        private void DrawLetterMappings(Image<Rgba32> image)
        {
            var yOffset = 0;
            for (var i = 0; i < _letters.Count; i++)
            {
                var xOffset = i % 50;
                if (i != 0 && i % 50 == 0)
                    yOffset++;

                var colour = _colourPalette.SelectColour();
                for (var y = 0 + (yOffset * 14); y < 14 + (yOffset * 14); y++)
                {
                    var pixelRowSpan = image.GetPixelRowSpan(y);

                    for (var x = 0 + (xOffset * 14); x < 14 + (xOffset * 14); x++)
                    {
                        // Reset x to be between 0 and 14 so we can use it as the index when accessing
                        // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                        // within the bounds of the fill array which has a length of 7.
                        var xWithoutOffset = x - (xOffset * 14);
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

            for (int i = 32; i <= 126; i++)
            {
                var index = _random.Next(tmpVariations.Count - 1);
                var letter = new Letter {Char = (char) i, Fill = tmpVariations[index]};

                // Removing the variation prevents us from accidently picking the same fill more than once.
                tmpVariations.RemoveAt(index);

                _letters.Add(letter);
            }
        }
    }
}