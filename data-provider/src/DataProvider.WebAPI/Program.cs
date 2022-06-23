using DataProvider.Persistence.SQL.Context;
using DataProvider.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddCustomHealthChecks(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnectionString = builder.Configuration.GetConnectionString("SqlDatabase");
var serverVersion = ServerVersion.AutoDetect(sqlConnectionString);

builder.Services.AddDbContext<SqlContext>(options => options.UseMySql(sqlConnectionString, serverVersion));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomtHealthChecks();

app.MapControllers();

app.Run();
