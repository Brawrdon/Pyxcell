// using System;
// using System.Collections.Generic;
// using System.IO;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.Formats.Jpeg;
// using SixLabors.ImageSharp.PixelFormats;
//
// namespace Pyxcell
// {
//     public class CommandGenerator : IPyxcellGenerator
//     {
//         private readonly List<Command> _commands;
//
//         public CommandGenerator()
//         {
//             _commands = new List<Command>();
//         }
//
//         public void Generate(string message)
//         {
//             var paddedMessage = CreateMessageToEncode(message);
//             
//             for (var i = 0; i < paddedMessage.Length; i++)
//             {
//                 _commands.Add(new Command(paddedMessage[i], i));
//             }
//         }
//
//         private static string CreateMessageToEncode(string originalMessage)
//         {
//             if (originalMessage.Length > 500)
//                 return originalMessage.Substring(0, 500);
//
//             var paddedMessage = originalMessage;
//             while (paddedMessage.Length < 500)
//             {
//                 paddedMessage += DateTime.Now.ToString();
//                 paddedMessage += originalMessage;
//             }
//             
//             if (paddedMessage.Length > 500)
//                 paddedMessage = paddedMessage.Substring(0, 500);
//
//             return paddedMessage;
//         }
//
//         public string DrawToBase64()
//         {
//             using (var image = new Image<Rgba32>(500, 250))
//             {
//                 foreach (var command in _commands)
//                     command.Execute(image);
//
//                 using (var outputStream = new MemoryStream())
//                 {
//                     image.SaveAsPng(outputStream);
//                     var bytes = outputStream.ToArray();
//                     return Convert.ToBase64String(bytes);
//                 }            
//             }
//         }
//     }
// }