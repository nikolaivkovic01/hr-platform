using HrPlatform.Data;
using HrPlatform.DTOs;
using HrPlatform.Models;
using HrPlatform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HrPlatform.Tests
{
    public class CandidateServiceTest
    {

        [Fact]
        public async Task GetAllAsync_ReturnAllCandidates()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("GetAllTestDb")
                .Options;

            using var context = new HrPlatformContext(options);

            context.Candidates.Add(new Candidate { 
            
                Id = 1,
                Name = "Test Candidate",
                DateOfBirth = new DateOnly(2000,01,01),
                Email = "test@test.com",
                ContactNumber = "+381615548862"
            });

            await context.SaveChangesAsync();

            var service = new CandidateService(context);

            var result = await service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("Test Candidate", result[0].Name);
            Assert.Equal("test@test.com", result[0].Email);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCandidate_WhenExists()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("GetByIdTest")
                .Options;

            using var context = new HrPlatformContext(options);

            context.Candidates.Add(new Candidate {

                Id = 1,
                Name = "Test",
                DateOfBirth = new DateOnly(2000, 01, 01),
                Email = "test@test.com",
                ContactNumber = "+381615548862"

            });

            await context.SaveChangesAsync();

            var service = new CandidateService(context);

            var result = await service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal(1, result.Id);

        }

        [Fact]

        public async Task CreateAsync_CreatesCandidate_WithValidData()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_Create")
                .Options;

            using var context = new HrPlatformContext(options);
            var service = new CandidateService(context);

            var dto = new CreateCandidateDto
            {
                Name = "New Candidate",
                DateOfBirth = new DateOnly(2000,01,01),
                Email = "new@test.com",
                ContactNumber = "+381624853315"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("New Candidate", result.Name);
            Assert.Equal("new@test.com", result.Email);

            var fromDb = await context.Candidates.FirstAsync();
            Assert.Equal("New Candidate", fromDb.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenCandidateNoExists()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext> ()
                .UseInMemoryDatabase("TestDb_GetById_Null")
                .Options;

            using var context = new HrPlatformContext(options);
            var service = new CandidateService(context);

            var result = await service.GetByIdAsync(999);

            Assert.Null(result);


        }

        [Fact]
        public async Task UpdateAsync_UpdatesCandidate_WhenExists()
        {
            var option = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_Update")
                .Options;

            using var context = new HrPlatformContext (option);

            context.Candidates.Add(new Candidate
            {
                Id = 1,
                Name = "Test",
                DateOfBirth = new DateOnly(2000, 01, 01),
                Email = "test@test.com",
                ContactNumber = "+381111111121"
            });

            await context.SaveChangesAsync();

            var service = new CandidateService (context);

            var updateDto = new UpdateCandidateDto
            {
                Name = "new test",
                DateOfBirth = new DateOnly(1999, 12, 31),
                Email = "new@test.com",
                ContactNumber = "+98122666546"
            };

            var result = await service.UpdateAsync(1, updateDto);

            Assert.NotNull(result);
            Assert.Equal("new test", result.Name);
            Assert.Equal("new@test.com", result.Email);
            Assert.Equal(new DateOnly(1999, 12, 31), result.DateOfBirth);

        }

        [Fact]
        
        public async Task DeleteAsync_DeletesCandidate_WhenExists()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_Delete")
                .Options;

            using var context = new HrPlatformContext(options);

            context.Candidates.Add(new Candidate
            {
                Id = 1,
                Name = "To Delete",
                DateOfBirth = new DateOnly(2000, 01, 01),
                Email = "delete@test.com",
                ContactNumber = "+381624448568"
            });

            await context.SaveChangesAsync();

            var service = new CandidateService (context);

            var result = await service.DeleteAsync(1);

            Assert.True(result);
            var fromDb = await context.Candidates.FindAsync(1);
            Assert.Null(fromDb);

        }

        [Fact]

        public async Task AddSkillToCandidateAsync_AddsSkill()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_AddSkill")
                .Options;

            using var context = new HrPlatformContext (options);

            context.Candidates.Add(new Candidate
            {
                Id = 1,
                Name = "Test Candidate",
                DateOfBirth = new DateOnly(2000,01,01),
                Email = "test@test.com",
                ContactNumber = "+381215554963"

            });

            context.Skills.Add(new Skill
            {
                Id = 1,
                Name = "C# Programming"

            });

            await context.SaveChangesAsync();

            var service = new CandidateService (context);

            var result = await service.AddSkillToCandidateAsync(1, 1);

            Assert.True(result);
            var candidateSkill = await context.CandidatesSkills
                .FirstOrDefaultAsync(cs => cs.CandidateId == 1 && cs.SkillId ==1);

            Assert.NotNull(candidateSkill);
        }

        [Fact]

        public async Task RemoveSkillFromCandidateAsync_RemovesSkill()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_RemoveSkill")
                .Options;

            using var context = new HrPlatformContext(options);

            context.Candidates.Add(new Candidate
            {
                Id = 1,
                Name = "Test Candidate",
                DateOfBirth = new DateOnly(2000,01,01),
                Email = "test@test.com",
                ContactNumber = "+38122645895"

            });

            context.Skills.Add(new Skill
            {
                Id=1,
                Name = "C# programming"
            });

            context.CandidatesSkills.Add(new CandidateSkill
            {
                CandidateId = 1,
                SkillId = 1
            });

            await context.SaveChangesAsync();

            var service = new CandidateService (context);

            var result = await service.RemoveSkillFromCandidateAsync(1, 1);

            Assert.True(result);
            var candidateSkill = await context.CandidatesSkills
                .FirstOrDefaultAsync(cs => cs.CandidateId ==1 && cs.SkillId ==1);
            Assert.Null(candidateSkill);
        }

        [Fact]
        public async Task SearchAsync_ReturnsFilteredResults()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_Search")
                .Options;

            using var context = new HrPlatformContext(options);

            var candidate1 = new Candidate
            {
                Id = 1,
                Name = "Marko Markovic",
                DateOfBirth = new DateOnly(2000,01,01),
                Email = "marko@test.com",
                ContactNumber = "+38126555486"
            };

            var candidate2 = new Candidate
            {
                Id = 2,
                Name = "Petar Petrovic",
                DateOfBirth = new DateOnly(1995, 01, 01),
                Email = "petar@test.com",
                ContactNumber = "+381624456254"
            };

            context.Candidates.AddRange(candidate1, candidate2);
            await context.SaveChangesAsync();

            var service = new CandidateService(context);
            var searchDto = new SearchCandidateDto { Name = "Marko" };

            var result = await service.SearchAsync(searchDto);

            Assert.Single(result);
            Assert.Equal("Marko Markovic", result[0].Name);
        }
    }
}
