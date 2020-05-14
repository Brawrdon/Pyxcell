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
        
        [Fact]
        public void GenerateEncodedExample2()
        {
            var colours = new List<Rgba32>
            {
                new Rgba32(255,250,205),
                new Rgba32(250,250,210),
                new Rgba32(75,0,130),
                new Rgba32(224,255,255),
                new Rgba32(0,255,255),
                new Rgba32(72,209,204),
                new Rgba32(0,206,209),
                new Rgba32(32,178,170),
                new Rgba32(95,158,160),
                new Rgba32(0,139,139),
                new Rgba32(0,128,128)
            };
            
            var colourPalette = new ColourPalette(colours);
            
            var encoder = new Encoder(colourPalette, new List<Keyword>
            {
                new Keyword { Word = "plugin", Colour = new Rgba32(173, 216, 230, 255)},
                new Keyword { Word = "message", Colour = new Rgba32(173, 216, 230, 255)},
                new Keyword { Word = "stream", Colour = new Rgba32(173, 216, 230, 255)},
                new Keyword { Word = "website", Colour = new Rgba32(250, 128, 114, 255)},
                new Keyword { Word = "food", Colour = new Rgba32(173, 216, 230, 255)},
                new Keyword { Word = "friends", Colour = new Rgba32(173, 216, 230, 255)},
                new Keyword { Word = "cheese", Colour = new Rgba32(0, 255, 0, 255)},
                new Keyword { Word = "rock", Colour = new Rgba32(173, 216, 230, 255)},
                new Keyword { Word = "blast", Colour = new Rgba32(0, 255, 0, 255)},
                
            });
            
            output.WriteLine(encoder.Generate("You need to install the plugin and activate it for your stream on their website." +
                                              "Sometimes, I really like it when my friends buy me food without me asking." +
                                              "Don't you think it's great that we can all have a blast on this tiny rock." +
                                              "I like cheese most days. But sometimes, I don't."));
        }
    }
}