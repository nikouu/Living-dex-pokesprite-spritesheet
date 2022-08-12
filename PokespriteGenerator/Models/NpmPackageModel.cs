using System.Text.Json.Serialization;

namespace PokespriteGenerator.Models;

public class NpmPackageQueryModel
{
    [JsonPropertyName("dist-tags")]
    public DistTags DistTags { get; set; }
}

public class DistTags
{
    [JsonPropertyName("latest")]
    public string Latest { get; set; }
}


public class NpmPackageModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; }

    [JsonPropertyName("repository")]
    public Repository Repository { get; set; }

    [JsonPropertyName("author")]
    public Author Author { get; set; }

    [JsonPropertyName("license")]
    public string License { get; set; }

    [JsonPropertyName("dist")]
    public Dist Dist { get; set; }
}

public class Repository
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public class Author
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}

public class Dist
{
    [JsonPropertyName("integrity")]
    public string Integrity { get; set; }

    [JsonPropertyName("shasum")]
    public string Shasum { get; set; }

    [JsonPropertyName("tarball")]
    public string Tarball { get; set; }

    [JsonPropertyName("fileCount")]
    public int FileCount { get; set; }
    [JsonPropertyName("unpackedSize")]

    public int UnpackedSize { get; set; }

    [JsonPropertyName("npmsignature")]
    public string Npmsignature { get; set; }

    [JsonPropertyName("signatures")]
    public Signature[] Signatures { get; set; }
}

public class Signature
{
    [JsonPropertyName("keyid")]
    public string Keyid { get; set; }

    [JsonPropertyName("sig")]
    public string Sig { get; set; }
}

public class Npmoperationalinternal
{
    [JsonPropertyName("host")]
    public string Host { get; set; }

    [JsonPropertyName("tmp")]
    public string Tmp { get; set; }
}

public class Maintainer
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}
