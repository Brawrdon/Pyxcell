using System.Collections.Generic;
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
        public void GenerateHelloWord()
        {
            var colourPalette = new ColourPalette();

            colourPalette.AddCharacterColour(new CharacterColour(Rgba32.Red));
            colourPalette.AddCharacterColour(new CharacterColour(Rgba32.Green));
            colourPalette.AddCharacterColour(new CharacterColour(Rgba32.Blue));
            colourPalette.AddKeywordColour(new KeywordColour(Rgba32.Violet, "world"));

        }

    }
}