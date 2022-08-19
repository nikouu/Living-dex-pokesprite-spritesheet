# Living Dex Pokésprite Spritesheet
For my Living Dex Project, I need Pokémon sprites for my huge list of caught Pokémon in [Making a Living Dex: Appendix A - The Whole Living Dex Roster](https://www.nikouusitalo.com/blog/making-a-living-dex-appendix-a-the-whole-living-dex-roster/). The spites come from [Pokesprite](https://github.com/msikma/pokesprite), which I think is what everyone who needs sprites uses. The spritesheet I originally used was based off the work from [PokdexTracker](https://pokedextracker.com/) which had their own generator as [pokesprite-fork](https://github.com/pokedextracker/pokesprite-fork) which is a fork of the original Pokesprite, but then made their own [pokesprite](https://github.com/pokedextracker/pokesprite).

However I didn't like the direction PokedexTracker went with Pokemon Legends Arceus where they used the more rounded and 3D look that the game did for sprite portraits. And with that, this project was created so I could easily generate my own spritesheets exactly to my needs. 

| Pixel-based                                    | 3D-based                                    |
| ---------------------------------------------- | ------------------------------------------- |
| ![image](docs/pokdextracker-pixel-sprites.png) | ![image](docs/pokdextracker-3d-sprites.png) |
                                                

This work is be heavily influenced by the work from PokdexTracker. 

## Pipeline

1. Get latest sprites and associated metadata
1. Move sprite from centre to top left
1. Trim whitespace such that all whitespace is uniformly sized for each sprite
1. Generate single spritesheet
1. Generate `.scss` and subsequently `.css` files
1. Copy to output location

## Trying out newer .NET features
Decided to force in a few newer features to learn too. 

### `System.Threading.Channels`
This project is also an experiment to learn about the newish `System.Threading.Channels` and how to create parallel and linked up pipelines using it. 


### `System.Formats.Tar`

### Working out specific use cases for `using` declarations

### Local functions

### Raw string literals

### `<LangVersion>preview</LangVersion>`




## Setup

### Installing dotnet

## Usage


