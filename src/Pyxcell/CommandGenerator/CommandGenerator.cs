using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    public class CommandGenerator : PyxcellGenerator
    {
        private readonly List<Command> _commands;

        public CommandGenerator()
        {
            _commands = new List<Command>();
        }

        public override void Generate(string text)
        {
            var textToEncode = text + Convert.ToBase64String(Encoding.ASCII.GetBytes(DateTime.Now.ToFileTimeUtc().ToString()));
            textToEncode = textToEncode.Insert(textToEncode.Length / 2, "I AM @BRAWRDONBOT©! Take MY ASCII!ƒ&#1¾	25;‰Ž");
            var encoded = Encoding.ASCII.GetBytes(textToEncode);

            // ToDo: Learn about method groups
            var encodedIntegers = Array.ConvertAll(encoded, Convert.ToInt32);

            var paddedIntegers = PaddArray(encodedIntegers);

            GenerateCommands(paddedIntegers);
        }

        private static int[] PaddArray(int[] characters)
        {
            // Double check this, modulus of 9 is 1, thanks Akerri
            var mod = characters.Length % 3;
            if (mod == 0)
                return characters;

            var padAmount = 3 - mod;

            var paddedCharacters = new int[characters.Length + padAmount];
            Array.Copy(characters, paddedCharacters, characters.Length);

            for (var i = 0; i < padAmount; i++)
                paddedCharacters[characters.Length + i] = characters[i];

            return paddedCharacters;
        }

        private void GenerateCommands(int[] characters)
        {
            for (var i = 0; i < characters.Length; i += 3)
            {
                var coordinates = characters[i];
                var direction = characters[i + 1];
                var distance = characters[i + 2];
                _commands.Add(new Command(coordinates, direction, distance));
            }
        }

        public override void Draw(string fileName)
        {
            using (var image = new Image<Rgba32>(500, 250))
            {
                foreach (var command in _commands)
                    command.Execute(image);
                
                image.Save(fileName);
            }
        }
    }
}