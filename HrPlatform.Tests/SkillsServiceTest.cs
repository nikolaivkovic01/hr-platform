using HrPlatform.Data;
using HrPlatform.DTOs;
using HrPlatform.Models;
using HrPlatform.Services;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.Marshalling;

namespace HrPlatform.Tests
{
    public class SkillsServiceTest
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllSkills()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_GetAllSkills")
                .Options;

            using var context = new HrPlatformContext(options);

            context.Skills.AddRange(

                new Skill { Id = 1, Name = "C#"},
                new Skill { Id = 2, Name = "Java"}

                );
            await context.SaveChangesAsync();

            var service = new SkillService(context);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, s => s.Name == "C#");
            Assert.Contains(result, s => s.Name == "Java");

            

        }

        [Fact]
        public async Task CreateAsync_CreatesSkill_WithValidData()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_CreateSkill")
                .Options;

            using var context = new HrPlatformContext(options);

            var service = new SkillService(context);

            var dto = new CreateSkillDto { Name = "New Skill" };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("New Skill", result.Name);
            Assert.True(result.Id > 0);

            var fromDb = await context.Skills.FirstAsync();
            Assert.Equal("New Skill", fromDb.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesSkill_WhenExists()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_DeleteSkill")
                .Options;

            using var context = new HrPlatformContext (options);

            context.Skills.Add(new Skill
            {
                Id = 1,
                Name = "To Delete"
            });

            await context.SaveChangesAsync();

            var service = new SkillService(context);

            var result = await service.DeleteAsync(1);

            Assert.True(result);
            var fromDb = await context.Skills.FindAsync(1);
            Assert.Null(fromDb);

        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenSkillNotExists()
        {
            var options = new DbContextOptionsBuilder<HrPlatformContext>()
                .UseInMemoryDatabase("TestDb_DeleteSkill_NotFound")
                .Options;

            using var context = new HrPlatformContext(options);
            var service = new SkillService (context);

            var result = await service.DeleteAsync(999);

            Assert.False(result);
        }
    }
}
