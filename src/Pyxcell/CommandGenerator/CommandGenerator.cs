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

        public override void Generate(string message)
        {
            var paddedMessage = CreateMessageToEncode(message);
            
            for (var i = 0; i < paddedMessage.Length; i++)
            {
                _commands.Add(new Command(paddedMessage[i], i));

            }
        }

        private static string CreateMessageToEncode(string originalMessage)
        {
            if (originalMessage.Length > 500)
                return originalMessage.Substring(0, 500);

            var paddedMessage = originalMessage;
            while (paddedMessage.Length < 500)
            {
                paddedMessage += DateTime.Now.ToString();
                paddedMessage += originalMessage;
            }
            
            if (paddedMessage.Length > 500)
                paddedMessage = paddedMessage.Substring(0, 500);

            return paddedMessage;
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