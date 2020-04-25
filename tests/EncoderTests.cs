using System.Collections.Generic;
using Pyxcell;
using Pyxcell.Common;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace PyxcellTests
{
    public class EncoderTests
    {
    
        private readonly ITestOutputHelper output;

        public EncoderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        //ToDo: Write more tests!
        [Fact]
        public void GenerateEncodedExample()
        {
            var colours = new List<Rgba32>
            {
                Rgba32.Aqua,
                Rgba32.Chocolate,
                Rgba32.Fuchsia,
                Rgba32.Khaki,
                Rgba32.Blue,
                Rgba32.Salmon,
                Rgba32.Coral,
                Rgba32.DarkGreen,
                Rgba32.Lime,
                Rgba32.Crimson
            };
            
            var colourPalette = new ColourPalette(colours);
            
            var encoder = new Encoder(colourPalette, new List<Keyword>
            {
                new Keyword { Word = "activate", Colour = Rgba32.Black},
                new Keyword { Word = "cheese", Colour = Rgba32.Yellow},
                new Keyword { Word = "rock", Colour = Rgba32.Silver},
            });
            
            output.WriteLine(encoder.Generate("You need to install the plugin and activate it for your stream on their website." +
                                        "Sometimes, I really like it when my friends buy me food without me asking." +
                                        "Don't you think it's great that we can all have a blast on this tiny rock." +
                                        "I like cheese most days. But sometimes, I don't."));
        }
    }
}