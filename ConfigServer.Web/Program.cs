using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ConfigServer.Domain.Interfaces;
using ConfigServer.Infrastructure.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using ConfigServer.Infrastructure.Repositories;
using Microsoft.Extensions.FileProviders;
using ConfigServer.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "YourApp.Cookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "configServer",
        ValidAudience = "ConfigServerAPI",
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere12345678987654321Sepideh"))
    };
});

// Add DbContext service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("ConfigServer.Web"))
);

// Add MVC services
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Config Server API",
        Version = "v1",
        Description = "API documentation for Config Server",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com",
        }
    });

    // JWT Bearer Security Definition for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add custom Operation Filter to handle file uploads

});

// Add services to DI container
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<TokenHelper>();
//builder.Services.AddScoped<IFileService, FileService>();
//builder.Services.AddScoped<IConfigService, ConfigService>();
builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/uploads"
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Config Server API v1");
        c.RoutePrefix = ""; // Set Swagger UI at the root
    });
}

app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();








