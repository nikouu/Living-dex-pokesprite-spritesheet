using PokespriteGenerator.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PokespriteGenerator
{
    public class Trimmer
    {
        private readonly ChannelReader<BaseSpriteData> _channelReader;
        private readonly ChannelWriter<BaseSpriteData> _channelWriter;

        public Trimmer(ChannelReader<BaseSpriteData> channelReader, ChannelWriter<BaseSpriteData> channelWriter)
        {
            _channelReader = channelReader;
            _channelWriter = channelWriter;
        }

        public async Task Trim()
        {
            // remove teh whitespace, and have it so its just the image without whitespace around it
            await foreach (BaseSpriteData item in _channelReader.ReadAllAsync())
            {
                using var imageStream = new MemoryStream(item.Image);
                using var image = new Bitmap(imageStream);

                var top = GetTop(image);
                var bottom = GetBottom(image);
                var left = GetLeft(image);
                var right = GetRight(image);

                // crop
                var srcRect = Rectangle.FromLTRB(left, top, right, bottom);
                var trimmedImage = image.Clone(srcRect, image.PixelFormat);

                using var memoryStream = new MemoryStream();
                trimmedImage.Save(memoryStream, ImageFormat.Png);

                // prob have to be a mapper
                if (item is PokemonData pokemonData)
                {
                    var newPokemonData = new PokemonData(pokemonData.Name, pokemonData.Number, pokemonData.Form, memoryStream.ToArray(), trimmedImage.Width, trimmedImage.Height);
                    await _channelWriter.WriteAsync(newPokemonData);
                }
                else if (item is BallData ballData)
                {
                    var newBallData = new BallData(ballData.Name, memoryStream.ToArray(), trimmedImage.Width, trimmedImage.Height);
                    await _channelWriter.WriteAsync(newBallData);
                }
            }

            Console.WriteLine("Completed trimming Pokemon data.");
            _channelWriter.Complete();
        }

        private int GetTop(Bitmap image)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);

                    if (pixel.A != 0)
                    {
                        return y;
                    }
                }
            }

            return 0;
        }

        private int GetBottom(Bitmap image)
        {
            for (int y = image.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);

                    if (pixel.A != 0)
                    {
                        return y + 1;
                    }
                }
            }

            return 0;
        }

        private int GetLeft(Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);

                    if (pixel.A != 0)
                    {
                        return x;
                    }
                }
            }

            return 0;
        }

        private int GetRight(Bitmap image)
        {
            for (int x = image.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);

                    if (pixel.A != 0)
                    {
                        return x + 1;
                    }
                }
            }

            return 0;
        }
    }
}