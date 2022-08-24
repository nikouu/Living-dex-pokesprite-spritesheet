# PokéSprite Spritesheet
For my Living Dex Project, I need Pokémon sprites for my huge list of caught Pokémon in [Making a Living Dex: Appendix A - The Whole Living Dex Roster](https://www.nikouusitalo.com/blog/making-a-living-dex-appendix-a-the-whole-living-dex-roster/). The spites come from [Pokesprite](https://github.com/msikma/pokesprite), which I think is what everyone who needs sprites uses. The spritesheet I originally used was based off the work from [PokdexTracker](https://pokedextracker.com/) which had their own generator as [pokesprite-fork](https://github.com/pokedextracker/pokesprite-fork) which is a fork of the original Pokesprite, but then made their own [pokesprite](https://github.com/pokedextracker/pokesprite).

However I didn't like the direction PokedexTracker went with Pokemon Legends Arceus where they used the more rounded and 3D look that the game did for sprite portraits. And with that, this project was created so I could easily generate my own spritesheets exactly to my needs. 

| Pixel-based                                    | 3D-based                                    |
| ---------------------------------------------- | ------------------------------------------- |
| ![image](docs/pokdextracker-pixel-sprites.png) | ![image](docs/pokdextracker-3d-sprites.png) |
                                                

This work is be heavily influenced by the work from PokdexTracker. 

## Pipeline

1. Get latest sprites and associated metadata
1. Scale the Pokemon sprite if needed 
1. Trim whitespace such that there is zero whitespace padding around the sprite
1. Generate single spritesheet
1. Generate `.css` files
1. Copy to output location

## Trying out newer .NET features
Decided to force in a few newer features to learn too. 

### `System.Threading.Channels`
This project is also an experiment to learn about the newish `System.Threading.Channels` and how to create parallel and linked up pipelines using it. Originally I had a place in another repo of mine for learning these: [System.Threading.Channels Learnings](https://github.com/nikouu/System.Threading.Channels-Learnings).

Channels are used here to process each Pokemon through the pipeline asynchronously.

### `System.Formats.Tar`
Having a few RaspberryPi projects using .NET, I became excited to see the [feature request to add `System.Formats.Tar`](https://github.com/dotnet/runtime/issues/65951). The Pokesprite NPM package that gets downloaded in a `.tar` format and needs to be extracted. Feel free to check out more information in my post [How to Natively Read .tgz Files With the New C# TarReader Class](https://www.nikouusitalo.com/how-to-natively-read-tgz-files-with-the-new-c-tarreader-class/).

### Working out specific use cases for `using` declarations
In .NET it often feels like `IDisposable` types are heavily paired with _[`using` statements](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement)_. Then as of C# 8 (.NET Core 3.x+) we got a new feature: _`using` declarations_. With these you don't explicitly set the scope and these `IDisposable` objects are disposed when the outer scope is completed. 

For example:

```csharp
// using statements
using (var gzip = new GZipStream(tgzStream, CompressionMode.Decompress))
{
	using (var unzippedStream = new MemoryStream())
	{
		await gzip.CopyToAsync(unzippedStream);
		unzippedStream.Seek(0, SeekOrigin.Begin);

		using (var reader = new TarReader(unzippedStream))
		{

		}
	}
}
```

```csharp
// using declarations
using var gzip = new GZipStream(tgzStream, CompressionMode.Decompress);

using var unzippedStream = new MemoryStream();
await gzip.CopyToAsync(unzippedStream);
unzippedStream.Seek(0, SeekOrigin.Begin);

using var reader = new TarReader(unzippedStream);
```

However, this pattern can't be used everywhere. For instance when using the `Graphics` object to manipulate a `Bitmap` object, the `Graphics` needs to be disposed before the changes occur in the `Bitmap` (I think..). Meaning there is a piece of code here that uses both styles of `using`:

```csharp
using var imageStream = new MemoryStream(item.Image);
using var pokemonImage = new Bitmap(imageStream);

using (var graphics = Graphics.FromImage(spritesheet))
{
	graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
	graphics.DrawImage(pokemonImage, column * maxWidth, row * maxHeight);
}   
```

### Local functions
Introduced in C# 7 (Framework and Core 2.x+) were [local functions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/local-functions). I don't think the project has a good use case for it, but I wedged it into `Npm.cs` to get what the most recent version of the NPM package is. 

### Raw string literals
At the time of writing this, [Raw String Literals](https://devblogs.microsoft.com/dotnet/csharp-11-preview-updates/) is a C# 11 preview feature. They're strings that let you put all sorts of characters in it like quotes or backslashes without escaping them - because you cannot escape anything inside a raw string literal. 

The syntax using triple double quotes on each side of the string: `"""..."""`. For this project it gets used as part of the css generation just to make the presentation nicer for the base css class:
```
private string RootClass = """
    .pkicon {
        @include crisp-rendering();
        display: inline-block;
        background-image: url("pokesprite.png");
        background-repeat: no-repeat;
    }
    """;
```

Then used the interpolated version of raw string literals to create each Pokemon (and form) class. Again, I didn't technically need to use them, but it made it much easier to deal with curly brackets required for css:
```
var cssClass = $$""".pkicon.pkicon-{{item.Number}}{{FormData(item.Form)}} { width: {{item.TrimmedWidth}}px; height: {{item.TrimmedHeight}}px; background-position: -{{column * maxWidth}}px -{{row * maxHeight}}px }""";

```

### Preview language features
In order to use the raw string literals, I had to edit my [project file](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version#edit-the-project-file) to use `<LangVersion>preview</LangVersion>`.



## Setup

### Installing dotnet

## Usage


