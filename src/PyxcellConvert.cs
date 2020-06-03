using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp.PixelFormats;

namespace Pyxcell
{
    public static class PyxcellConvert 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string Encode(string message, List<Rgba32> colours, Dictionary<string, Rgba32> keywords = null)
        {     
            var gridPallete = new GridPalette(colours, keywords); 
            throw new NotImplementedException();

        }



    }
}