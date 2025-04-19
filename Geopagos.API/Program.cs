using Geopagos.Presenter;
using Geopagos.Repository;
using Geopagos.Repository.Repositories;
using Geopagos.Services;
using Geopagos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
    });

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Dependency Injection  
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<ITournamentPresenter, TournamentPresenter>();

// Register the repositories
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();

// Register DbContext with SQL Server (you can switch to other providers if needed)  
builder.Services.AddDbContext<GeopagosDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
