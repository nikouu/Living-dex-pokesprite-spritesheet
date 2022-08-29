using PokespriteGenerator;
using PokespriteGenerator.Models;
using System.Threading.Channels;

var npm = new Npm();

var tgzStream = await npm.GetTarball("pokesprite-images");

var decompressor = new Decompressor();

var files = await decompressor.DecompressTgzAsync(tgzStream);

var initialDataChannel = Channel.CreateUnbounded<BaseSpriteData>();
var scaledDataChannel = Channel.CreateUnbounded<BaseSpriteData>();
var trimmedDataChannel = Channel.CreateUnbounded<BaseSpriteData>();

var generator = new PokemonDataGenerator(files, initialDataChannel.Writer);
var scaler = new Scaler(initialDataChannel.Reader, scaledDataChannel.Writer);
var trimmer = new Trimmer(scaledDataChannel.Reader, trimmedDataChannel.Writer);
var spritesheetGenerator = new SpritesheetGenerator(trimmedDataChannel.Reader);

var generatePokemonData = generator.Generate();
var scaledPokemonData = scaler.Scale();
var trimmedPokemonData = trimmer.Trim();
var generateSpritesheet = spritesheetGenerator.GenerateSpritesheet();

await Task.WhenAll(generatePokemonData, scaledPokemonData, trimmedPokemonData, generateSpritesheet);

// we can safely use .Result after a whenall because its done by then
var (spriteData, spritesheet) = generateSpritesheet.Result;

var scssGenerator = new ScssGenerator();

var scssString = scssGenerator.GenerateScss(spriteData);

// should probably do some png crushing too

var crudeSaveLocation = new DirectoryInfo(System.Reflection.Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\..\output");
File.WriteAllBytes(Path.Combine(crudeSaveLocation.FullName, "pokesprite.png"), spritesheet);
File.WriteAllText(Path.Combine(crudeSaveLocation.FullName, "pokesprite.css"), scssString);

var testString = spriteData.Select(x => $$"""<i class="pokesprite {{x.ClassName.Replace(".", " ")}}"></i>{{(spriteData.IndexOf(x) % 32 == 31 ? $"<br />{Environment.NewLine}" : "") }}""");

string FormData(string form)
{
    if (form == "")
    {
        return "";
    }

    return $"form-{form}";
}

var testPage = $$"""
<!DOCTYPE html>
<html>
<head>
	<link rel="stylesheet" href="pokesprite.css">
</head>
    
<body>
{{string.Join("", testString)}}  
</body>
</html>
""";

File.WriteAllText(Path.Combine(crudeSaveLocation.FullName, "test.html"), testPage);

Console.WriteLine("Hello, World!");