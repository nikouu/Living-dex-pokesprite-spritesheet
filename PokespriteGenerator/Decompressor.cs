using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokespriteGenerator
{
    public class Decompressor
    {
        public async Task<Dictionary<string, byte[]>> DecompressTgzAsync(MemoryStream tgzStream)
        {
            using var gzip = new GZipStream(tgzStream, CompressionMode.Decompress);

            using var unzippedStream = new MemoryStream();
            await gzip.CopyToAsync(unzippedStream);
            unzippedStream.Seek(0, SeekOrigin.Begin);

            using var reader = new TarReader(unzippedStream);

            var files = new Dictionary<string, byte[]>();

            while (reader.GetNextEntry() is TarEntry entry)
            {
                //Console.WriteLine($"Entry name: {entry.Name}, entry type: {entry.EntryType}");
                using var memoryFile = new MemoryStream();
                await entry.DataStream.CopyToAsync(memoryFile);
                files.Add(entry.Name, memoryFile.ToArray());
            }

            return files;
        }
    }
}
