using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DistantStars.Client.Common.Tools.Interfaces;
using DistantStars.Client.IDAL.Systems;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;
using Newtonsoft.Json;

namespace DistantStars.Client.DAL.Systems
{
    public class FileDAL : WebDataAccess, IFileDAL
    {
        public FileDAL(IConfig config) : base(config)
        {
        }
        public async Task<byte[]> DownloadFileAsync(FileParameter parameter)
        {
            var uri = $"File/DownloadFile{parameter.GetUriParameter()}";
            return await GetByteArrayAsync(uri);
        }

        public async Task<FileInfoDto> UploadFileAsync(FileParameter parameter, string filePath)
        {
            var uri = $"File/UploadFile{parameter.GetUriParameter()}";
            var fs = File.OpenRead(filePath);
            var contents = new MultipartFormDataContent();
            var streamContent = new StreamContent(fs);
            streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"SXF.jpg\"");
            streamContent.Headers.Add("Content-Type", "image/jpeg");
            contents.Add(streamContent);
            var json = await PostStringAsync(uri, contents);
            fs.Dispose();
            return JsonConvert.DeserializeObject<FileInfoDto>(json);
        }
    }
}
