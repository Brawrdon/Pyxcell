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
            var red = new CharacterGrid(Rgba32.Red, 'a');
            var blue = new CharacterGrid(Rgba32.Blue, 'b');

            // Act
            var redResult = colourPallete.AddCharacterGrid(red);
            var blueResult = colourPallete.AddCharacterGrid(blue);

            // Assert
            Assert.True(redResult);
            Assert.True(blueResult);
            Assert.Equal(2, colourPallete.Grids.Count);
            Assert.Contains(red, colourPallete.Grids);
            Assert.Contains(blue, colourPallete.Grids);
        }

        [Fact]
        public void AddCharacterGrid_ContainsChar_ReturnFalse()
        {
            // Arrange
            var colourPallete = new ColourPalette();
            var red = new CharacterGrid(Rgba32.Red, 'a');
            var blue = new CharacterGrid(Rgba32.Blue, 'a');

            // Act
            var redResult = colourPallete.AddCharacterGrid(red);
            var blueResult = colourPallete.AddCharacterGrid(blue);

            // Assert
            Assert.True(redResult);
            Assert.False(blueResult);
            Assert.Single(colourPallete.Grids);
            Assert.Contains(red, colourPallete.Grids);
            Assert.DoesNotContain(blue, colourPallete.Grids);
        }

    }
}