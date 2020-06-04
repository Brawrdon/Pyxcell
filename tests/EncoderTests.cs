using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace Pyxcell
{
    public class EncoderTests
    {
        private readonly ITestOutputHelper output;

        public EncoderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Encode_EncodeValidData_OutputImage()
        {
            // Arrange
            var colours = new List<Color>
            {
                Color.Aqua,
                Color.Chocolate,
                Color.Fuchsia,
                Color.Khaki,
                Color.Blue,
                Color.Salmon,
                Color.Coral,
                Color.DarkGreen,
                Color.Lime,
                Color.Crimson
            };

            var keywords = new Dictionary<string, Color>
            {  
                { "Hello", Color.Red },
                { "world", Color.Purple }
            };

            var image = PyxcellConvert.Encode("Hello world, how are you?", colours, keywords);
            

            // Assert
        }
    }
}