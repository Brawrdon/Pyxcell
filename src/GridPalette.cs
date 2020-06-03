using System;
using System.Collections.Generic;
using System.Linq;

namespace Pyxcell
{
    public class GridPalette
    {
        public List<Grid> Grids { get; }

        public GridPalette(List<Grid> grids = null)
        {
            Grids = grids ?? new List<Grid>();
        }

        public bool AddCharacterGrid(CharacterGrid characterGrid)
        {
            return AddGrid(characterGrid, () => Grids.OfType<CharacterGrid>().Any(x => x.Character == characterGrid.Character));
        }

        public bool AddKeywordGrids(KeywordGrids keywordGrids)
        {            
            return AddGrid(keywordGrids, () => Grids.OfType<KeywordGrids>().Any(x => x.Keyword == keywordGrids.Keyword));
        }
        
        private bool AddGrid(Grid grid, Func<bool> FindChar) 
        {
            // Check to see if the character exists
            var valueExists = FindChar();
            
            // Check to see if the colour exists
            var colourExists = Grids.Any(x => x.Rgba32 == grid.Rgba32);

            if(!valueExists && !colourExists)
            {
                Grids.Add(grid);
                return true;
            }

            return false;
        }
        
    }
}