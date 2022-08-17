// See https://aka.ms/new-console-template for more information
using PokespriteGenerator;
using System.Formats.Tar;
using System.IO.Compression;


// remember System.Formats.Tar is a thing now https://github.com/dotnet/runtime/issues/65951
// https://www.npmjs.com/package/pokesprite-images/v/2.6.0
// https://registry.npmjs.org/pokesprite-images/2.6.0

var g = new Npm();

var tgzStream = await g.GetTarball("pokesprite-images");
tgzStream.Seek(0, SeekOrigin.Begin);

using var gzip = new GZipStream(tgzStream, CompressionMode.Decompress);

using var unzippedStream = new MemoryStream();
await gzip.CopyToAsync(unzippedStream);
unzippedStream.Seek(0, SeekOrigin.Begin);

using var reader = new TarReader(unzippedStream);

while (reader.GetNextEntry() is TarEntry entry)
{
	Console.WriteLine($"Entry name: {entry.Name}, entry type: {entry.EntryType}");
	//entry.ExtractToFile(destinationFileName: Path.Join("D:/MyExtractionFolder/", entry.Name), overwrite: false);
}




Console.WriteLine("Hello, World!");

/*
 * 
 * https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/
 * https://itnext.io/use-system-io-pipelines-and-system-threading-channels-apis-to-boost-performance-832d7ab7c719
 * http://www.workflowpatterns.com/patterns/control/basic/wcp1.php
 * http://www.workflowpatterns.com/patterns/
 * https://www.codeproject.com/Articles/1182984/Chain-of-Responsibility-and-Strategy-pattern-using
 * https://www.stevejgordon.co.uk/an-introduction-to-system-threading-channels
 * https://deniskyashif.com/2019/12/08/csharp-channels-part-1/
 */