using Microsoft.EntityFrameworkCore;
using HrPlatform.Data;
using HrPlatform.Services;
using Microsoft.Extensions.Logging.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HrPlatformContext>(options =>
options.UseSqlite("Data Source=hrplatform.db"));

builder.Services.AddScoped<CandidateService>();
builder.Services.AddScoped<SkillService>();

builder.Services.AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HrPlatformContext>();
    context.Database.EnsureCreated();
}

app.Run();
