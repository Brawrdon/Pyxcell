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
            Console.WriteLine(pyxcell.DrawToBase64());
            
            pyxcell.Generate("I am brandon");
            Console.WriteLine(pyxcell.DrawToBase64());
        }
    }
}