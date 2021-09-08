using DistantStars.Common.DTO.Parameters;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Dtos;

namespace DistantStars.Client.IBLL.Systems
{
    public interface IFileBLL
    {
        Task<byte[]> DownloadFileAsync(FileParameter parameter);

        Task<FileInfoDto> UploadFileAsync(FileParameter parameter, string filePath);
    }
}
