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
        public void Encode_EncodeValidData_OutputImage()
        {
            var pyxcellImage = PyxcellConvert.Decode("HelloWorld.png");
        }
    }
}