using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;

namespace Pyxcell
{
    class EncodingData
    {
        public List<Color> Colours { get { return new List<Color>(_colours); } }
        public List<Grid> Grids { get; }

        private List<int[]> _availablePatterns;
        private List<Color> _colours;
        private Random _random;

        public EncodingData(List<Color> colours, Dictionary<string, Color> keywords = null)
        {
            if(colours == null || !colours.Any())
                throw new ArgumentException($"{nameof(colours)} is null or empty", nameof(colours));

            Grids = new List<Grid>();

            _colours = colours.Distinct().ToList();
            _availablePatterns = GeneratePatterns();
            _random = new Random();

            AddCharactersToGridsList();
            AddColoursToGridList();

            if (keywords != null && keywords.Any())
                AddKeywordsToGridsList(keywords);
        }

        public int[] GetPatternFromCharacter(char character)
        {
            return Grids.OfType<CharacterGrid>().Single(x => x.Character == character).Pattern;
        }

        public int[] GetRandomPattern()
        {
            if (_availablePatterns.Count == 0)
                throw new Exception("Pattern limit reached.");

            var index = _random.Next(_availablePatterns.Count - 1);
            var pattern = _availablePatterns[index];
            _availablePatterns.RemoveAt(index);
            return pattern;
        }

        public Color GetRandomColour()
        {
            var index = _random.Next(Colours.Count);
            return Colours[index];
        }

        private void AddCharactersToGridsList()
        {
            for (int i = Constraints.StartCharacter; i <= Constraints.EndCharacter; i++)
            {
                var characterGrid = new CharacterGrid((char) i);
                characterGrid.Pattern = GetRandomPattern();
                Grids.Add(characterGrid);
            }
 
        }

        private void AddKeywordsToGridsList(Dictionary<string, Color> keywords)
        {            
            foreach (var (keyword, colour) in keywords)
            {
                if (_colours.Contains(colour))
                    throw new ArgumentException($"Colour {colour} is already used in the main colour palette.", nameof(keywords));
        
                var keywordGrids = new KeywordGrids(keyword, colour);
                Grids.Add(keywordGrids);
            }
            
        }

        private void AddColoursToGridList()
        {
            foreach (var colour in _colours)
            {
                var colourDataGrid = new ColourDataGrid(colour);
                colourDataGrid.Pattern = GetRandomPattern();
                Grids.Add(colourDataGrid);
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

            for (var i = 1; i < Constraints.PatternLimit; i++)
            {
                var binary = Convert.ToString(i, 2).PadLeft(Constraints.GridSize / 2, '0');
                var binaryAsCharArray = binary.ToCharArray();
                var pattern = new int[Constraints.GridSize];
                
                for (int j = 0; j < Constraints.GridSize; j++)
                    pattern[j] = int.Parse(binaryAsCharArray[j / 2].ToString());

                patterns.Add(pattern);
            }
            return patterns;
        } 
    }  
}