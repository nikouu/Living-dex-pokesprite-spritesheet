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
    public class Scaler
    {
        private const int Threshold = 100;
        private const float ScaleTo = 0.5f;

        private readonly ChannelReader<BaseSpriteData> _channelReader;
        private readonly ChannelWriter<BaseSpriteData> _channelWriter;


        public Scaler(ChannelReader<BaseSpriteData> channelReader, ChannelWriter<BaseSpriteData> channelWriter)
        {
            _channelReader = channelReader;
            _channelWriter = channelWriter;
        }

        // is this even needed for my case..?
        public async Task Scale()
        {
            await foreach (BaseSpriteData item in _channelReader.ReadAllAsync())
            {
                using var imageStream = new MemoryStream(item.Image);
                using var image = new Bitmap(imageStream);

                if (image.Width > Threshold || image.Height > Threshold)
                {
                    // scale image
                    //var scaledX = (int)Math.Round(image.Width * ScaleTo);
                    //var scaledY = (int)Math.Round(image.Height * ScaleTo);

                    //using var resizedImage = new Bitmap(scaledX, scaledY, PixelFormat.Format64bppArgb);
                    //resizedImage.SetResolution(image.Width, image.Height);
                    //using var graphics = Graphics.FromImage(resizedImage);
                    //graphics.DrawImage(image, new Rectangle(0, 0, scaledX, scaledY), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

                    //using var memoryStream = new MemoryStream();
                    //resizedImage.Save(memoryStream, ImageFormat.Png);
                    //var newPokemonData = new PokemonData(item.Name, item.Number, item.Form, memoryStream.ToArray());

                    //await _channelWriter.WriteAsync(newPokemonData);
                }
                else
                {
                    await _channelWriter.WriteAsync(item);
                }
            }

            Console.WriteLine("Completed scaling Pokemon data.");
            _channelWriter.Complete();
        }
    }
}
