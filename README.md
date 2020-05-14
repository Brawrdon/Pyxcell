# Pyxcell - A generative text to pixel art .NET Standard library.
![Test & Deploy](https://github.com/Brawrdon/Pyxcell/workflows/Test%20&%20Deploy/badge.svg?branch=master)

<p>
<img align="right" alt="Steve Jobs Wikipedia entry generation example" src="https://raw.githubusercontent.com/Brawrdon/Pyxcell/master/tests/Resources/SteveJobs.png" width="20%">

Pyxcell is a barcode like, pixel art encoder and decoder library which uses text as an input. It was designed to be used in my project for the Computational Creativity module at the University of Kent. My project involed utilising this library, along with sentiment analysis, to produce generative pixel art based on the emotions of a given text.

</p>

## How It Works
An arbitrary image size of 700 by 700 pixels is used.

The image is split into 50 by 50 grids, with a size of 14 by 14 pixels. Each grid represents a character. Only the ASCII characters 32 and 127 are supported. The last character, DEL, is used as terminator character.

In order to represent the 95 possible characters, as well as other encodable information, grids can be painted in a specific fashion represented by a 7-bit binary value. This allows for each character to be randomly mapped to a value between 1 and 127. We ignore 0 to remove the possibility of producing an empty grid. Each bit in the binary value represents whether or not a 2 pixel wide line should be drawn within a grid.

This provides up to 3 distinct pieces of information to encode: the characters and their representations, the set of standard colours and the set of optional keyword colours. An unused 7-bit value is selected to decide how grids containing colours should be drawn. The pattern is meaningless and is used to ensure a coherent aesthetic. Each piece of metadata ends with the DEL terminating character, with which Pyxcell can determine which type of information it is currently reading.


Each generation is be seemingly random. Generations using the same texts theoretically will have a small chance to reproduce the same image due to the usage of random selection. 

## Installation
Pyxcell is currently built for _.NET Standard 2.1_. You can install it by downloading packages from GitHub / NuGet, or by using:

```sh
# .NET Core CLI
$ dotnet add package Pyxcell --version 0.7.0-alpha
```

## Dependencies
Pyxcell uses [SixLabors' ImageSharp](https://github.com/SixLabors/ImageSharp) library for drawing and reading images. It also makes use of its ```Rgba32``` class when defining colour palettes.

## Usage
### Basic Encoding
A base 64 encoded string representation of the is returned.

```csharp
using Encoder = Pyxcell.Encoder;
using Pyxcell.Common;
using SixLabors.ImageSharp.PixelFormats;

public string Encode()
{
    var message = "Here's some cool piece of text I want to encode into some cool piece of art."
   
    // Define the standard colours to be used in the palette.
    var colours = new List<Rgba32>
    {
        Rgba32.Aqua,
        Rgba32.Chocolate,
        Rgba32.Fuchsia,
        Rgba32.Khaki,
        Rgba32.Blue,
        Rgba32.Salmon,
        Rgba32.Coral,
        Rgba32.DarkGreen,
        Rgba32.Lime,
        Rgba32.Crimson
    };
    
    // Convert the string into ASCII. Pyxcel can currently only process ASCII characters 32 - 127.
    message = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(message.Content));
    
    // Trim the length of the message to account for the amount of free space left.
    // This depends on the number of colours defined.
    if (message.Content.Length > 2300)
        message.Content = message.Content.Substring(0, 2300);
        
    // Create a colourPalette object.
    var colourPalette = new ColourPalette(colours);
    var encoder = new Encoder(colourPalette);
    
    return encoder.Generate(message);
}
```

### Keyword Encoding
Pyxcell allows you to highlight all the characters in keywords with the same colour to make certain words stand out. Keywords and their colours are defined as pairs. The standard colour palette cannot contain keyword colours. These colours will be automatically removed from the standard palette.

```csharp
using Encoder = Pyxcell.Encoder;
using Pyxcell.Common;
using SixLabors.ImageSharp.PixelFormats;

public string Encode()
{
    var encoder = new Encoder(colourPalette, new List<Keyword>
            {
                new Keyword { Word = "cool", Colour = Rgba32.Black},
                new Keyword { Word = "piece", Colour = Rgba32.Yellow},
            }); );
    
    return encoder.Generate(message);
}
```

### Basic Decoding
```csharp
using Pyxcell;
using SixLabors.ImageSharp.PixelFormats;

public void Decode()
{
    var decoder = new Decoder("path/to/encoded/image");
    var decodedImage = decoder.Decode();
    
    // The DecodedImage class provides you with the following:
    var characterMappings = decodedImage.CharacterMappings;
    
    var colourPalette = decodedImage.ColourPalette;
    var keywordColourPalette = decodedImage.KeywordColourPalette;
        
    var text = decodedImage.Text;
    var keywords = decodedImage.Keywords;
}

```
