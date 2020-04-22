using System.Collections.Generic;
using Pyxcell.Common;
using Pyxcell.Encoder;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace PyxcellTests
{
    public class PyxcellTest
    {
    
        private readonly ITestOutputHelper output;

        public PyxcellTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GetSentiment()
        {
            var colours = new List<Rgba32>
            {
                new Rgba32(10, 10, 200),
                new Rgba32(234, 232, 1),
                new Rgba32(124, 124, 23),
                new Rgba32(211, 111, 30),
                new Rgba32(100, 50, 35),
                new Rgba32(100, 120, 200),
                new Rgba32(100, 200, 200),
                new Rgba32(100, 10, 10),
            };
            
            var colourPalette = new ColourPalette(colours);
            
            var encoder = new Encoder(colourPalette);
            
            output.WriteLine(encoder.Generate("You need to install the plugin and activate it for your stream on their website." +
                                        "Sometimes, I really like it when my friends buy me food without me asking." +
                                        "Don't you think it's great that we can all have a blast on this tiny rock." +
                                        "I like cheese most days. But sometimes, I don't."));
        }
    }
}