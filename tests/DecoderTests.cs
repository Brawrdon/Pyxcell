using Pyxcell;
using Xunit;
using Xunit.Abstractions;

namespace PyxcellTests
{
    public class DecoderTests
    {
    
        private readonly ITestOutputHelper output;

        public DecoderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GetLettersFromImage()
        {
           var decoder = new Decoder("EncodedTestImage.png");
           var decodedImage = decoder.Decode();
           
           // ToDo: Make these match the test image
           // Assert.Equal((char) 32, decodedImage.CharacterMappings[0].Char);
           // Assert.Equal(new int[] {1, 1, 0, 0, 1, 0, 0}, decodedImage.CharacterMappings[0].Fill);
           //
           // Assert.Equal((char) 33, decodedImage.CharacterMappings[1].Char);
           // Assert.Equal(new int[] {1, 1, 0, 1, 0, 0, 1}, decodedImage.CharacterMappings[1].Fill);
           //
           // Assert.True(char.IsControl(decodedImage.CharacterMappings[95].Char));
           // Assert.Equal(new int[] {1, 0, 0, 1, 0, 0, 0}, decodedImage.CharacterMappings[95].Fill);
           
           Assert.Equal("You need to install the plugin and activate it for your stream on their website." +
                        "Sometimes, I really like it when my friends buy me food without me asking." +
                        "Don't you think it's great that we can all have a blast on this tiny rock." +
                        "I like cheese most days. But sometimes, I don't.", decodedImage.Text);
           Assert.Equal(8, decodedImage.ColourPalette.Colours.Count);
 
        }
    }
}