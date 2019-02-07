using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Pyxcell
{
    public class Command
    {
        private readonly Direction _direction;
        private readonly int _distance;
        private readonly Rgb _colour;
        private Rectangle _rectangle;
        
        internal Command(int coordinates, int direction, int distance)
        {
            var x = GetX(coordinates);
            var y = GetY(coordinates);
            _direction = GetDirection(direction);
            _distance = GetDistance(distance);
            _colour = new Rgba32(Convert.ToByte(coordinates), Convert.ToByte(direction), Convert.ToByte(distance));
            _rectangle = new Rectangle(x, y, 10, 10);        

        }

        private static int GetDistance(int distance)
        {
            return (500 - distance) * 3 / 4 ;
        }

        private static Direction GetDirection(int direction)
        {
            var mod = direction % 4;

            switch (mod)
            {
                case 0:
                    return Direction.Up;
                case 1:
                    return Direction.Right;
                case 2:
                    return Direction.Down;
                case 3:
                    return Direction.Left;
                default:
                    return Direction.Up;
            }
        }

        private static int GetX(int coordinates)
        {
                
            var x = coordinates * coordinates;

            if (x <= 500) 
                return x;
            
            while (x > 500)
                x -= 500;

            return x;
        }
        
        private static int GetY(int coordinates)
        {
                
            var y = coordinates;


            if (coordinates > 255)
                y -= 5;
            
            return y;
        }

        public void Execute(Image<Rgba32> image)
        {
            if (_distance == 0)
                return;
            
            image.Mutate(x => x.Fill(_colour, _rectangle));
            
            UpdateCoordinate(image);
            
        }

        private void UpdateCoordinate(Image<Rgba32> image)
        {
            var moveDistance = 10;

            if (_direction == Direction.Up || _direction == Direction.Left)
                moveDistance *= -1;

            switch (_direction)
            {
                case Direction.Left:
                case Direction.Right:
                    UpdateXCoordinate(image, moveDistance);
                    break;
                case Direction.Up:
                case Direction.Down:
                    UpdateYCoordinate(image, moveDistance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateYCoordinate(Image<Rgba32> image, int moveDistance)
        {
            for (var i = 0; i < _distance; i++)
            {
                _rectangle.Y += moveDistance;
                image.Mutate(x => x.Fill(_colour, _rectangle));

            }
        }

        private void UpdateXCoordinate(Image<Rgba32> image, int moveDistance)
        {
            for (var i = 0; i < _distance; i++)
            {
                _rectangle.X += moveDistance;
                image.Mutate(x => x.Fill(_colour, _rectangle));

            }
        }
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

}