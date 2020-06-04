using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Pyxcell
{
    internal class GridPainter
    {
        private int _currentPosition;
        private CharacterGrid _delChar;
        private GridPalette _gridPalette;
        private Image<Rgba32> _image;
        private string _message;
        
        public GridPainter(Image<Rgba32> image, GridPalette gridPalette, string message) 
        {            
            _currentPosition = 0;
            _gridPalette = gridPalette;
            _delChar = gridPalette.Grids.OfType<CharacterGrid>().First(x => x.Character == ((char) 127));
            _image = image; 
            _message = message;           
        }

        public void Paint()
        {
            DrawCharacterGrids();
            DrawColourGrids();
            DrawKeywordColourGrids();
            DrawMessage();
        }

        private void DrawCharacterGrids()
        {
            var characterGrids = _gridPalette.Grids.OfType<CharacterGrid>().ToList();
            foreach (var grid in characterGrids)
            {
                var characterGrid = grid as CharacterGrid;
                Draw(characterGrid.Pattern, _gridPalette.GetRandomColour());
            }
        }

        private void DrawColourGrids()
        {
            var colourDataGrids = _gridPalette.Grids.OfType<ColourDataGrid>().ToList();
            foreach (var grid in colourDataGrids)
            {
                var colourDataGrid = grid as ColourDataGrid;
                Draw(colourDataGrid.Pattern, colourDataGrid.Colour);
            }

            Draw(_delChar.Pattern, _gridPalette.GetRandomColour());
        }

        private void DrawKeywordColourGrids()
        {
            var keywordGrids = _gridPalette.Grids.OfType<KeywordGrids>().ToList();
            foreach (var grid in keywordGrids)
            {
                var keywordGrid = grid as KeywordGrids;
                Draw(_gridPalette.GetRandomPattern(), keywordGrid.Colour);
            }

            Draw(_delChar.Pattern, _gridPalette.GetRandomColour());
        }

        private void DrawMessage()
        {
            var words = GetWordsFromMessage();
            var keywordGrids = _gridPalette.Grids.OfType<KeywordGrids>().Cast<KeywordGrids>();
            foreach (var word in words)
            {
                foreach (var character in word)
                {
                    Color colour;
                    if (IsKeyword(word, keywordGrids))
                        colour = keywordGrids.Single(x => x.Keyword == word).Colour;
                    else
                        colour = _gridPalette.GetRandomColour();
                    
                    Draw(_gridPalette.GetPatternFromCharacter(character), colour);
                } 
            }
        }
            
        private bool IsKeyword(string word, IEnumerable<KeywordGrids> keywordGrids)
        {
             return keywordGrids.Any(x => x.Keyword == word);
        }

        public void Draw(int[] pattern, Color colour)
        {
            var startPositionX = (_currentPosition % 50) * 14;
            var startPositionY = (_currentPosition / 50) * 14;
            
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == 1)
                {
                    var x = new PointF(startPositionX + i, startPositionY);
                    var y = new PointF(x.X + 1, x.Y + 14);

                    var line = new RectangularPolygon(x, y);
                    _image.Mutate(x => x.Fill(colour, line));
                }
                
            }

            _currentPosition++;
        }

        private List<string> GetWordsFromMessage()
        {
            string pattern = @"^(\s+|\d+|\w+|[^\d\s\w])+$";
            var words = new List<string>();

            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(_message))
                throw new Exception("Regex failed.");

            Match match = regex.Match(_message);
            foreach (Capture capture in match.Groups[1].Captures)
                words.Add(capture.Value);
              
            return words;    
        }
    }
}

