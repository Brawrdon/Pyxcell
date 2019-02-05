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
            pyxcell.Generate("hello");
        }
    }
}