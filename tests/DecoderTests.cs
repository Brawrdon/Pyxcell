using Pyxcell;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace PyxcellTests
{
    public class DecoderTests
    {
        private readonly DecodedImage _decodedImage;
        
        public DecoderTests(ITestOutputHelper output)
        {
            var decoder = new Decoder("EncodedTestImage.png");
            _decodedImage = decoder.Decode();
        }

        [Fact]
        public void DecodeText()
        {
            // Assumes character encoding is correct if text decodes successfully.
            Assert.Equal("You need to install the plugin and activate it for your stream on their website." +
                         "Sometimes, I really like it when my friends buy me food without me asking." +
                         "Don't you think it's great that we can all have a blast on this tiny rock." +
                         "I like cheese most days. But sometimes, I don't.", _decodedImage.Text);
        }
        
        [Fact]
        public void DecodeColourPalettes()
        {
            Assert.Equal(10, _decodedImage.ColourPalette.Colours.Count);
            Assert.Single(_decodedImage.KeywordColourPalette.Colours);
        }
        
        [Fact]
        public void DecodeKeywords()
        {
            Assert.Equal(3, _decodedImage.Keywords.Count);
            Assert.Equal("activate",  _decodedImage.Keywords[0].Word);
            Assert.Equal(Rgba32.Black,  _decodedImage.Keywords[0].Colour);
            
            Assert.Equal("rock",  _decodedImage.Keywords[1].Word);
            Assert.Equal(Rgba32.Silver,  _decodedImage.Keywords[1].Colour);
            
            Assert.Equal("cheese",  _decodedImage.Keywords[2].Word);
            Assert.Equal(Rgba32.Yellow,  _decodedImage.Keywords[2].Colour);
        }
    }
}