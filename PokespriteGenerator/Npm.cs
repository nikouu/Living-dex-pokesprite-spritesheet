using PokespriteGenerator.Models;
using System.Text.Json;

namespace PokespriteGenerator
{
    public class Npm
    {
        // pass streams or byte[] back/around?
        public async Task<MemoryStream> GetTarball(string packageName, string? packageVersion = null)
        {
            var version = packageVersion ?? await GetLatestVersionAsync();

            var packageMetadata = await GetPackageMetadataAsync<NpmPackageModel>(packageName, version);

            var httpClient = new HttpClient();

            // returns non seekable stream, is this fine for my purposes?
            // if not, can just copy to a memorystream
            // https://stackoverflow.com/a/3373614
            using var stream = await httpClient.GetStreamAsync(packageMetadata.Dist.Tarball);
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;

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
