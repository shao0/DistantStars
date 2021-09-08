using System.Collections.Generic;
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
    [Route("api/Role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }
        // GET: api/<RoleController>
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<RoleInfoDto>>> GetAllRoles([FromQuery] RoleParameter parameter)
        {
            IEnumerable<RoleInfo> roleInfos = await _roleService.GetRolesAsync(parameter);
            return Ok(_mapper.Map<IEnumerable<RoleInfoDto>>(roleInfos));
        }


        // POST api/<RoleController>
        [HttpPost("[action]")]
        public async Task<ActionResult<RoleInfoDto>> UpdateRole([FromBody] RoleInfoDto role)
        {
            var roleInfo = _mapper.Map<RoleInfo>(role);
            var newRole = await _roleService.UpdateRoleAsync(roleInfo);
            return _mapper.Map<RoleInfoDto>(newRole);
        }


        // POST api/<RoleController>
        [HttpPost("[action]")]
        public async Task<ActionResult<RoleInfoDto>> AddRole([FromBody] RoleInfoDto role)
        {
            var roleInfo = _mapper.Map<RoleInfo>(role);
            var newRole = await _roleService.AddRoleAsync(roleInfo);
            return _mapper.Map<RoleInfoDto>(newRole);
        }

        // DELETE api/<RoleController>/5
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return Ok();
        }
    }
}
