using HrPlatform.DTOs;
using HrPlatform.Models;
using HrPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;


namespace HrPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly CandidateService _candidateService;

        public CandidatesController(CandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CandidateDto>>> GetAll()
        {
            var candidates = await _candidateService.GetAllAsync();
            return Ok(candidates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CandidateDto>> GetById(int id)
        {
            var candidate = await _candidateService.GetByIdAsync(id);
            if (candidate == null)
                return NotFound();
            return Ok(candidate);
        }

        [HttpPost]
        public async Task<ActionResult<CandidateDto>> Create(CreateCandidateDto createCandidateDto)
        {
            var candidate = await _candidateService.CreateAsync(createCandidateDto);
            return CreatedAtAction(nameof(GetById), new { id = candidate.Id }, candidate);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CandidateDto>> Update(int id, UpdateCandidateDto updateCandidateDto)
        {
            var candidate = await _candidateService.UpdateAsync(id, updateCandidateDto);

            if (candidate == null)
                return NotFound();
            return Ok(candidate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _candidateService.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPost("{candidateId}/skills/{skillId}")]
        public async Task<ActionResult> AddSkill(int candidateId, int skillId)
        {
            var result = await _candidateService.AddSkillToCandidateAsync(candidateId, skillId);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{candidateId}/skills/{skillId}")]
        public async Task<ActionResult> RemoveSkill(int candidateId, int skillId)
        {
            var result = await _candidateService.RemoveSkillFromCandidateAsync(candidateId, skillId);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<CandidateDto>>> Search([FromQuery] SearchCandidateDto searchDto)
        {
            var candidates = await _candidateService.SearchAsync(searchDto);
            return Ok(candidates);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CandidateDto>> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var candidate = await _candidateService.PatchCandidateAsync(id, updates);
            if (candidate == null) return NotFound();
            return Ok(candidate);
        }


    }


    }

