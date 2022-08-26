using PokespriteGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PokespriteGenerator
{
    public class BallDataGenerator
    {
        private readonly Dictionary<string, byte[]> _rawData;
        private readonly ChannelWriter<BaseSpriteData> _channelWriter;

        public BallDataGenerator(Dictionary<string, byte[]> rawData, ChannelWriter<BaseSpriteData> channelWriter)
        {
            _rawData = rawData;
            _channelWriter = channelWriter;
        }

        public async Task Generate()
        {
            var ballImageFiles = _rawData.Where(x => x.Key.Contains("/items/ball"));

            foreach (var item in ballImageFiles)
            {
                Console.WriteLine($"Processing {item.Key}");
                var name = item.Key.Replace("package/items/ball/", "").Replace(".png", "");
                var ballData = new BallData(name, item.Value);
                await _channelWriter.WriteAsync(ballData);
            }

            _channelWriter.Complete();
        }

        // \package\data\legacy\item-icons.json
        // items/ball
        // under the "ball" key
    }
}
