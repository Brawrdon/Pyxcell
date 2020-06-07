using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace Pyxcell
{
    public class EncoderTests
    {
        private readonly ITestOutputHelper _output;
        private readonly List<Color> _defaultColours;

        public EncoderTests(ITestOutputHelper output)
        {
            _output = output;
            _defaultColours = new List<Color>
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
        }

        [Fact]
        public void Encode_EncodeValidData_OutputImage()
        {
            var keywords = new Dictionary<string, Color>
            {  
                { "Hello", Color.Red },
                { "world", Color.Purple }
            };

            var image = PyxcellConvert.Encode("Hello world, how are you?", _defaultColours, keywords);
                
        }

        [Fact]
        public void Encode_EmptyOrNullMessage_Exception()
        {
            // Arrange
            var colours = new List<Color>()
            {
                Color.Cornsilk
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => PyxcellConvert.Encode("", colours, new Dictionary<string, Color>()));
            Assert.Throws<ArgumentException>(() => PyxcellConvert.Encode(string.Empty, colours, new Dictionary<string, Color>()));
            Assert.Throws<ArgumentException>(() => PyxcellConvert.Encode(null, colours, new Dictionary<string, Color>()));
        }

        [Fact]
        public void Encode_EmptyOrNullColours_Exception()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => PyxcellConvert.Encode("Hello world!", new List<Color>(), new Dictionary<string, Color>()));
            Assert.Throws<ArgumentException>(() => PyxcellConvert.Encode("Hello world", null, new Dictionary<string, Color>()));
        }

        [Fact]
        public void Encode_PatternLimit_Exception()
        {
            // Arrange
            var colours = new List<Color>();
            for (int i = 0; i < 128; i++)
            {
                var valueOne = Convert.ToByte(i % 256);
                var byteMax =  Convert.ToByte(255);
                var byteMin =  Convert.ToByte(0);
                colours.Add(new Rgba32(valueOne, byteMax, byteMax));
            }

            // Act & Assert
            Assert.Throws<Exception>(() => PyxcellConvert.Encode("Hello world!", colours, new Dictionary<string, Color>()));
        }

        [Fact]
        public void Encode_MessageLengthLimit_Exception()
        {
            // Arrange
            var message = string.Empty;
            for (int i = 0; i < 2500; i++)
                message += "x";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => PyxcellConvert.Encode(message, _defaultColours, new Dictionary<string, Color>()));
        }

        [Fact]
        public void Encode_KeywordColourInMainPalette_Exception()
        {
            // Arrange
            var colours = new List<Color>
            {
                Color.Blue,
                Color.Teal
            };

            var keywords = new Dictionary<string, Color>
            {
                {"Hello", Color.Teal}
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => PyxcellConvert.Encode("Hello world", colours, keywords));
        }
    }
}