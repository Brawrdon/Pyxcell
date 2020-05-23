using System;
using System.Collections.Generic;
using System.Linq;

namespace Pyxcell
{
    public class ColourPalette
    {
        // A list of colours
        public List<GridColour> Colours {get;}

        public ColourPalette(List<GridColour> colours = null)
        {
            Colours = colours ?? new List<GridColour>();
        }

        public bool AddCharacterColour(CharacterColour characterColour)
        {
            var gridColour = Colours.FirstOrDefault(x => x.Rgba32 == characterColour.Rgba32);

            if(gridColour == null)
            {
                Colours.Add(characterColour);
                return true;
            }

            return false;
        }
        
        public bool AddKeywordColour(KeywordColour keywordColour)
        {
            var gridColour = Colours.FirstOrDefault(x => x.Rgba32 == keywordColour.Rgba32) as KeywordColour;

            if(gridColour == null)
            {
                Colours.Add(keywordColour);
                return true;
            }

            if(gridColour.Keyword != keywordColour.Keyword)
            {
                Colours.Add(keywordColour);
                return true;
            }

            return false;
        }
        
    }
}