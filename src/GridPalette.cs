using System;
using System.Collections.Generic;
using System.Linq;
using Pyxcell.Grids;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    internal class GridPalette
    {
        public List<Rgba32> Colours { get { return new List<Rgba32>(_colours); } }
        public List<Grid> Grids { get; }

        private List<int[]> _availablePatterns;
        private List<Rgba32> _colours;
        private Random _random;

        public const int StartCharacter = 32; // a
        public const int EndCharacter = 127; // DEL
        private const int PatternLimit = (2^7); // 127

        public GridPalette(List<Rgba32> colours, Dictionary<string, Rgba32> keywords = null)
        {
            if(colours == null || !colours.Any())
                throw new Exception($"{nameof(colours)} is null or empty");

            Grids = new List<Grid>();

            _colours = ValidateColours(colours);
            _availablePatterns = GeneratePatterns();
            _random = new Random();

            AddCharactersToGridsList();

            if (keywords != null && keywords.Any())
                AddKeywordsToGridsList(keywords);
        }

        private List<Rgba32> ValidateColours(List<Rgba32> colours)
        {
            foreach (var colour in colours)
            {     
                if (_colours.Contains(colour))
                    throw new Exception($"The palette already contains the colour {colour}.");
            }

            return colours;
        }

        private void AddCharactersToGridsList()
        {
            for (int i = StartCharacter; i <= EndCharacter; i++)
            {
                var characterGrid = new CharacterGrid((char) i);
                Grids.Add(characterGrid);
            }
 
        }

        private void AddKeywordsToGridsList(Dictionary<string, Rgba32> keywords)
        {            
            foreach (var (keyword, colour) in keywords)
            {
                if (_colours.Contains(colour))
                    throw new Exception($"Colour {colour} is already used in the main colour palette.");
        
                var keywordGrids = new KeywordGrids(keyword, colour);
                var result = Grids.OfType<KeywordGrids>().Any(x => x.Keyword == keywordGrids.Keyword || x.Colour == keywordGrids.Colour);

                if(!result)
                    throw new Exception($"Keyword {keywordGrids.Keyword} already exists or the keyword's colour {keywordGrids.Colour} is used by another keyword.");

                Grids.Add(keywordGrids);
            }
            
        }
    
        /// <summary>
        // We are supporting ASCII characters 32 - 127 giving us a total of 95 characters.
        // These characters are going to be randomly mapped to a shape which is defined by a 
        // 14 x 14 grid with 7 segments inside. The segments will be either on (painted) or off.
        // 127 represents the maximum value that can be represented in a 7-bit binary value;
        // We start with 1 to make sure no grid is empty.
        /// </summary>
        /// <returns></returns>
        private static List<int[]> GeneratePatterns()
        {
            var patterns = new List<int[]>();

            for (var i = 1; i < PatternLimit; i++)
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

        private int[] GetPattern()
        {
            if (_availablePatterns.Count == 0)
                throw new Exception("Grid limit reached.");

            var index = _random.Next(_availablePatterns.Count - 1);
            var pattern = _availablePatterns[index];
            _availablePatterns.RemoveAt(index);
            return pattern;
        }
    }
}