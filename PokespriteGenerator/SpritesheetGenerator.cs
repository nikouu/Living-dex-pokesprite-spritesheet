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

        private readonly ChannelReader<BaseSpriteData> _channelReader;

        public SpritesheetGenerator(ChannelReader<BaseSpriteData> channelReader)
        {
            _channelReader = channelReader;
        }

        public async Task<(List<BaseSpriteData>, byte[])> GenerateSpritesheet()
        {
            var spriteDataList = new List<BaseSpriteData>();
            await foreach (BaseSpriteData item in _channelReader.ReadAllAsync())
            {
                spriteDataList.Add(item);
            }

            var maxWidth = spriteDataList.Max(x => x.TrimmedWidth).GetValueOrDefault();
            var maxHeight = spriteDataList.Max(x => x.TrimmedHeight).GetValueOrDefault();

            var rows = (int)Math.Ceiling((double)(spriteDataList.Count / Columns)) + 1;

            using var spritesheet = new Bitmap(Columns * maxWidth, rows * maxHeight);

            // todo: work out a way nicer way of doing this
            // maybe as an icomparable for each object, but then would need some sort of meta reflection to 
            // pick apart the list and what subtypes are in it
            var pokemonList = spriteDataList.OfType<PokemonData>().OrderBy(x => int.Parse(x.Number)).Cast<BaseSpriteData>();
            var ballList = spriteDataList.OfType<BallData>().OrderBy(x => x.Name).Cast<BaseSpriteData>();
            var orderedSpriteDataList = pokemonList.Concat(ballList).ToList();

            foreach (var item in orderedSpriteDataList)
            {
                var number = orderedSpriteDataList.IndexOf(item);
                var column = (number) % Columns;
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

            Console.WriteLine("Completed generating Pokemon spritesheet.");
            return (orderedSpriteDataList, memoryStream.ToArray());
        }
    }
}
