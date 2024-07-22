using ITI_Material.Data;
using ITI_Material.IRepository;
using ITI_Material.IRepository.Repository;
using ITI_Material.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using System.Data;
using System.Text;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ITI_Material
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. 
            builder.Services.AddDbContext<ItiMateiral>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection")));
            builder.Services.AddIdentity<ApplicationUser , IdentityRole>().AddEntityFrameworkStores<ItiMateiral>();
            // [Autorized] to check the JWT token 
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = builder.Configuration["JWT:Issure"],
                            ValidateAudience = true,
                            ValidAudience = builder.Configuration["JWT:Auduance"],
                            IssuerSigningKey =
                                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),

                        };
                     });

            builder.Services.AddScoped<IServicesDepartements<Department>, ServicesDepartements>();
            builder.Services.AddScoped<IServicesDepartements<Employee>, ServicesEmployees>();
            
           

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen(c=>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ITI_Material", Version = "v1" })
            );

            builder.Services.AddSwaggerGen(swagger =>
            {
                            //This is to generate the Default UI of Swagger Documentation    
                            swagger.SwaggerDoc("v2", new OpenApiInfo
                            {
                                Version = "v1",
                                Title = "ASP.NET 5 Web API",
                                Description = " ITI Projrcy"
                            });

                            // To Enable authorization using Swagger (JWT)    
                            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                            {
                                Name = "Authorization",
                                Type = SecuritySchemeType.ApiKey,
                                Scheme = "Bearer",
                                BearerFormat = "JWT",
                                In = ParameterLocation.Header,
                                Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                            });
                            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                                        new string[] {}
                                        }
                            });
          
            });

            //---------------------------------------------------------------------
            // Declar new policy and it's validation
            // must set a policy to allow other platforms to response the data from provider
            // the project here"Provider" must be run to connect the server to other platforms 
            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("MyPolicy1", CorsPolicyBuilder =>
                {
                    CorsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();   
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ITI_Material v1"));
            }

            // To run the page html on other ports 
            //Customize policy open 1,2,3 declare ConfigureService method
            app.UseCors("MyPolicy1");

            // Add Role 

            var userManager = app.Services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
            var roleManager = app.Services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<ApplicationUser>>();

            //roleManager.CreateAsync(new Role { Name = "Admin" }).Wait();
            //roleManager.CreateAsync(new Role { Name = "SuperAdmin" }).Wait();

            // Create users
            //var adminUser = new User { UserName = "admin@example.com", Email = "admin@example.com" };
            //userManager.CreateAsync(adminUser, "Admin123!").Wait();
            //userManager.AddToRoleAsync(adminUser, "Admin").Wait();


            app.UseHttpsRedirection();

            app.UseAuthentication(); // Check JWT token
            app.UseAuthorization();

            app.UseStaticFiles(); // To support the html pages
            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
