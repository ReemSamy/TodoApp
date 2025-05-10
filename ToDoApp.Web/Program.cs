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
        sqlOptions => sqlOptions.EnableRetryOnFailure()));
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

#region Configure Authentication
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
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});
#endregion

#region Configure Authorization Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticate", policy => policy.RequireAuthenticatedUser());
});
#endregion

#region Configure MVC
builder.Services.AddControllersWithViews();
#endregion

var app = builder.Build();

#region Configure Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ToDo}/{action=Index}");

app.Run();
#endregion
