using System.Threading.Tasks;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.IDAL.Systems
{
    public interface IFileDAL
    {
        Task<byte[]> DownloadFileAsync(FileParameter parameter);

        Task<FileInfoDto> UploadFileAsync(FileParameter parameter, string filePath);

    }
}
