namespace HrPlatform.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<CandidateSkill> CandidateSkills { get; set; } = new();

    }
}
