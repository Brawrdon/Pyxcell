# Pyxcell - A generative text to pixel art .NET Standard library.
![Test & Deploy](https://github.com/Brawrdon/Pyxcell/workflows/Test%20&%20Deploy/badge.svg?branch=master)

<p>
<img align="right" alt="Steve Jobs Wikipedia entry generation example" src="https://raw.githubusercontent.com/Brawrdon/Pyxcell/master/tests/Resources/SteveJobs.png" width="20%">

Pyxcell is a barcode like, pixel art encoder and decoder library which uses text as an input. It was designed to be used in my project for the Computational Creativity module at the University of Kent. My project involved utilising this library, along with sentiment analysis, to produce generative pixel art based on the emotions of a given text.

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

A base64 encoded string representation of the image is returned.

```csharp
using Encoder = Pyxcell.Encoder;
using Pyxcell.Common;
using SixLabors.ImageSharp.PixelFormats;

public string Encode()
{
    var message = "Here's some cool piece of text I want to encode into some cool piece of art."

    // Define the colours to be used in the palette.
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

    return PyxcellConvert.Encode("Hello world, how are you?", colours);
}
```

### Keyword Encoding

Pyxcell allows you to highlight all the characters in keywords with the same colour to make certain words stand out. Keywords and their colours are defined as key and value pairs. The main colour palette cannot contain colours used by keywords.

```csharp
using Encoder = Pyxcell.Encoder;
using Pyxcell.Common;
using SixLabors.ImageSharp.PixelFormats;

public string Encode()
{
    var colours = new List<Color>
    {
        Color.Blue,
        Color.Teal
    };

    var keywords = new Dictionary<string, Color>
    {  
        { "Hello", Color.Red },
        { "world", Color.Purple }
    };

    return PyxcellConvert.Encode("Hello world, how are you?", colours, keywords);
}
```

### Basic Decoding

```csharp
using Pyxcell;
using SixLabors.ImageSharp.PixelFormats;

public void Decode()
{
    var image = PyxcellConvert.Decode("HelloWorld.png");

    // The PyxcellImage class provides you with the following:
    string base64 = image.Base64;
    string message = image.Message;
    Dictionary<char, int[]> characterMappings = image.CharacterPatternMap; // Returns a copy of the dictionary containing character mappings
    Dictionary<string, Color> keywords = image.Keywords; // Returns a copy of the dictionary containing keywords
    List<Color> Colours = image.Colours; // Returns a copy of the list containing colours
}

```
