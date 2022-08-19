using PokespriteGenerator.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PokespriteGenerator
{
    public class SpritesheetGenerator
    {
        private const int Columns = 32;

        private readonly ChannelReader<PokemonData> _channelReader;

        public SpritesheetGenerator(ChannelReader<PokemonData> channelReader)
        {
            _channelReader = channelReader;
        }

        public async Task<(List<PokemonData>, byte[])> GenerateSpritesheet()
        {
            var pokemonDataList = new List<PokemonData>();
            await foreach (PokemonData item in _channelReader.ReadAllAsync())
            {
                pokemonDataList.Add(item);
            }

            var maxWidth = pokemonDataList.Max(x => x.TrimmedWidth).GetValueOrDefault();
            var maxHeight = pokemonDataList.Max(x => x.TrimmedHeight).GetValueOrDefault();

            var rows = (int)Math.Ceiling((double)(pokemonDataList.Count / Columns)) + 1;

            using var spritesheet = new Bitmap(Columns * maxWidth, rows * maxHeight);

            foreach (var item in pokemonDataList)
            {
                var number = int.Parse(item.Number);
                var column = (number -1) % Columns;
                var row = number / Columns;

                using var imageStream = new MemoryStream(item.Image);
                using var pokemonImage = new Bitmap(imageStream);

                using (var graphics = Graphics.FromImage(spritesheet))
                {
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    graphics.DrawImage(pokemonImage, column * maxWidth, row * maxHeight);
                }                   
            }

            using var memoryStream = new MemoryStream();
            spritesheet.Save(memoryStream, ImageFormat.Png);

            return (pokemonDataList, memoryStream.ToArray());
        }
    }
}
