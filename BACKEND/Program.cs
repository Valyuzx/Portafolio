using BACKEND.BuissnesLayer;
using BACKEND.Helpers;
using BACKEND.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration;
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<RefreshTokenService>();
//builder.Services.AddHostedService<BlackListCleanupService>();

builder.Services.AddControllers();
builder.Services.AddScoped<IAccountBL, AccountBL>();

builder.Services.AddCors(options =>
    options.AddPolicy("Frontend", p => p.WithOrigins("...").AllowAnyMethod()));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("ApplicationDbContext"))
);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Portfolio API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT. Ejemplo: Bearer {tu_token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }},
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer("Bearer", jwtOptions =>
  {
      jwtOptions.Authority = builder.Configuration["Jwt:Authority"];
      jwtOptions.Audience = builder.Configuration["Jwt:Audience"];
      jwtOptions.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          ValidateIssuer = true,
          ValidateAudience = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
          ValidAudiences = builder.Configuration.GetSection("Jwt:ValidAudiences").Get<string[]>(),
          ValidIssuers = builder.Configuration.GetSection("Jwt:ValidIssuers").Get<string[]>(),
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero
      };

      jwtOptions.MapInboundClaims = false;
  });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
