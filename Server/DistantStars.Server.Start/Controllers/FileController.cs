using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Enums;
using DistantStars.Common.DTO.Parameters;
using DistantStars.Server.IService.Systems;
using DistantStars.Server.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace DistantStars.Server.Start.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public FileController(IFileService fileService, IMapper mapper, IConfiguration configuration)
        {
            _fileService = fileService;
            _mapper = mapper;
            _configuration = configuration;
        }
        async Task<string> GetMD5(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var md5 = MD5.Create();
            var bytes = await md5.ComputeHashAsync(stream);
            return BitConverter.ToString(bytes).ToLower().Replace("-", string.Empty);
        }
        
        [HttpGet("[action]")]
        public async Task<ActionResult> DownloadFile([FromQuery] FileParameter parameter)
        {
            var files = await _fileService.QueryAsync<FileInfoModel>(f =>
         f.FileType == parameter.FileType && f.MD5 == parameter.MD5);
            if (files?.Count() > 0)
            {
                var file = files.First();
                var path = _configuration["FilePath"];
                path = $"{path}{file.FilePath}";
                if (System.IO.File.Exists(path))
                {
                    var bytes = await System.IO.File.ReadAllBytesAsync(path);
                    return File(bytes, file.ContentType);
                }
            }
            return Ok();
        }
        
        [HttpPost("[action]")]
        public async Task<ActionResult<FileInfoDto>> UploadFile([FromQuery] FileParameter parameter, IFormFile file)
        {
            var md5 = await GetMD5(file);
            FileInfoModel model;
            var fileModels = await _fileService.QueryAsync<FileInfoModel>(f => f.MD5 == md5 && f.FileType == parameter.FileType);
            if (fileModels == null || !fileModels.Any())
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{md5}{extension}";
                string fileDirectory;
                switch (parameter.FileType)
                {
                    case FileType.Image:
                        fileDirectory = "Images";
                        break;
                    case FileType.Text:
                        fileDirectory = "Texts";
                        break;
                    case FileType.Video:
                        fileDirectory = "Videos";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var filePath = $"{_configuration["FilePath"]}\\{fileDirectory}";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = $"{filePath}\\{fileName}";

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var fileInfoModel = new FileInfoModel();
                fileInfoModel.ContentType = file.ContentType;
                fileInfoModel.FileType = parameter.FileType;
                fileInfoModel.Extension = extension;
                fileInfoModel.FilePath = $"{fileDirectory}\\{fileName}";
                fileInfoModel.MD5 = md5;
                model = await _fileService.InsertAsync(fileInfoModel);
            }
            else
            {
                model = fileModels.First();
            }
            var fileInfoDto = _mapper.Map<FileInfoDto>(model);
            return Ok(fileInfoDto);
        }

    }
}
