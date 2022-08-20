﻿using PokespriteGenerator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
            try
            {
                var pokemonJsonFile = _rawData.First(x => x.Key == "package/data/pokemon.json").Value;

                var pokemonJson = Encoding.Default.GetString(pokemonJsonFile);

                var pokemonMetadataModel = JsonSerializer.Deserialize<Dictionary<string, SinglePokemonMetadata>>(pokemonJson);


                foreach (var (key, value) in pokemonMetadataModel)
                {
                    Console.WriteLine($"Processing {value.Name}");
                    var normalFilename = $"package/pokemon-{value.LatestGeneration.Replace("-", "")}/regular/{value.Slug}.png";
                    var normalVersion = new PokemonData(value.Name, value.Number, "", _rawData[normalFilename]);
                    await _channelWriter.WriteAsync(normalVersion);

                    foreach (var form in value.Forms)
                    {
                        var formFilename = $"package/pokemon-{value.LatestGeneration.Replace("-", "")}/regular/{value.Slug}-{form}.png";

                        if (!_rawData.ContainsKey(formFilename))
                        {
                            Console.WriteLine($"Did not find file for {formFilename}");
                            continue;
                        }
                        
                        var formVersion = new PokemonData(value.Name, value.Number, form, _rawData[formFilename]);
                        await _channelWriter.WriteAsync(formVersion);
                    }
                }

                _channelWriter.Complete();

                Console.WriteLine("Completed generating Pokemon data.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}