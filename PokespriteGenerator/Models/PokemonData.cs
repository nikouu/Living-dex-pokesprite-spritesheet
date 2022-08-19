namespace PokespriteGenerator.Models;

public record struct PokemonData(string Name, string Number, byte[] Image, int? TrimmedWidth = null, int? TrimmedHeight = null);
