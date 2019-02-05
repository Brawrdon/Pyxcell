using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Pyxcell
{
    public class Command
    {
        private int[] _coordinates;
        private Direction _direction;
        private int _distance;
        private string _colour;
        
        internal Command(int coordinates, int direction, int distance, int colour)
        {
         
            // Random checks to set the values
            using (Image<Rgba32> image = new Image<Rgba32>(500, 200))
            {
                var rectangle = new Rectangle(0, 0, 100, 100);
                image.Mutate(x => x.Fill(Rgba32.Purple, rectangle));
                
                image.Save("hello.png");


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