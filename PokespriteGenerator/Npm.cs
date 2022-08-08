using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokespriteGenerator
{
    internal class Npm
    {
        private readonly Uri NpmUrl = new("https://registry.npmjs.com/");

        private void GetTarball(string packageName, string packageVersion)
        {

        }
    }


    internal class PackageMetadata
    {
        public DistributionTags disttags { get; set; }
    }

    internal class DistributionTags
    {
        public string latest { get; set; }
    }

    public class PackageVersionMetadata
    {
        public Distribution dist { get; set; }

    }

    public class Distribution
    {
        public string integrity { get; set; }
        public string shasum { get; set; }
        public string tarball { get; set; }
        public int fileCount { get; set; }
        public int unpackedSize { get; set; }
        public string npmsignature { get; set; }
        public Signature[] signatures { get; set; }
    }

    public class Signature
    {
        public string keyid { get; set; }
        public string sig { get; set; }
    }
}
