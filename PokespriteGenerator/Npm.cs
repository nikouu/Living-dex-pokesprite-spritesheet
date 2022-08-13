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
        public async Task GetTarball(string packageName, string? packageVersion = null)
        {
            var queryVersion = packageVersion ?? await GetLatestPackageVersion(packageName);

            var packageMetadata = await GetPackageMetadata(packageName, queryVersion);
        }

        private async Task<string> GetLatestPackageVersion(string packageName)
        {          
            var url = BuildNpmPackageUrl(packageName, null);
            var result = await GetNpmJson(url);
            var g = JsonSerializer.Deserialize<NpmPackageQueryModel>(result);

            return g.DistTags.Latest;
        }

        private async Task<NpmPackageModel> GetPackageMetadata(string packageName, string packageVersion)
        {
            var url = BuildNpmPackageUrl(packageName, packageVersion);
            var result = await GetNpmJson(url);
            var g = JsonSerializer.Deserialize<NpmPackageModel>(result);

            return g;
        }

        private async Task<string> GetNpmJson(Uri url)
        {
            var client = new HttpClient();
            var jsonString = await client.GetStringAsync(url);
            return jsonString;
        }

        private Uri BuildNpmPackageUrl(string packageName, string packageVersion)
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
