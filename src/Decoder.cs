using System.Collections.Generic;
using Pyxcell.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using static System.String;

namespace Pyxcell
{
    public class DecodedImage
    {
        
        public  IColourPalette ColourPalette { get; }
        public List<Letter> Letters { get;}
        public string Text;
        
        internal DecodedImage()
        {
            ColourPalette = new ColourPalette();
            Letters = new List<Letter>();
            Text = Empty;
        }
    }
    
    public class Decoder : IDecoder
    {
        // To decode...
        // We need to get the base64Image
        // We read the first 94 spaces to determine the characters...
        // The next space shows us how many colours there are in the palette
        // The next number of spaces from above == the colours used

        private readonly string _imagePath;
        private readonly DecodedImage _decodedImage;


        public Decoder(string imagePath)
        {
            _imagePath = imagePath; 
            _decodedImage = new DecodedImage();
        }

        public DecodedImage Decode()
        {
            using (var image = Image.Load<Rgba32>(_imagePath)) 
            {
                DecodeLetterMappings(image);
            }

            return _decodedImage;
        }
        
     
        private void DecodeLetterMappings(Image<Rgba32> image)
        {
            var yOffset = 0;
            //ToDo: Remove hard coding for letter count
            for (var i = 0; i < 95; i++)
            {
                var xOffset = i % 50;
                if (i != 0 && i % 50 == 0)
                    yOffset++;

                var letter = new Letter { Char = (char)(i + 32), Fill = new int[7]};
                for (var y = 0 + yOffset * 14; y < 14 + yOffset * 14; y++)
                {
                    var pixelRowSpan = image.GetPixelRowSpan(y);
                    for (var x = 0 + xOffset * 14; x < 14 + xOffset * 14; x += 2)
                    {
                        // Reset x to be between 0 and 14 so we can use it as the index when accessing
                        // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                        // within the bounds of the fill array which has a length of 7.
                        var xWithoutOffset = x - xOffset * 14;
                        var indexForFill = xWithoutOffset / 2;

                        var colour = new Rgba32(pixelRowSpan[x].PackedValue);
                        letter.Fill[indexForFill] = colour.Equals(Rgba32.Transparent) || colour.Equals(new Rgba32(0, 0, 0, 0)) ? 0 : 1;
                    }
                }
                
                _decodedImage.Letters.Add(letter); 
            }
        }
    }
}