
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Security.Claims;
using System.Text;
using TaskManagementApi_CodingChallenge.Data;
using TaskManagementApi_CodingChallenge.Repository;
using TaskManagementApi_CodingChallenge.Services;

namespace TaskManagementApi_CodingChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            builder.Services.AddDbContext<TaskDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));


            // Load configuration from appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Log the configuration values to ensure they are loaded correctly
            Console.WriteLine("Issuer: " + builder.Configuration["Jwt:Issuer"]);
            Console.WriteLine("Audience: " + builder.Configuration["Jwt:Audience"]);
            Console.WriteLine("Key: " + builder.Configuration["Jwt:Key"]);

            // Configure JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = builder.Configuration["Jwt:Key"];

        // Check if the configuration values are null or empty
        if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("JWT configuration values are missing in appsettings.json.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        };
    });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Set up Serilog logging to write to a file
            Log.Logger = new LoggerConfiguration()
               .WriteTo.Console()  // Optionally log to the console as well
               .WriteTo.File("D:\\LogFiles\\TMLogs.txt", rollingInterval: RollingInterval.Day)  // Write logs to a file, with daily log file rotation
               .CreateLogger();

            builder.Host.UseSerilog(); // Use Serilog for logging


            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "QuickTask", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(); // Add authentication services
            builder.Services.AddAuthorization();  // Add authorization services

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
