global using RentCarSystem.Migrations.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RentCarSystem.Reponsitories;
using RentCarSystem.Mappings;
using RentCarSystem.Models.Domain;
using System.Reflection.Emit;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RentCarSystem.Authorization;


namespace RentCarSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Rent Car System API", Version = "v1" }); //t?o ra tài li?u API  và version
                //Add security configution
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header, // v? trí token trong HTTP header
                    Type = SecuritySchemeType.ApiKey, //JWT s? truy?n nh? là m?t API key trong quá trình Request
                    Scheme = JwtBearerDefaults.AuthenticationScheme // S? d?ng chu?n Bearer Token
                });

                //Call endpoint will have JWT
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, // Tham chi?u t?i ??nh ngh?a b?o m?t ?ã ??nh ngh?a tr??c ?ó
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "Bearer",
                            Name = JwtBearerDefaults.AuthenticationScheme, //Bearer
                            In = ParameterLocation.Header //Xác ??nh token s? ???c tìm th?y trong HTTP request
                        },
                        new List <string>()
                    }
                });
            });


            //Add DBContext dependency injection
            builder.Services.AddDbContext<RentCarSystemContext>(option =>
                                                            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<ITokenReponsitory, TokenReponsitory>();
            builder.Services.AddScoped<IRegisterReponsitory, RegisterReponsitory>();
            builder.Services.AddScoped<ILoginReponsitory, LoginReponsitory>();
            builder.Services.AddScoped<IAdminReponsitory, AdminReponsitory>();
            builder.Services.AddScoped<IAuthorizationHandler, BusinessApprovalHandler>();
            builder.Services.AddScoped<IVehicleReponsitory, VehicleReponsitory>();
            builder.Services.AddScoped<IMotorRepository, MotorRepository>();
            builder.Services.AddScoped<ICarReponsitory, CarReponsitory>();

            //Add Hashpassword
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
               

            //Add mapping
            builder.Services.AddAutoMapper(typeof(AutomapperProfile));


            ////Set-up identity
            builder.Services.AddIdentityCore<IdentityUser>() //Them dich vu cho ng dung`
                .AddRoles<IdentityRole>() //vai tro`
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("RentCarSystem") // Manage Token
                .AddEntityFrameworkStores<RentCarSystemContext>()
                .AddDefaultTokenProviders(); //them default tokens

            //Cau hinh tuy chon Identity
            builder.Services.Configure<IdentityOptions>(options =>
            {
                //Cac thong so can co trong password
                options.Password.RequireDigit = false; //number 
                options.Password.RequireLowercase = false; //chu thuong
                options.Password.RequireNonAlphanumeric = false; //khong phai ky tu hoac so
                options.Password.RequireUppercase = false; // chu hoa
                options.Password.RequiredLength = 6; //length toi thieu
                options.Password.RequiredUniqueChars = 1; //Yeu cau ky tu duy nhat
            });


            //JWT configrution
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //them dich vu xac thuc cho JWT
                .AddJwtBearer(options => //Cau hinh cac tham so cho JWT 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //Kiem tra nha phat hanh co hop le khong
                        ValidateAudience = true, //token cua ng dung
                        ValidateLifetime = true, //Thoi gian song token
                        ValidateIssuerSigningKey = true, //signature cua nha phat hanh
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        
                        //dinh nghia chu ky lay tu application
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });


            //add Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value.ToLower() == "admin")));

                options.AddPolicy("Service", policy =>
                     policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value.ToLower() == "service")));
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("BusinessWithAcceptStatus", policy =>
                    policy.Requirements.Add(new BusinessApprovalRequirement()));
            });


            var app = builder.Build();

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
}
