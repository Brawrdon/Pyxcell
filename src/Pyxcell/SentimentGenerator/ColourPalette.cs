using System;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.SentimentGenerator
{
    public class ColourPalette
    {
        public Rgba32 ColourOne { get; }
        public Rgba32 ColourTwo { get; }
        public Rgba32 ColourThree { get; }
        public Rgba32 Positive { get; }
        public Rgba32 Negative { get; }

        private readonly Random _random;

        private ColourPalette(Rgba32 colourOne, Rgba32 colourTwo, Rgba32 colourThree, Rgba32 positive, Rgba32 negative)
        {
            _random = new Random();
            ColourOne = colourOne;
            ColourTwo = colourTwo;
            ColourThree = colourThree;
            Positive = positive;
            Negative = negative;
        }
        
        public static ColourPalette SelectPalette()
        {
            var random = new Random();
            var paletteChoice = random.Next(0, 3);
            
            return paletteChoice switch
            {
                0 => PaletteOne(),
                1 => PaletteTwo(),
                _ => PaletteThree()
            };
        }
        
        public Rgba32 SelectColour()
        {
            var colourChoice = _random.Next(0, 3);
            return colourChoice switch
            {
                0 => ColourOne,
                1 => ColourTwo,
                _ => ColourThree
            };
        }

        private static ColourPalette PaletteOne()
        {
            return new ColourPalette(new Rgba32(250, 249, 243),
                new Rgba32(132, 220, 198),
                new Rgba32(195, 60, 84),
                new Rgba32(35, 32, 32),
                new Rgba32(85, 55, 57));
        }
        
        private static ColourPalette PaletteTwo()
        {
            return new ColourPalette(new Rgba32(242, 129, 35),
                new Rgba32(211, 78, 26),
                new Rgba32(195, 60, 84),
                new Rgba32(56, 114, 108),
                new Rgba32(13, 59, 102));
        }
        
        private static ColourPalette PaletteThree()
        {
            return new ColourPalette(new Rgba32(218, 214, 214),
                new Rgba32(146, 191, 177),
                new Rgba32(244, 172, 69),
                new Rgba32(105, 74, 56),
                new Rgba32(69, 66, 90));
        }

    }
}