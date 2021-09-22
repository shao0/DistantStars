using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Server.IService.Systems;
using DistantStars.Server.Model.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistantStars.Server.Start.Controllers
{
    [Route("api/Menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuController(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }
        
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<MenuInfoDto>>> GetAllMenus()
        {
            return Ok(_mapper.Map<IEnumerable<MenuInfoDto>>(await _menuService.GetAllMenusAsync()));
        }
        
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<IEnumerable<MenuInfoDto>>> GetMenusByRoleId(int id)
        {
            return Ok(_mapper.Map<IEnumerable<MenuInfoDto>>(await _menuService.GetMenusByRoleIdAsync(id)));
        }
        
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<IEnumerable<MenuInfoDto>>> GetMenusByUserId(int id)
        {
            return Ok(_mapper.Map<IEnumerable<MenuInfoDto>>(await _menuService.GetMenusByUserIdAsync(id)));
        }
        
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<IEnumerable<MenuInfoDto>>> DeleteMenu(int id)
        {
            await _menuService.DeleteAsync<MenuInfo>(id);
            return Ok();
        }
        
        [HttpPost("[action]")]
        public async Task<ActionResult<MenuInfoDto>> AddMenu([FromBody] MenuInfoDto dto)
        {
            var menu = _mapper.Map<MenuInfo>(dto);
            return _mapper.Map<MenuInfoDto>(await _menuService.AddMenuAsync(menu));
        }
        
        [HttpPost("[action]")]
        public async Task<ActionResult<MenuInfoDto>> UpdateMenu([FromBody] MenuInfoDto dto)
        {
            if (dto.MenuId == 0)
            {
                return NotFound();
            }
            var menu = _mapper.Map<MenuInfo>(dto);
            await _menuService.UpdateAsync(menu);
            return _mapper.Map<MenuInfoDto>(await _menuService.UpdateAsync(menu));
        }
    }
}
