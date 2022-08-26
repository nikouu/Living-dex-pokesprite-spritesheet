namespace PokespriteGenerator.Models;

public class BaseSpriteData
{
    public string Name { get; }
    public byte[] Image { get; }
    public int? TrimmedWidth { get; }
    public int? TrimmedHeight { get; }
    public virtual string ClassName => Name;

    protected BaseSpriteData(string name, byte[] image, int? trimmedWidth, int? trimmedHeight)
    {
        Name = name;
        Image = image;
        TrimmedWidth = trimmedWidth;
        TrimmedHeight = trimmedHeight;
    }
}