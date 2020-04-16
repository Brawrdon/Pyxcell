// using System;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.ColorSpaces;
// using SixLabors.ImageSharp.PixelFormats;
// using SixLabors.ImageSharp.Processing;
// using SixLabors.Primitives;
//
// namespace Pyxcell
// {
//     public class Command
//     {
//         private readonly Direction _direction;
//         private readonly int _distance;
//         private readonly Rgb _colour;
//         private Rectangle _rectangle;
//         
//         internal Command(char character, int characterIndex)
//         {
//             var characterUnicodeInt = Convert.ToInt32(character);
//
//             var x = FindOffsetReverse(characterIndex, characterUnicodeInt, 500);
//             var y = FindOffset(characterIndex, characterUnicodeInt, 250);
//             _direction = GetDirection(characterUnicodeInt);
//             _distance = GetDistance(characterIndex, characterUnicodeInt);
//             _colour = GetColour(characterIndex, characterUnicodeInt);
//             _rectangle = new Rectangle(x, y, 5, 5);        
//
//         }
//         
//         public void Execute(Image<Rgba32> image)
//         {
//             if (_distance == 0)
//                 return;
//             
//             image.Mutate(x => x.Fill(_colour, _rectangle));
//             
//             UpdateCoordinate(image);
//             
//         }
//
//         
//
//         /// <summary>
//         /// Applies an offset to the original index, wrapping around the specified limit.
//         /// The offset is wrapped back to 0 when the value reaches the limit.
//         /// The value must be within 0 to (limit - 1).
//         /// </summary>
//         /// <param name="original"></param>
//         /// <param name="offset"></param>
//         /// <param name="limit"></param>
//         /// <returns></returns>
//         private static int FindOffset(int original, int offset, int limit)
//         {
//             if (original < 0)
//                 throw new ArgumentException();
//
//             while (original >= limit)
//                 original -= limit;
//             
//             var offsettedValue = original + offset;
//
//             if (offsettedValue < limit)
//                 return offsettedValue;
//            
//             do
//             {
//                 var wrapDifference = offsettedValue - limit;
//                 offsettedValue = 0 + wrapDifference;
//
//             } while (offsettedValue >= limit);
//
//             return offsettedValue;
//         }
//         
//         /// <summary>
//         /// Applies a negative offset to the original value, wrapping around the specified limit.
//         /// The offset is wrapped back to (limit - 1) when the value reaches the limit.
//         /// The value is mapped to its opposite.
//         /// The value must be within 0 to (limit - 1).
//         /// </summary>
//         /// <param name="original"></param>
//         /// <param name="offset"></param>
//         /// <param name="limit"></param>
//         /// <returns></returns>
//         private static int FindOffsetReverse(int original, int offset, int limit)
//         {
//             // ToDo: Fix bug with wrapping around minus values
//             if (original < 0)
//                 throw new ArgumentException();
//
//             while (original >= limit)
//                 original -= limit;
//             
//             original = limit - 1 - original;
//             var offsettedValue = original + offset;
//
//             if (offsettedValue < limit)
//                 return offsettedValue;
//            
//             do
//             {
//                 var wrapDifference = offsettedValue - limit;
//                 offsettedValue = limit - 1 - wrapDifference;
//
//             } while (offsettedValue >= limit);
//
//             return offsettedValue;
//         }
//
//         private static Direction GetDirection(int value)
//         {
//             var mod = value % 4;
//
//             switch (mod)
//             {
//                 case 0:
//                     return Direction.Up;
//                 case 1:
//                     return Direction.Right;
//                 case 2:
//                     return Direction.Down;
//                 case 3:
//                     return Direction.Left;
//                 default:
//                     return Direction.Up;
//             }
//         }
//         
//         private int GetDistance(int index, int offset)
//         {
//             var limit = 500;
//             if (_direction == Direction.Down || _direction == Direction.Up)
//                 limit = 250;
//             
//             var distance = FindOffset(index, offset, limit);
//
//             if (distance > limit / 2)
//                 distance = 0;
//
//             return distance;
//         }
//
//         private Rgba32 GetColour(int index, int offset)
//         {
//             var colourIndex = FindOffset(index, offset, ColorConstants.WebSafeColors.Length);
//
//             return ColorConstants.WebSafeColors[colourIndex];
//         }
//       
//         private void UpdateCoordinate(Image<Rgba32> image)
//         {
//             var moveDistance = 5;
//
//             if (_direction == Direction.Up || _direction == Direction.Left)
//                 moveDistance *= -1;
//
//             switch (_direction)
//             {
//                 case Direction.Left:
//                 case Direction.Right:
//                     UpdateXCoordinate(image, moveDistance);
//                     break;
//                 case Direction.Up:
//                 case Direction.Down:
//                     UpdateYCoordinate(image, moveDistance);
//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException();
//             }
//         }
//
//         private void UpdateYCoordinate(Image<Rgba32> image, int moveDistance)
//         {
//             for (var i = 0; i < _distance; i++)
//             {
//                 _rectangle.Y += moveDistance;
//                 image.Mutate(x => x.Fill(_colour, _rectangle));
//
//             }
//         }
//
//         private void UpdateXCoordinate(Image<Rgba32> image, int moveDistance)
//         {
//             for (var i = 0; i < _distance; i++)
//             {
//                 _rectangle.X += moveDistance;
//                 image.Mutate(x => x.Fill(_colour, _rectangle));
//
//             }
//         }
//     }
//
//     internal enum Direction
//     {
//         Up,
//         Down,
//         Left,
//         Right
//     }
//
// }