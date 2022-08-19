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
