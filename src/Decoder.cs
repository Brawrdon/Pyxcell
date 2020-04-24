using System.Collections.Generic;
using System.Linq;
using Pyxcell.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using static System.String;

namespace Pyxcell
{
    public class DecodedImage
    {
        
        public  IColourPalette ColourPalette { get; }
        public List<CharacterGrid> CharacterMappings { get;}
        public string Text;
        
        internal DecodedImage()
        {
            ColourPalette = new ColourPalette();
            CharacterMappings = new List<CharacterGrid>();
            Text = Empty;
        }
    }
    
    public class Decoder : IDecoder
    {
        private readonly string _imagePath;
        private readonly DecodedImage _decodedImage;
        private readonly IColourPalette _colourPalette;
        private int _row;
        private int _column;

        public Decoder(string imagePath)
        {
            _imagePath = imagePath;
            _row = 0;
            _column = 0;
            _colourPalette = new ColourPalette();
            _decodedImage = new DecodedImage();
        }

        public DecodedImage Decode()
        {
            using (var image = Image.Load<Rgba32>(_imagePath))
            {
                DecodeLetterMappings(image);
                DecodeColourPalette(image);
                DecodeText(image);
            }

            return _decodedImage;
        }

        private void DecodeText(Image<Rgba32> image)
        {
            var isControlChar = false;
            while (!isControlChar)
            {
                var grid = DecodeCharacterGrid(image);
                var gridCharacter = _decodedImage.CharacterMappings.First(x => x.Fill.SequenceEqual(grid.Fill)).Char;
                grid.Char = gridCharacter;
                
                if (char.IsControl(grid.Char))
                    isControlChar = true;
                else
                    _decodedImage.Text += grid.Char;
            }
        }
        
        private void DecodeColourPalette(Image<Rgba32> image)
        {
            var isControlChar = false;
            while (!isControlChar)
            {
                var grid = DecodeColourGrid(image);
                var controlGrid = _decodedImage.CharacterMappings.First(x => char.IsControl(x.Char));

                if (controlGrid.Fill.SequenceEqual(grid.Fill))
                    isControlChar = true;
                else
                    _colourPalette.AddColour(grid.Colour);
            }
        }

        private void DecodeLetterMappings(Image<Rgba32> image)
        {
            var isControlChar = false;
            var character = (char) 32;
            while (!isControlChar)
            {
                if (char.IsControl(character))
                    isControlChar = true;
                
                var grid = DecodeCharacterGrid(image, character);
                _decodedImage.CharacterMappings.Add(grid);
                character++;

                
            }
        }

        private CharacterGrid DecodeCharacterGrid(Image<Rgba32> image, char character = default)
        {
            var characterGrid = new CharacterGrid(character);
            var column = GetColumn();
            SetRowAndColumn();
            
            for (var y = 0 + _row * 14; y < 14 + _row * 14; y++)
            {
                var pixelRowSpan = image.GetPixelRowSpan(y);

                // Columns take a 14 x 14 grid.
                for (var x = column * 14; x < 14 + column * 14; x++)
                {
                    // Reset x to be between 0 and 14 so we can use it as the index when accessing
                    // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                    // within the bounds of the fill array which has a length of 7.
                    var indexForFill = (x - column * 14) / 2;
                    
                    var tmpColour = new Rgba32(pixelRowSpan[x].PackedValue);
                    characterGrid.Fill[indexForFill] = tmpColour.Equals(Rgba32.Transparent) || tmpColour.Equals(new Rgba32(0, 0, 0, 0)) ? 0 : 1;
                }
            }
            
            _column++;
            return characterGrid;
        }
        
        private ColourGrid DecodeColourGrid(Image<Rgba32> image)
        {
            var colourMapping = new ColourGrid(Rgba32.Transparent);
            var column = GetColumn();
            SetRowAndColumn();
            
            for (var y = 0 + _row * 14; y < 14 + _row * 14; y++)
            {
                var pixelRowSpan = image.GetPixelRowSpan(y);

                // Columns take a 14 x 14 grid.
                for (var x = column * 14; x < 14 + column * 14; x++)
                {
                    // Reset x to be between 0 and 14 so we can use it as the index when accessing
                    // the current letter's Fill array. We divide by 2 as it'll produce a whole number
                    // within the bounds of the fill array which has a length of 7.
                    var indexForFill = (x - column * 14) / 2;
                    
                    var tmpColour = new Rgba32(pixelRowSpan[x].PackedValue);
                    colourMapping.Fill[indexForFill] = tmpColour.Equals(Rgba32.Transparent) || tmpColour.Equals(new Rgba32(0, 0, 0, 0)) ? 0 : 1;
                   
                    if (!tmpColour.Equals(Rgba32.Transparent) || !tmpColour.Equals(new Rgba32(0, 0, 0, 0)))
                        colourMapping.Colour = tmpColour;
                }
            }
            
            _column++;
            return colourMapping;
        }
        

        private void SetRowAndColumn()
        {
            if (_column == 0 || _column % 50 != 0) 
                return;
           
            _row++;
            _column = 0;
        }
        
        private int GetColumn()
        {
            return _column % 50;
        }

    }
}