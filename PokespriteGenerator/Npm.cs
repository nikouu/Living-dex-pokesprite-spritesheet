using PokespriteGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokespriteGenerator
{
    public class Npm
    {
        private readonly Uri NpmUrl = new("https://registry.npmjs.com/");

        public async Task GetTarball(string packageName, string? packageVersion = null)
        {
            var queryVersion = packageVersion ?? await GetLatestPackageVersion(packageName);

            var packageMetadata = await GetPackageData(packageName, queryVersion);
        }

        private async Task<string> GetLatestPackageVersion(string packageName)
        {
            var client = new HttpClient();
            var url = BuildNpmUrl(packageName, null);
            var s = client.GetStringAsync(url);
            var result = await s;
            var g = JsonSerializer.Deserialize<NpmPackageQueryModel>(result);

            return g.DistTags.Latest;
        }

        private async Task<NpmPackageModel> GetPackageData(string packageName, string packageVersion)
        {
            // https://dotnetfiddle.net/LOwriN        

            var client = new HttpClient();


            var url = BuildNpmUrl(packageName, packageVersion);

            var s = client.GetStringAsync(url);

            var result = await s;

            var g = JsonSerializer.Deserialize<NpmPackageModel>(result);

            return g;
        }

        private Uri BuildNpmUrl(string packageName, string packageVersion)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = "https",
                Host = "registry.npmjs.com",
                Path = $"{packageName}/{packageVersion}"
            };

            return uriBuilder.Uri;
        }
    }
}
