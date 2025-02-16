using System.Globalization;
using System.Security.Claims;
using System.Text;
using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EventsManagerWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null); ;
            builder.Services.AddRazorPages();
            builder.Services.AddSession();

            builder.Services.AddMemoryCache(); // Or IDistributedCache (e.g., Redis)

            builder.Services.AddTransient<JWTManager>();
            builder.Services.AddOptions();

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Secret").ToString())),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidAudience = config["Jwt:Issuer"],
                        ValidIssuer = config["Jwt:Audience"]
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseFileServer();

            app.UseRouting();

            app.UseAntiforgery();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapRazorPages();

            app.MapGet("/", () => Results.Redirect("/Home"));

            app.MapPost("/auth", (User user, JWTManager jwtManager)
                => jwtManager.GetToken(user));

            app.MapGet("/signin", () => "User Authenticated Successfully!").RequireAuthorization();

            app.Run();

        }
    }
}
