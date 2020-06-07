using System.Collections.Generic;
using SixLabors.ImageSharp;

namespace Pyxcell
{
    public class PyxcellImage
    {
        private IEnumerable<Color> _colours;
        private IDictionary<char, int[]>  _characterPatternMap;
        private IDictionary<string, Color> _keywords;

        public string Base64 { get; }
        public List<Color> Colours
        {
             get => new List<Color>(_colours); 
             internal set => _colours = value; 
        }
        public Dictionary<char, int[]> CharacterPatternMap
        { 
            get => new Dictionary<char, int[]>(_characterPatternMap);
            internal set => _characterPatternMap = value; 
        }
        public Dictionary<string, Color> Keywords
        { 
            get => new Dictionary<string, Color>(_keywords);
            internal set => _keywords = value; 
        }
        public string Message { get; internal set; }

        internal PyxcellImage(string base64)
        {
            Base64 = base64;
            _colours = new List<Color>();
            _characterPatternMap = new Dictionary<char, int[]>();
            _keywords = new Dictionary<string, Color>();
            Message = string.Empty;
        }
    } 
}

