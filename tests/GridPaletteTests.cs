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
        public void Constructor_NullArgument_AssignsEmptyList()
        {
            // Arrange
            var gridPallete = new GridPalette();

            // Assert
            Assert.NotNull(gridPallete.Grids);
            Assert.Empty(gridPallete.Grids);
        }

        [Fact]
        public void AddCharacterGrid_DoesNotContainChar_ReturnTrueAndAddToGridsList()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var a = new CharacterGrid('a');
            var b = new CharacterGrid('b');

            // Act
            var aResult = gridPallete.AddCharacterGrid(a);
            var bResult = gridPallete.AddCharacterGrid(b);

            // Assert
            Assert.True(aResult);
            Assert.True(bResult);
            Assert.Equal(2, gridPallete.Grids.Count);
            Assert.Contains(a, gridPallete.Grids);
            Assert.Contains(b, gridPallete.Grids);
        }

        [Fact]
        public void AddCharacterGrid_ContainsChar_ReturnFalse()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var redA = new CharacterGrid('a');
            var blueA = new CharacterGrid('a');

            // Act
            var redAResult = gridPallete.AddCharacterGrid(redA);
            var blueAResult = gridPallete.AddCharacterGrid(blueA);

            // Assert
            Assert.True(redAResult);
            Assert.False(blueAResult);
            Assert.Single(gridPallete.Grids);
            Assert.Contains(redA, gridPallete.Grids);
            Assert.DoesNotContain(blueA, gridPallete.Grids);
        }

        [Fact]
        public void AddKeywordGrid_DoesNotContainKeyword_ReturnTrueAndAddToGridsList()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var hello = new KeywordGrids("hello", Rgba32.Red);
            var world = new KeywordGrids("world", Rgba32.Blue);

            // Act
            var helloResult = gridPallete.AddKeywordGrids(hello);
            var worldResult = gridPallete.AddKeywordGrids(world);

            // Assert
            Assert.True(helloResult);
            Assert.True(worldResult);
            Assert.Equal(2, gridPallete.Grids.Count);
            Assert.Contains(hello, gridPallete.Grids);
            Assert.Contains(world, gridPallete.Grids);
        }

        [Fact]
        public void AddKeywordGrid_ContainsKeyword_ReturnFalse()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var redHello = new KeywordGrids("hello", Rgba32.Red);
            var blueHello = new KeywordGrids("hello", Rgba32.Blue);

            // Act
            var redHelloResult = gridPallete.AddKeywordGrids(redHello);
            var blueHelloResult = gridPallete.AddKeywordGrids(blueHello);

            // Assert
            Assert.True(redHelloResult);
            Assert.False(blueHelloResult);
            Assert.Single(gridPallete.Grids);
            Assert.Contains(redHello, gridPallete.Grids);
            Assert.DoesNotContain(blueHello, gridPallete.Grids);
        }

        [Fact]
        public void AddKeywordGrid_ContainsKeyword_ReturnFalse()
        {
            // Arrange
            var gridPallete = new GridPalette();
            var redHello = new KeywordGrids("hello", Rgba32.Red);
            var blueHello = new KeywordGrids("hello", Rgba32.Blue);

            // Act
            var redHelloResult = gridPallete.AddKeywordGrids(redHello);
            var blueHelloResult = gridPallete.AddKeywordGrids(blueHello);

            // Assert
            Assert.True(redHelloResult);
            Assert.False(blueHelloResult);
            Assert.Single(gridPallete.Grids);
            Assert.Contains(redHello, gridPallete.Grids);
            Assert.DoesNotContain(blueHello, gridPallete.Grids);
        }
        
    }
}