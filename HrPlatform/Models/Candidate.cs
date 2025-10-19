namespace HrPlatform.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string ContactNumber { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;

        public List<CandidateSkill> CandidateSkills { get; set; } = new();


    }
}
