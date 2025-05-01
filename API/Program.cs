using System.Text;
using System.Text.Json.Serialization;
using API.Controllers;
using API.Extensions;
using Application.DTOs;
using Application.Models;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddConfigurationService(builder.Configuration)
    .AddAuthServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices();

builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<FileUploadOption>(builder.Configuration.GetSection("FileUpload"));

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter like this => Bearer <your_jwt_token>",
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
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHttpClient();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "localhost:5151", 
            ValidAudience = "localhost:5261", 
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c5d4daef4df64b08b4ce630a38c0005e10a5953f519c2f1d143379784689fdd4"))
        };
    });

var minioSection = builder.Configuration.GetSection(nameof(MinioOptions));
var minioOptions = new MinioOptions();
minioSection.Bind(minioOptions);
builder.Services.Configure<MinioOptions>(minioSection);
builder.Services.AddMinio(config =>
{
    config
        .WithEndpoint(minioOptions.Host)
        .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
        .WithSSL(false)
        .Build();
});

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
await InitializeContext();
app.Run();
async Task InitializeContext()
{
    var scope  = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var usermanager = services.GetRequiredService<UserManager<AppUser>>();
        var rolemanager = services.GetRequiredService<RoleManager<AppRole>>();
        await ContextInitializer.Initialize(context, rolemanager, usermanager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}