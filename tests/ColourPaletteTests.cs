using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace Pyxcell
{
    public class ColourPaletteTests
    {
        private readonly ITestOutputHelper output;

        public ColourPaletteTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Constructor_NullArgument_AssignsEmptyList()
        {
            // Arrange
            var colourPallete = new ColourPalette();

            // Assert
            Assert.NotNull(colourPallete.Grids);
            Assert.Empty(colourPallete.Grids);
        }

        [Fact]
        public void AddCharacterGrid_DoesNotContainChar_ReturnTrueAndAddToGridsList()
        {
            // Arrange
            var colourPallete = new ColourPalette();
            var a = new CharacterGrid(Rgba32.Red, 'a');
            var b = new CharacterGrid(Rgba32.Blue, 'b');

            // Act
            var aResult = colourPallete.AddCharacterGrid(a);
            var bResult = colourPallete.AddCharacterGrid(b);

            // Assert
            Assert.True(aResult);
            Assert.True(bResult);
            Assert.Equal(2, colourPallete.Grids.Count);
            Assert.Contains(a, colourPallete.Grids);
            Assert.Contains(b, colourPallete.Grids);
        }

        [Fact]
        public void AddCharacterGrid_ContainsChar_ReturnFalse()
        {
            // Arrange
            var colourPallete = new ColourPalette();
            var redA = new CharacterGrid(Rgba32.Red, 'a');
            var blueA = new CharacterGrid(Rgba32.Blue, 'a');

            // Act
            var redAResult = colourPallete.AddCharacterGrid(redA);
            var blueAResult = colourPallete.AddCharacterGrid(blueA);

            // Assert
            Assert.True(redAResult);
            Assert.False(blueAResult);
            Assert.Single(colourPallete.Grids);
            Assert.Contains(redA, colourPallete.Grids);
            Assert.DoesNotContain(blueA, colourPallete.Grids);
        }

        [Fact]
        public void AddKeywordGrid_DoesNotContainKeyword_ReturnTrueAndAddToGridsList()
        {
            // Arrange
            var colourPallete = new ColourPalette();
            var hello = new KeywordGrids(Rgba32.Red, "hello");
            var world = new KeywordGrids(Rgba32.Blue, "world");

            // Act
            var helloResult = colourPallete.AddKeywordGrids(hello);
            var worldResult = colourPallete.AddKeywordGrids(world);

            // Assert
            Assert.True(helloResult);
            Assert.True(worldResult);
            Assert.Equal(2, colourPallete.Grids.Count);
            Assert.Contains(hello, colourPallete.Grids);
            Assert.Contains(world, colourPallete.Grids);
        }

        [Fact]
        public void AddKeywordGrid_ContainsKeyword_ReturnFalse()
        {
            // Arrange
            var colourPallete = new ColourPalette();
            var redHello = new KeywordGrids(Rgba32.Red, "hello");
            var blueHello = new KeywordGrids(Rgba32.Blue, "hello");

            // Act
            var redHelloResult = colourPallete.AddKeywordGrids(redHello);
            var blueHelloResult = colourPallete.AddKeywordGrids(blueHello);

            // Assert
            Assert.True(redHelloResult);
            Assert.False(blueHelloResult);
            Assert.Single(colourPallete.Grids);
            Assert.Contains(redHello, colourPallete.Grids);
            Assert.DoesNotContain(blueHello, colourPallete.Grids);
        }

        [Fact]
        public void AddGrid_ColourExists_ReturnFalse()
        {
            // Arrage
            var colourPallete = new ColourPalette();
            var a = new CharacterGrid(Rgba32.Red, 'a');
            var b = new CharacterGrid(Rgba32.Red, 'b');

            // Act
            var aResult = colourPallete.AddCharacterGrid(a);
            var bResult = colourPallete.AddCharacterGrid(b);

            // Assert
            Assert.True(aResult);
            Assert.False(bResult);
            Assert.Single(colourPallete.Grids);
            Assert.Contains(a, colourPallete.Grids);
            Assert.DoesNotContain(b, colourPallete.Grids);
        }
        
    }
}