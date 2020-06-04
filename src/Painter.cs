using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Pyxcell
{
    internal class Painter
    {
        public int CurrentPosition { get; set; }
        public GridPalette GridPalette { get;}
        public Image<Rgba32> Image { get; }

        private Random _random;


        public Painter(Image<Rgba32> image, GridPalette gridPalette) 
        {            
            CurrentPosition = 0;
            GridPalette = gridPalette;
                        
            Image = image;

            _random = new Random();
        }

        public void DrawCharacterGrids()
        {
            var characterGrids = GridPalette.Grids.OfType<CharacterGrid>().ToList();
            foreach (var grid in characterGrids)
            {
                var characterGrid = grid as CharacterGrid;
                var index = _random.Next(GridPalette.Colours.Count);
                Draw(characterGrid.Pattern, GridPalette.Colours[index]);
            }
        }

        public void Draw(int[] pattern, Color colour)
        {
            var startPositionX = (CurrentPosition % 50) * 14;
            var startPositionY = (CurrentPosition / 50) * 14;
            
            for (int i = 0; i < pattern.Length; i++)
            {
                var x = new PointF(startPositionX + i, startPositionY);
                var y = new PointF(x.X + 1, x.Y + 14);

                var line = new RectangularPolygon(x, y);
                Image.Mutate(x => x.Fill(colour, line));
            }

                CurrentPosition++;
        }

        public Color GetRandomColour()
        {
            throw new NotImplementedException();
        }
    }
}

