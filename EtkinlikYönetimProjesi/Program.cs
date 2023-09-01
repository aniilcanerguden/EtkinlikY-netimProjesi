
using EtkinlikYönetimProjesi.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using EtkinlikYönetimProjesi.IRepository.Repository;
using EtkinlikYönetimProjesi.IRepository;
using System.Text.Json;
using EtkinlikYönetimProjesi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<ICompanyRepository,CompanyRepository>();

//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:SecretKey").Value!);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5075") // Ýzin verilecek alan adý
                   .AllowAnyHeader()
                   .AllowAnyMethod().AllowAnyOrigin()
                 ;
        });
}); var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {

        JwtSettings tokenOption = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = tokenOption.Issuer,
            ValidAudience = tokenOption.Audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey))
        };
    });
builder.Services.AddControllers().AddXmlSerializerFormatters() // XML dönüþümünü ekler
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // JSON dönüþümü ayarlarý
    }).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.MapControllers();


app.Run();
