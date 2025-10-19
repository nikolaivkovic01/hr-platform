using HrPlatform.DTOs;
using HrPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace HrPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {

        private readonly SkillService _skillService;

        public SkillsController(SkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SkillDto>>> GetAll()
        {
            var skills = await _skillService.GetAllAsync();
            return Ok(skills);
        }

        [HttpPost]
        public async Task<ActionResult<SkillDto>> Create(CreateSkillDto createSkillDto)
        {
            var skill = await _skillService.CreateAsync(createSkillDto);
            return Ok(skill);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _skillService.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

    }
}
