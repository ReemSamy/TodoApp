using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.Application.Interfaces;
using TodoApp.Application.MappingProfiles;
using TodoApp.Application.Services;
using TodoApp.Domain.Repository;
using TodoApp.Infrastructure.Context;
using TodoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

#region Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
#endregion

#region Configure Repositories
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
#endregion

#region Configure Services
builder.Services.AddScoped<ITodoService, TodoService>();
#endregion

#region Configure AutoMapper
builder.Services.AddAutoMapper(typeof(autoMapper).Assembly);
#endregion

#region Add Authentication
var secretKey = Encoding.UTF8.GetBytes("your-very-secure-128-bit-secret-key");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});
#endregion

#region Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
#endregion

#region Add Authorization Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticate", policy => policy.RequireAuthenticatedUser());
});
#endregion

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
// Swagger setup for API documentation
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable Authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapControllers();

app.Run();
