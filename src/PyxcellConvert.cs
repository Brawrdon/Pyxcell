using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    public static class PyxcellConvert 
    {
        const int Width = 700;
        const int Height = 700;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string Encode(string message, List<Color> colours, Dictionary<string, Color> keywords = null)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
            
            var encodingData = new EncodingData(colours, keywords);
            message = ValidateMessage(message, colours, keywords, encodingData);
                        
            using var image = new Image<Rgba32>(Width, Height);
            // ToDo: Maybe use extension methods on Image?
            var painter = new Painter(image, message, encodingData);
            painter.Paint();
            
            return image.ToBase64String(PngFormat.Instance);
        }


        public static PyxcellImage Decode(string filePath)
        {
            using var image = Image.Load<Rgba32>(filePath);
            var pyxcellImage = new PyxcellImage(image.ToBase64String(PngFormat.Instance));
            var decoder = new Decoder(image);

            return pyxcellImage;
            // ToDo: Verify image is valid first. This could be done on the fly...


        }

        internal static (int, int) GetGridPosition(int gridNumber)
        {
            var row = (gridNumber / 50) * Constraints.GridSize;
            var column = (gridNumber % 50) * Constraints.GridSize;

            return (row, column);
        }

        private static string ValidateMessage(string message, List<Color> colours, Dictionary<string, Color> keywords, EncodingData encodingData)
        {
            var characterCount = Constraints.EndCharacter - Constraints.StartCharacter;
            var colourCount = encodingData.Colours.Count() + 1;
            var keywordColourCount = encodingData.Grids.OfType<KeywordGrids>().Count();

            if (keywordColourCount != 0)
                keywordColourCount++;

            var metadataTotal = characterCount + colourCount + keywordColourCount;

            var maxMessageLength = (50 * 50) - metadataTotal;

            if (maxMessageLength < 0)
                throw new Exception("Too much metadata.");

            if (message.Length > maxMessageLength)
                throw new ArgumentException($"Message should be {maxMessageLength} characters or less.");

            return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(message));
        }
   
    }
}