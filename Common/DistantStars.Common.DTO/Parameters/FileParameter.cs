using System;
using System.Collections.Generic;
using System.Text;
using DistantStars.Common.DTO.Enums;

namespace DistantStars.Common.DTO.Parameters
{
    public class FileParameter : ParameterBase
    {
        public string MD5 { get; set; }
        public FileType FileType { get; set; }
    }
}
