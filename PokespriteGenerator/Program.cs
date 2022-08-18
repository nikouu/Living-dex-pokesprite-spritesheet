// See https://aka.ms/new-console-template for more information
using PokespriteGenerator;

// remember System.Formats.Tar is a thing now https://github.com/dotnet/runtime/issues/65951
// https://www.npmjs.com/package/pokesprite-images/v/2.6.0
// https://registry.npmjs.org/pokesprite-images/2.6.0

var npm = new Npm();

var tgzStream = await npm.GetTarball("pokesprite-images");
tgzStream.Seek(0, SeekOrigin.Begin);



Func<string, bool> filter = (entry) =>
{
    if (entry.StartsWith(@"package/pokemon-gen8/regular"))
    {
        return true;
    }
    else if (entry.StartsWith(@"package/data/pokemon.json"))
    {
        return true;
    }
    else
    {
        return false;
    }
};

var decompressor = new Decompressor();

var files = await decompressor.DecompressTgzAsync(tgzStream, filter);




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