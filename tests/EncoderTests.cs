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
    }
}