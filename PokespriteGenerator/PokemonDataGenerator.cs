using PokespriteGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PokespriteGenerator
{
    public class PokemonDataGenerator
    {
        private readonly Dictionary<string, byte[]> _rawData;
        private readonly ChannelWriter<PokemonData> _channelWriter;

        public PokemonDataGenerator(Dictionary<string, byte[]> rawData, ChannelWriter<PokemonData> channelWriter)
        {
            _rawData = rawData;
            _channelWriter = channelWriter;
        }

        public async Task Generate()
        {
            var pokemonJsonFile = _rawData.First(x => x.Key == "package/data/pokemon.json").Value;

            var pokemonJson = Encoding.Default.GetString(pokemonJsonFile);

            var pokemonMetadataModel = JsonSerializer.Deserialize<Dictionary<string, SinglePokemonMetadata>>(pokemonJson);

            foreach (var (key, value) in pokemonMetadataModel)
            {
                // crude way to get the normal base version
                var filename = _rawData.Keys.Where(x => x.Contains(value.Slug))
                    .Where(x => x.Contains("package/pokemon"))
                    .Where(x => !x.Contains("/shiny/"))
                    .OrderBy(x => x)
                    .Last();

                var model = new PokemonData(value.Name, value.Number, _rawData[filename]);

                await _channelWriter.WriteAsync(model);
            }

            _channelWriter.Complete();
        }
    }
}
