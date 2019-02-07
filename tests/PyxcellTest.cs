using System;
using Pyxcell;
using Xunit;

namespace PyxcellTests
{
    public class PyxcellTest
    {
        [Fact]
        public void TestDrawing()
        {
            var pyxcell = new CommandGenerator();
            pyxcell.Generate("œ ¡š i am s{}o cool ///'''±ASCII { ARE PROGRAM with twitch chat");
            pyxcell.Draw("image.png");
            
            pyxcell.Generate("Hello");
            pyxcell.Draw("image2.png");
        }
    }
}