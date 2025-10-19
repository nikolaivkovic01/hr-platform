using System.ComponentModel.DataAnnotations;

namespace HrPlatform.DTOs
{
    public class CandidateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string ContactNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public List<SkillDto> Skills { get; set; } = new();


    }

    public class CreateCandidateDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required, Phone]
        public string ContactNumber { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public List<int> SkillIds { get; set; } = new();

    }
    public class UpdateCandidateDto
    {
        
        public string Name { get; set; } = string.Empty;
        
        public DateOnly DateOfBirth { get; set; }
        
        public string ContactNumber { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
    }

    public class SkillDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class CreateSkillDto
    {
        public string Name { get; set; } = string.Empty;

    }
    public class SearchCandidateDto
    {
        public string? Name { get; set; }
        public List<string>? Skills { get; set; }
    }
}
