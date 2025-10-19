using Microsoft.EntityFrameworkCore;
using HrPlatform.Data;
using HrPlatform.DTOs;
using HrPlatform.Models;


namespace HrPlatform.Services
{
    public class SkillService
    {
        private readonly HrPlatformContext _context;

        public SkillService(HrPlatformContext context)
        {
            _context = context;
        }

        public async Task<List<SkillDto>> GetAllAsync()
        {
            return await _context.Skills
                .Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .ToListAsync();
        }

        public async Task<SkillDto> CreateAsync (CreateSkillDto createSkillDto)
        {
            var skill = new Skill
            {
                Name = createSkillDto.Name
            };

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return new SkillDto
            {
                Id = skill.Id,
                Name = skill.Name
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return false;

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
