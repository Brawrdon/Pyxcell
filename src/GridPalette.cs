using System;
using System.Collections.Generic;
using System.Linq;
using Pyxcell.Grids;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    public class GridPalette
    {
        public List<Rgba32> Colours { get { return new List<Rgba32>(_colours); } }
        public List<Grid> Grids { get; }

        private List<int[]> _availablePatterns;
        private List<Rgba32> _colours;

        public GridPalette()
        {
            Grids = new List<Grid>();

            _colours =  new List<Rgba32>();
            _availablePatterns = GenerateGridPatterns();
        }

        public void AddColour(Rgba32 colour)
        {
            if (_colours.Contains(colour))
                throw new Exception($"The palette already contains the colour {colour}.");

            if (Grids.OfType<KeywordGrids>().Any(x => x.Colour == colour))
                throw new Exception($"A keyword already uses the colour {colour}.");

            _colours.Add(colour);
        }

        public void AddCharacterGrid(CharacterGrid characterGrid)
        {
            var result = AddGrid(characterGrid, () => Grids.OfType<CharacterGrid>().Any(x => x.Character == characterGrid.Character));

            if(!result)
                throw new Exception($"Char {characterGrid.Character} already exists.");
        }

        public void AddKeywordGrids(KeywordGrids keywordGrids)
        {            
            var result = AddGrid(keywordGrids, () => Grids.OfType<KeywordGrids>().Any(x => x.Keyword == keywordGrids.Keyword) || _colours.Contains(keywordGrids.Colour));

            if(!result)
                throw new Exception($"Keyword {keywordGrids.Keyword} already exists or the keyword's colour {keywordGrids.Colour} is already in the palette.");
        }
        
        private bool AddGrid(Grid grid, Func<bool> FindGrid) 
        {
            var gridExists = FindGrid();
                       
            if(!gridExists)
            {
                Grids.Add(grid);
                return true;
            }

            return false;
        }
        
        /// <summary>
        // We are supporting ASCII characters 32 - 127 giving us a total of 95 characters.
        // These characters are going to be randomly mapped to a shape which is defined by a 
        // 14 x 14 grid with 7 segments inside. The segments will be either on (painted) or off.
        // 127 represents the maximum value that can be represented in a 7-bit binary value;
        // We start with 1 to make sure no grid is empty.
        /// </summary>
        /// <returns></returns>
        private static List<int[]> GenerateGridPatterns()
        {
            const int MaxCharacters = 127;
            var patterns = new List<int[]>();

            for (var i = 1; i <= MaxCharacters; i++)
            {
                var binary = Convert.ToString(i, 2).PadLeft(7, '0');
                var binaryAsCharArray = binary.ToCharArray();
                var patternSize = binaryAsCharArray.Length * 2;
                var pattern = new int[14];
                
                for (int j = 0; j < patternSize; j++)
                    pattern[j] = int.Parse(binaryAsCharArray[j / 2].ToString());

                patterns.Add(pattern);
            }
            return patterns;
        } 
    }
}