using PokespriteGenerator.Models;
using System.Numerics;

public class BallData : BaseSpriteData
{
    public override string ClassName => $$"""ball.{{Name}}""";
    public BallData(string name, byte[] image, int? trimmedWidth = null, int? trimmedHeight = null)
        : base(name, image, trimmedWidth, trimmedHeight) { }
}