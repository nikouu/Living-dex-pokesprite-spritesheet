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

            foreach (var item in spriteDataList.OrderByDescending(x => x.GetType().Name))
            {
                var number = spriteDataList.IndexOf(item);
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
            return (spriteDataList, memoryStream.ToArray());
        }
    }
}
