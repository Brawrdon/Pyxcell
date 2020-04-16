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

        private ColourPalette(Rgba32 colourOne, Rgba32 colourTwo, Rgba32 colourThree, Rgba32 positive, Rgba32 negative)
        {
            ColourOne = colourOne;
            ColourTwo = colourTwo;
            ColourThree = colourThree;

            Positive = positive;
            Negative = negative;
        }

        public static ColourPalette PaletteOne()
        {
            return new ColourPalette(new Rgba32(250, 249, 243),
                new Rgba32(132, 220, 198),
                new Rgba32(195, 60, 84),
                new Rgba32(35, 32, 32),
                new Rgba32(85, 55, 57));
        }
        
        public static ColourPalette PaletteTwo()
        {
            return new ColourPalette(new Rgba32(242, 129, 35),
                new Rgba32(211, 78, 26),
                new Rgba32(195, 60, 84),
                new Rgba32(56, 114, 108),
                new Rgba32(13, 59, 102));
        }
    }
}