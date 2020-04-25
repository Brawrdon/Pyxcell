using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell.Common
{
    public class ColourPalette : IColourPalette
    {
        private readonly Random _random;

        public ColourPalette(List<Rgba32> colours = null)
        {
            _random = new Random();
            Colours = colours ?? new List<Rgba32>();
        }

        public List<Rgba32> Colours { get; }

        public void AddColour(Rgba32 colour)
        {
            if (!Colours.Contains(colour))
                Colours.Add(colour);
        }

        public void AddColours(IEnumerable<Rgba32> colours)
        {
            foreach (var colour in colours)
            {
                if (!Colours.Contains(colour))
                    Colours.Add(colour);
            }
        }

        public Rgba32 SelectRandomColour()
        {
            var colourChoice = _random.Next(0, Colours.Count);
            return Colours[colourChoice];
        }
    }
}