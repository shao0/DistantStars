using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.IDAL.Systems;
using DistantStars.Common.DTO.Parameters;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Dtos;

namespace DistantStars.Client.BLL.Systems
{
    public class FileBLL : IFileBLL
    {
        private readonly IFileDAL _fileDal;

        public FileBLL(IFileDAL fileDal)
        {
            _fileDal = fileDal;
        }
        public async Task<byte[]> DownloadFileAsync(FileParameter parameter)
        {
            return await _fileDal.DownloadFileAsync(parameter);
        }

        public async Task<FileInfoDto> UploadFileAsync(FileParameter parameter, string filePath)
        {
            return await _fileDal.UploadFileAsync(parameter, filePath);
        }
    }
}
