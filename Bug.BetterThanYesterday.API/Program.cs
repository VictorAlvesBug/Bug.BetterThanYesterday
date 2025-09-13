using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Users;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DatabaseConfig>(
    builder.Configuration.GetSection(nameof(DatabaseConfig)));
builder.Services.AddSingleton<IDatabaseConfig>(sp =>
    sp.GetRequiredService<IOptions<DatabaseConfig>>().Value);

builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
