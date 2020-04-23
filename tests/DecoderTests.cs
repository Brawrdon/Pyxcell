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
           
           Assert.Equal((char) 32, decodedImage.Letters[0].Char);
           Assert.Equal(new int[] {0, 0, 0, 1, 0, 1, 1}, decodedImage.Letters[0].Fill);
           
           Assert.Equal((char) 33, decodedImage.Letters[1].Char);
           Assert.Equal(new int[] {0, 0, 1, 0, 1, 1, 1}, decodedImage.Letters[1].Fill);
        }
    }
}