using System.Text.Json;
using System.Text.Json.Serialization;

namespace PokespriteGenerator.Models;

public class SinglePokemonMetadata
{
    [JsonPropertyName("idx")]
    public string Number { get; set; }

    public string Name => PokemonName.Name;

    public string Slug => PokemonSlug.Slug;

    [JsonPropertyName("name")]
    public PokemonName PokemonName { get; set; }

    [JsonPropertyName("slug")]
    public PokemonSlug PokemonSlug { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? RemainingData { get; set; }

    public string LatestGeneration => RemainingData.Keys.Where(x => x.Contains("gen")).OrderBy(x => x).Last();

    private List<string> _forms = new();

    public List<string> Forms
    {
        get
        {
            if (_forms.Any())
            {
                return _forms;
            }

            var jsonElement = RemainingData[LatestGeneration].EnumerateObject();

            foreach (var item in jsonElement)
            {
                var keys = item.Value.EnumerateObject().Select(x => x.Name).Where(x => x != "$");
                _forms.AddRange(keys);
            }

            return _forms;
        }
    }
}

public class PokemonName
{
    [JsonPropertyName("eng")]
    public string Name { get; set; }
}

public class PokemonSlug
{
    [JsonPropertyName("eng")]
    public string Slug { get; set; }
}