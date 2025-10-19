using HrPlatform.Data;
using HrPlatform.DTOs;
using HrPlatform.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;


namespace HrPlatform.Services
{
    public class CandidateService
    {
        private readonly HrPlatformContext _context;

        public CandidateService(HrPlatformContext context)
        {
            _context = context;
        }

        public async Task<List<CandidateDto>> GetAllAsync()
        {
            var candidates = await _context.Candidates
                .Include(c => c.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .ToListAsync();

            return candidates.Select(c => new CandidateDto
            {
                Id = c.Id,
                Name = c.Name,
                DateOfBirth = c.DateOfBirth,
                ContactNumber = c.ContactNumber,
                Email = c.Email,
                Skills = c.CandidateSkills.Select(cs => new SkillDto
                {
                    Id = cs.Skill.Id,
                    Name = cs.Skill.Name,

                }).ToList()
            }).ToList();
        }

        public async Task<CandidateDto?> GetByIdAsync(int id)
        {
            var candidate = await _context.Candidates
                .Include(c => c.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidate == null)
                return null;

            return new CandidateDto
            {
                Id = candidate.Id,
                Name = candidate.Name,
                DateOfBirth = candidate.DateOfBirth,
                ContactNumber = candidate.ContactNumber,
                Email = candidate.Email,
                Skills = candidate.CandidateSkills.Select(cs => new SkillDto
                {
                    Id = cs.Skill.Id,
                    Name = cs.Skill.Name
                }).ToList()
            };
        }

        public async Task<CandidateDto> CreateAsync(CreateCandidateDto createCandidateDto)
        {
            var candidate = new Candidate
            {
                Name = createCandidateDto.Name,
                DateOfBirth = createCandidateDto.DateOfBirth,
                ContactNumber = createCandidateDto.ContactNumber,
                Email = createCandidateDto.Email
            };


            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            if (createCandidateDto.SkillIds.Any())
            {
                foreach (var skillId in createCandidateDto.SkillIds)
                {
                    var skillExists = await _context.Skills.AnyAsync(s => s.Id == skillId);

                    if (skillExists)
                    {
                        _context.CandidatesSkills.Add(new CandidateSkill
                        {
                            CandidateId = candidate.Id,
                            SkillId = skillId
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }

            return await GetByIdAsync(candidate.Id);
        }

        public async Task<CandidateDto?> UpdateAsync (int id,UpdateCandidateDto updateCandidateDto)
        {
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
                return null;

            candidate.Name = updateCandidateDto.Name;
            candidate.DateOfBirth = updateCandidateDto.DateOfBirth;
            candidate.ContactNumber = updateCandidateDto.ContactNumber;
            candidate.Email = updateCandidateDto.Email;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync (int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
                return false;

            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSkillToCandidateAsync(int candidateId, int skillId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);

            var skill = await _context.Skills.FindAsync(skillId);

            if (candidate == null || skill == null)
                return false;

            var existing = await _context.CandidatesSkills
                .FirstOrDefaultAsync(cs => cs.CandidateId == candidateId && cs.SkillId == skillId);

            if (existing != null)
                return true;

            _context.CandidatesSkills.Add(new CandidateSkill
            {
                CandidateId = candidateId,
                SkillId = skillId
            });

            await _context.SaveChangesAsync();
            return true;
       }

        public async Task<bool> RemoveSkillFromCandidateAsync (int candidateId, int skillId)
        {
            var candidateSkill = await _context.CandidatesSkills
                .FirstOrDefaultAsync(cs => cs.CandidateId == candidateId && cs.SkillId == skillId);

            if (candidateSkill == null) 
                return false;

            _context.CandidatesSkills.Remove(candidateSkill);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<CandidateDto>> SearchAsync (SearchCandidateDto searchDto)
        {
            var query = _context.Candidates
                .Include(c => c.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchDto.Name))
            {
                query = query.Where(c => c.Name.Contains(searchDto.Name));

            }

            if (searchDto.Skills != null && searchDto.Skills.Any())
            {
                foreach (var skillName in searchDto.Skills)
                {
                    query = query.Where(c => c.CandidateSkills
                    .Any(cs => cs.Skill.Name.Contains(skillName)));
                      
                }
            }

            var candidates = await query.ToListAsync();

            return candidates.Select(c => new CandidateDto
            {
                Id = c.Id,
                Name = c.Name,
                DateOfBirth = c.DateOfBirth,
                ContactNumber = c.ContactNumber,
                Email = c.Email,
                Skills = c.CandidateSkills.Select(cs => new SkillDto
                {
                    Id = cs.Skill.Id,
                    Name = cs.Skill.Name
                }).ToList()
            }).ToList();
        }
        
        public async Task<CandidateDto?> PatchCandidateAsync(int id, Dictionary<string, object> updates)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null) return null;

            foreach (var update in updates)
            {
                switch (update.Key.ToLower())
                {
                    case "name" when update.Value is string name:
                        candidate.Name = name;
                        break;
                    case "dateofbirth" when update.Value is string dateStr:
                        if (DateOnly.TryParse(dateStr, out var date))
                            candidate.DateOfBirth = date;
                        break;
                    case "contactnumber" when update.Value is string contact:
                        candidate.ContactNumber = contact;
                        break;
                    case "email" when update.Value is string email:
                        candidate.Email = email;
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }
    }
}
