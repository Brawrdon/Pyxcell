using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Language.V1;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.SentimentGenerator
{
    public class SentimentGenerator
    {
        private List<int[]> Variations { get; }
        private List<Letter> Letters { get; }
        
        public SentimentGenerator()
        {
            Variations = new List<int[]>();
            Letters = new List<Letter>();
            GenerateVariations();
            MapLetters();
        }
        
        public void Generate(string message, Rgba32 positiveColour,  Rgba32 negativeColour)
        {
            // ToDo: Randomly assign four colours
            var colourOne = Rgba32.Aqua;
            var colourTwo = Rgba32.Black;
            var colourThree = Rgba32.White;
            var colourFour = Rgba32.Olive;

            // Run sentiment analysis
            //var client = LanguageServiceClient.Create();
//            var response = client.AnnotateText(document, new AnnotateTextRequest.Types.Features 
            // {
            //     ExtractDocumentSentiment = true, ExtractEntitySentiment = true
            // });
            //

        }

        private void GenerateVariations()
        {
            // We are supporting ASCII characters 32 - 126 giving us a total of 94 characters.
            // These characters are going to be randomly mapped to a shape which is defined by a 
            // 10 x 10 grid with 7 segments inside. The segments will be either on (painted) or off.
            // 127 represents the maximum value that can be represented in a 7-bit binary value;
            
            for (var i = 0; i <= 127; i++)
            {
                var binary = Convert.ToString(i, 2).PadLeft(8, '0');
                var binaryAsCharArray = binary.ToCharArray();
                Variations.Add(binaryAsCharArray.Select(x => int.Parse(x.ToString())).ToArray());
            }
        }

        private void MapLetters()
        {
            // Temporary variable just to keep things tidy.
            var tmpVariations = new List<int[]>(Variations);
            
            var rnd = new Random();

            for (int i = 32; i <= 126; i++)
            {
                var index = rnd.Next(tmpVariations.Count - 1);
                var letter = new Letter { Char = (char) i, Fill = tmpVariations[index]};

                // Removing the variation prevents us from accidently picking the same fill more than once.
                tmpVariations.RemoveAt(index);
                
                Letters.Add(letter);
            }
        }
    }
}