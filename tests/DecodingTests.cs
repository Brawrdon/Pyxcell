using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using Xunit;
using Xunit.Abstractions;

namespace Pyxcell
{
    public class DecoderTests
    {
        private readonly ITestOutputHelper output;

        public DecoderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Decode_Succesfull_Exception()
        {
            // Arrange & Act
            var image = PyxcellConvert.Decode("HelloWorld.png");

            // Assert
            Assert.Equal(96, image.CharacterPatternMap.Count);
            Assert.Equal("Hello world, how are you?", image.Message);
            Assert.Contains("Hello", image.Keywords.Keys);
            Assert.Contains("world", image.Keywords.Keys);

            // ToDo: add colour tests because not all colours may be used due to random selection
        }

        [Fact]
        public void Decode_EmptyGrid_Exception()
        {
            // Arrange, Act & Assert
            Assert.Throws<Exception>(() => PyxcellConvert.Decode("Empty.png"));
        }
    }
}