using BACKEND.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  //  .AddJwtBearer(options => { /* Configuración JWT */ });

builder.Services.AddCors(options =>
    options.AddPolicy("Frontend", p => p.WithOrigins("...").AllowAnyMethod()));

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(Configuration.GetConnectionString("ApplicationDbContext"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Frontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
