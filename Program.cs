using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateManagementAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

// Authorization
builder.Services.AddAuthorization();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    ));

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
        policy =>
        {
            policy.WithOrigins("http://localhost:5232")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Development tools
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// HTTPS
app.UseHttpsRedirection();

// CORS MUST come before Authentication/Authorization
app.UseCors("AllowMVC");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();