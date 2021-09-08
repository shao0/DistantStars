using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DistantStars.Client.Common.Tools.Interfaces;

namespace DistantStars.Client.DAL
{
    public class WebDataAccess
    {
        private readonly IConfig _config;
        public WebDataAccess(IConfig config)
        {
            _config = config;
        }
        private string Domain => _config.ReadByKey("Domain");

        protected string GetCompleteUri(string uri, string domain = null) => string.IsNullOrWhiteSpace(domain) ? $"{Domain}{uri}" : $"{domain}{uri}";
        protected async Task<Stream> GetStreamAsync(string uri, string domain = null)
        {
            uri = GetCompleteUri(uri, domain);
            return await Task.Run(() => new HttpClient().GetStreamAsync(uri));
        }
        protected async Task<string> GetStringAsync(string uri, string domain = null)
        {
            uri = GetCompleteUri(uri, domain);

            return await Task.Run(() => new HttpClient().GetStringAsync(uri));

        }
        protected async Task<byte[]> GetByteArrayAsync(string uri, string domain = null)
        {
            uri = GetCompleteUri(uri, domain);

            return await Task.Run(() => new HttpClient().GetByteArrayAsync(uri));
        }

        private async Task<HttpContent> PostContentAsync(string uri, HttpContent content, string domain = null)
        {
            uri = GetCompleteUri(uri, domain);
            return await Task.Run(async () =>
            {
                var client = new HttpClient();
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content;
                }
                throw new Exception("请求失败" + response.RequestMessage);
            });
        }
        protected async Task<string> PostStringAsync(string uri, HttpContent content, string domain = null)
        {
            content = await PostContentAsync(uri, content, domain);
            return await content.ReadAsStringAsync();
        }
        protected async Task<byte[]> PostBytesAsync(string uri, HttpContent content, string domain = null)
        {
            content = await PostContentAsync(uri, content, domain);
            return await content.ReadAsByteArrayAsync();
        }
        protected async Task<Stream> ReadAsStreamAsync(string uri, HttpContent content, string domain = null)
        {
            content = await PostContentAsync(uri, content, domain);
            return await content.ReadAsStreamAsync();
        }


    }
}
