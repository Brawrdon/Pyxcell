using System;
using Xunit;

namespace PyxcellTests
{
    public class PyxcellTest
    {
        [Fact]
        public void PaddIntArray()
        {
            var pyxcell = new Pyxcell.Pyxcell();
            pyxcell.GenerateViaCommands("hello");
        }
    }
}