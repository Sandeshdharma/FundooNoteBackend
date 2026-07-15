
using BusinessLayer.BusinessImplementation;
using BusinessLayer.BusinessLayerImplementation;
using BusinessLayer.Config;
using BusinessLayer.Helper;
using BusinessLayer.IBusiness;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using NLog;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.IRepository;
using RepositoryLayer.RepositoryImplementation;
using System.Text;

namespace FundooNotes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            //redis
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "FundooNotes";
            });
            builder.Services.AddScoped<RepositoryLayer.Redis.IRedisService, RepositoryLayer.Redis.RedisService>();


            // NLog Configuration
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            //Email
            builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SMTPSettings"));

            builder.Services.AddScoped<IEmailService, EmailService>();

            // Database
            builder.Services.AddDbContext<NotesContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Connection")));

            // Repository Layer
            builder.Services.AddScoped<IFundooRL, FunDooImplementationRL>();
            builder.Services.AddScoped<ILoginRL, LoginImpRl>();
            builder.Services.AddScoped<IRegisterRL, RegisterImpRL>();

            // Business Layer
            builder.Services.AddScoped<IFundooBL, FunDooImplementationBL>();
            builder.Services.AddScoped<IloginBL, LoginImplementationBL>();
            builder.Services.AddScoped<IRegisterBL, RegisterImplementationBL>();

            // Helpers
            builder.Services.AddScoped<PasswordHelper>();
            builder.Services.AddScoped<JwtTokenHelper>();

            //collaborator

            builder.Services.AddScoped<ICollaboratorRL, CollaboratorRL>();
            builder.Services.AddScoped<ICollaboratorBL, CollaboratorBL>();


            //label
            builder.Services.AddScoped<ILabelRL, LabelRL>();

            builder.Services.AddScoped<ILabelBL, LabelBL>();

        

            // JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                builder.Configuration["JwtSettings:Issuer"],

                            ValidAudience =
                                builder.Configuration["JwtSettings:Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        builder.Configuration["JwtSettings:Key"] ?? string.Empty))
                        };
                });

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization Header using Bearer Scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            });

            var app = builder.Build();

            Console.WriteLine("Application Build Successful");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

      

          

            app.UseCors("AngularPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            Console.WriteLine("Application Starting...");

            app.Run();

            Console.WriteLine("Application Ended");
        }
    }
}