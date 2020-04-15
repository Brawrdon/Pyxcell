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
        }
    }
}