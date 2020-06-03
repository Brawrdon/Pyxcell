using System;
using Pyxcell.Grids;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace Pyxcell
{
    public class GridPaletteTests
    {
        private readonly ITestOutputHelper output;

        public GridPaletteTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Constructor()
        {
            // Arrange
            var gridPallete = new GridPalette();

            // Assert
            Assert.NotNull(gridPallete.Grids);
            Assert.Empty(gridPallete.Grids);
        }

        [Fact]
        public void AddColour_ColourNotInPalette_AddToPalette()
        {
            // Arrange
            var gridPallete = new GridPalette();

            // Act
            gridPallete.AddColour(Rgba32.Red);
            gridPallete.AddColour(Rgba32.Blue);

            // Assert
            Assert.Equal(2, gridPallete.Colours.Count);
            Assert.Contains(Rgba32.Red, gridPallete.Colours);
            Assert.Contains(Rgba32.Blue, gridPallete.Colours);
        }


        [Fact]
        public void AddColour_ColourExistsInKeyword_ThrowException()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var hello = new KeywordGrids("hello", Rgba32.Red);

            // Act
            gridPallete.AddKeywordGrids(hello);

            // Assert
            Assert.Throws<Exception>(() => gridPallete.AddColour(Rgba32.Red));
            Assert.DoesNotContain(Rgba32.Red, gridPallete.Colours);
        }

        [Fact]
        public void AddCharacterGrid_DoesNotContainChar_AddToGridsList()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var a = new CharacterGrid('a');
            var b = new CharacterGrid('b');

            // Act
            gridPallete.AddCharacterGrid(a);
            gridPallete.AddCharacterGrid(b);

            // Assert
            Assert.Equal(2, gridPallete.Grids.Count);
            Assert.Contains(a, gridPallete.Grids);
            Assert.Contains(b, gridPallete.Grids);
        }

        [Fact]
        public void AddCharacterGrid_ContainsChar_ThrowException()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var a1 = new CharacterGrid('a');
            var a2 = new CharacterGrid('a');

            // Act
            gridPallete.AddCharacterGrid(a1);

            // Assert
            Assert.Throws<Exception>(() => gridPallete.AddCharacterGrid(a2));
            Assert.Single(gridPallete.Grids);
            Assert.Contains(a1, gridPallete.Grids);
            Assert.DoesNotContain(a2, gridPallete.Grids);
        }

        [Fact]
        public void AddKeywordGrid_DoesNotContainKeyword_AddToGridsList()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var hello = new KeywordGrids("hello", Rgba32.Red);
            var world = new KeywordGrids("world", Rgba32.Blue);

            // Act
            gridPallete.AddKeywordGrids(hello);
            gridPallete.AddKeywordGrids(world);

            // Assert
            Assert.Equal(2, gridPallete.Grids.Count);
            Assert.Contains(hello, gridPallete.Grids);
            Assert.Contains(world, gridPallete.Grids);
        }

        [Fact]
        public void AddKeywordGrid_ContainsKeyword_ThrowException()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var redHello = new KeywordGrids("hello", Rgba32.Red);
            var blueHello = new KeywordGrids("hello", Rgba32.Blue);

            // Act
            gridPallete.AddKeywordGrids(redHello);
        
            // Assert
            Assert.Throws<Exception>(() => gridPallete.AddKeywordGrids(blueHello));
            Assert.Single(gridPallete.Grids);
            Assert.Contains(redHello, gridPallete.Grids);
            Assert.DoesNotContain(blueHello, gridPallete.Grids);
        }

        [Fact]
        public void AddKeywordGrid_ColourExistsInPalette_ThrowException()
        {
            // Arrange
            var gridPallete = new GridPalette();
            gridPallete.AddColour(Rgba32.Red);

            var hello = new KeywordGrids("hello", Rgba32.Red);

            // Assert
            Assert.Throws<Exception>(() => gridPallete.AddKeywordGrids(hello));
            Assert.DoesNotContain(hello, gridPallete.Grids);
        }

        
    }
}