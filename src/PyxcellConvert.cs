using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
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
            
            var gridPalette = new GridPalette(colours, keywords);
            ValidateMessage(message, colours, keywords, gridPalette);

            message = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(message));
            
            using var image = new Image<Rgba32>(Width, Height);
            var painter = new Painter(image, gridPalette);
            painter.DrawCharacterGrids();
            
            using var outputStream = new MemoryStream();
            image.SaveAsPng(outputStream);
            image.Dispose();
            var bytes = outputStream.ToArray();
            return Convert.ToBase64String(bytes);
        }

        private static void ValidateMessage(string message, List<Color> colours, Dictionary<string, Color> keywords, GridPalette gridPalette)
        {
            var characterCount = GridPalette.EndCharacter - GridPalette.StartCharacter;
            var colourCount = gridPalette.Colours.Count() + 1;
            var keywordColourCount = gridPalette.Grids.OfType<KeywordGrids>().Count();

            if (keywordColourCount != 0)
                keywordColourCount++;

            var metadataTotal = characterCount + colourCount + keywordColourCount;

            var maxMessageLength = (50 * 50) - metadataTotal;

            if (maxMessageLength < 0)
                throw new Exception("Too much metadata.");

            if (message.Length > maxMessageLength)
                throw new ArgumentException($"Message should be {maxMessageLength} characters or less.");
        }
    }
}