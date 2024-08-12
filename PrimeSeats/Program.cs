using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrimeSeats;
using PrimeSeats_Data.Data;
using PrimeSeats_Data.Repository;
using PrimeSeats_Data.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Replace with your Angular app's URL
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                    //.AllowCredentials("Access-Control-Allow-Origin");
                });
        });
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        //jwt -------
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience= true,
                ValidateLifetime = true,
                ValidIssuer  = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        //git test-----
        //jwt end ----------------

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOption>();


        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        var app = builder.Build();

        app.UseCors("AllowSpecificOrigin");
        app.UseRouting();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}