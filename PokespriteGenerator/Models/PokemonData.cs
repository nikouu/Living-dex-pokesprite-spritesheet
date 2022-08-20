namespace PokespriteGenerator.Models;

public record struct PokemonData(string Name, string Number, string Form, byte[] Image, int? TrimmedWidth = null, int? TrimmedHeight = null);
