namespace PokespriteGenerator.Models;

public class PokemonData : BaseSpriteData
{
    public string Number { get; }
    public string Form { get; }
    public override string ClassName => $$"""pkicon-{{Number}}{{(Form == "" ? "" : $".form-{Form}")}}""";
    public PokemonData(string name, string number, string form, byte[] image, int? trimmedWidth = null, int? trimmedHeight = null) 
        : base (name, image, trimmedWidth, trimmedHeight)
    {
        Number = number;
        Form = form;
    }
}
