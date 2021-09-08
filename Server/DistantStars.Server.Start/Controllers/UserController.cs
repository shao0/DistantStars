using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;
using DistantStars.Server.IService.Systems;
using DistantStars.Server.Model.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistantStars.Server.Start.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMenuService menuService, IMapper mapper)
        {
            _userService = userService;
            _menuService = menuService;
            _mapper = mapper;
        }
        private string GetMd5(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            var bytes = Encoding.Default.GetBytes(value);
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }
        private string GetMd5Password(string username, string password)
        {
            return GetMd5($"{GetMd5(password)}|{username}");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetAllUsers([FromQuery] UserParameter parameter)
        {
            var users = await _userService.GetAllUsers(parameter);
            var dtos = _mapper.Map<IEnumerable<UserInfoDto>>(users);
            return Ok(dtos);
        }
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            await _userService.DeleteAsync<UserInfo>(id);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserInfoDto>> UpdateUser([FromBody] UserInfoDto dto)
        {
            var userInfo = _mapper.Map<UserInfo>(dto);
            if (dto.ModifyPassword) userInfo.UserPassword = GetMd5Password(userInfo.UserAccount, userInfo.UserPassword);
            var info = await _userService.UpdateAsync(userInfo);
            dto = _mapper.Map<UserInfoDto>(info);
            return dto;
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<UserInfoDto>> Login([FromBody] LoginParameter parameter)
        {
            var md5Password = GetMd5Password(parameter.UserAccount, parameter.Password);
            var userInfos = await _userService.QueryAsync<UserInfo>(user => user.UserAccount == parameter.UserAccount && user.UserPassword == md5Password);
            if (userInfos?.Count() > 0)
            {
                var userInfo = userInfos.First();
                var menus = await _menuService.GetMenusByUserIdAsync(userInfo.Id);
                var userInfoDto = _mapper.Map<UserInfoDto>(userInfo);
                userInfoDto.Menus = _mapper.Map<IEnumerable<MenuInfoDto>>(menus);
                return userInfoDto;
            }
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<UserInfoDto>> SignUp([FromBody] UserInfoDto userInfo)
        {
            userInfo.UserPassword = GetMd5Password(userInfo.UserAccount, userInfo.UserPassword);
            var user = _mapper.Map<UserInfo>(userInfo);
            var info = await _userService.InsertAsync(user);
            var userInfoDto = _mapper.Map<UserInfoDto>(info);
            return userInfoDto;
        }

    }
}
