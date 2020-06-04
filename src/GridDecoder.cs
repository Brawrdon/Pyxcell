using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Pyxcell
{
    
    class GridDecoder
    {
        private int _currentPosition;
        private CharacterGrid _delChar;
        public List<Grid> Grids { get; }
        private Image<Rgba32> _image;

        public GridDecoder(Image<Rgba32> image)
        {
            _currentPosition = 0;
            _image = image; 
        } 

        public List<CharacterGrid> DecodeCharacterGrids()
        {
            var characterGrids = new List<CharacterGrid>();
            for (int i = Constraints.StartCharacter; i <= Constraints.EndCharacter; i++)
            {
                var characterGrid = new CharacterGrid((char) i) { Pattern = DecodeGridPattern()};
                characterGrids.Add(characterGrid);
            }

            return characterGrids;

        }

        private int[] DecodeGridPattern()
        {
            var startPositionX = (_currentPosition % 50) * 14;
            var startPositionY = (_currentPosition / 50) * 14;
            
            var pixelRow = _image.GetPixelRowSpan(startPositionY);

            var pattern = new int[14];
            for (int i = 0; i < pattern.Length; i++)
            {
                var columnFill = pixelRow[startPositionX + i].ToHex();
                pattern[i] = columnFill == Color.Transparent.ToHex() ? 0 : 1;
            }

            _currentPosition++;

            return pattern;
        }

    }
}