using System;
using System.Text;

namespace Pyxcell
{
    public class Pyxcell
    {


        public Pyxcell()
        {
            
        }
        
        // To give back a PNG file
        public void GenerateViaCommands(string text)
        {
            var textToEncode = text + DateTime.Now.ToFileTimeUtc();
            var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(textToEncode));
            
            // Take each character and convert it to octal

            var integers = new int[encoded.Length];

            var i = 0;
            foreach (var character in encoded)
            {
                integers[i] = Convert.ToInt32(character);
                i++;
            }

            var paddedIntegers = PaddIntArray(integers);
            
            Console.WriteLine(paddedIntegers.Length);


        }
        
        // Method to make array divisable by 4

        private static int[] PaddIntArray(int[] integers)
        {

            var mod = integers.Length % 4;
            if (mod == 0)
                return integers;

            var paddedIntegers = new int[integers.Length + mod];
            Array.Copy(integers, paddedIntegers, integers.Length);

            for (int i = 0; i < mod; i++)
                paddedIntegers[integers.Length + i] = integers[i];


            return paddedIntegers;
        }

    // Take each character of the base 64 encoded string (wrapping around when string is exhausted)
    // 


    // 

    // Command Class
    // Position
    // Colour
    // Distance
    // Direction
    // 
      

    }
}
