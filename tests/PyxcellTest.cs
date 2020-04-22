using System;
using Pyxcell;
using Pyxcell.SentimentGenerator;
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
        // [Fact]
        // public void TestDrawing()
        // {
        //     var pyxcell = new CommandGenerator();
        //     pyxcell.Generate("œ ¡š i am s{}o cool ///'''±ASCII { ARE PROGRAM with twitch chat");
        //     Console.WriteLine(pyxcell.DrawToBase64());
        //     
        //     pyxcell.Generate("I am brandon");
        //     Console.WriteLine(pyxcell.DrawToBase64());
        // }

        [Fact]
        public void GetSentiment()
        {
            var sentimentGenerator = new SentimentGenerator(); 
            output.WriteLine(sentimentGenerator.Generate("You need to install the plugin and activate it for your stream on their website." +
                                        "Sometimes, I really like it when my friends buy me food without me asking." +
                                        "Don't you think it's great that we can all have a blast on this tiny rock." +
                                        "I like cheese most days. But sometimes, I don't."));
        }
    }
}