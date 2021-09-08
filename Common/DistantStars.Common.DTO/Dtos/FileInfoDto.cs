using DistantStars.Common.DTO.Enums;

namespace DistantStars.Common.DTO.Dtos
{
    public class FileInfoDto
    {
        public string MD5 { get; set; }

        public string FilePath { get; set; }

        public FileType FileType { get; set; }

        public string Extension { get; set; }
    }
}
