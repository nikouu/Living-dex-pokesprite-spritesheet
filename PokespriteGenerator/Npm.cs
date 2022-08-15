using PokespriteGenerator.Models;
using System.Text.Json;

namespace PokespriteGenerator
{
    public class Npm
    {
        // pass streams or byte[] back/around?
        public async Task GetTarball(string packageName, string? packageVersion = null)
        {
            var version = packageVersion ?? await GetLatestVersionAsync();

            var packageMetadata = await GetPackageMetadataAsync<NpmPackageModel>(packageName, version);

            

            // messing with local functions
            async Task<string> GetLatestVersionAsync()
            {
                var latestVersion = await GetPackageMetadataAsync<NpmPackageQueryModel>(packageName, null);
                return latestVersion.DistTags.Latest;
            }
        }

        private async Task<T> GetPackageMetadataAsync<T>(string packageName, string packageVersion)
        {
            var url = BuildNpmPackageUrl(packageName, packageVersion);
            var json = await GetNpmJsonAsync(url);
            var packageMetadata = JsonSerializer.Deserialize<T>(json);

            return packageMetadata;
        }

        private async Task<string> GetNpmJsonAsync(Uri url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            return json;
        }

        private Uri BuildNpmPackageUrl(string packageName, string? packageVersion)
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
