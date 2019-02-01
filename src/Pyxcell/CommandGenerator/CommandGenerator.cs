using System;
using System.Collections.Generic;
using System.Text;

namespace Pyxcell
{
    public class CommandGenerator : PyxcellGenerator
    {
        private List<Command> _commands;

        public CommandGenerator()
        {
            _commands = new List<Command>();
        }
        
        public override void Generate(string text)
        {
            // Encode string
            var textToEncode = text + DateTime.Now.ToFileTimeUtc();
            var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(textToEncode));
            
            // Convert to Unicode equivalent
            var encodedCharacters = encoded.ToCharArray();

            // ToDo: Learn about method groups
            var encodedIntegers = Array.ConvertAll(encodedCharacters, Convert.ToInt32);
            
            // Padd to be divisable by 4
            var paddedIntegers = PaddArray(encodedIntegers); 
            
            GenerateCommands(paddedIntegers);
        }
        
        private static int[] PaddArray(int[] characters)
        {

            var mod = characters.Length % 4;
            if (mod == 0)
                return characters;

            var paddedCharacters = new int[characters.Length + mod];
            Array.Copy(characters, paddedCharacters, characters.Length);

            for (var i = 0; i < mod; i++)
                paddedCharacters[characters.Length + i] = characters[i];

            return paddedCharacters;
        }

        private static void GenerateCommands(int[] characters)
        {
            // Take 4 characters
            
        }
    }
}