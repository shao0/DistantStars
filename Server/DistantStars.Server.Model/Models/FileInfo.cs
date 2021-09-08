using System.ComponentModel.DataAnnotations;
using DistantStars.Common.DTO.Enums;

namespace DistantStars.Server.Model.Models
{
    public class FileInfoModel
    {
        [Key]
        public string MD5 { get; set; }

        public string FilePath { get; set; }

        public string ContentType { get; set; }

        public FileType FileType { get; set; }

        public string Extension { get; set; }
    }
}
