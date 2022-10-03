using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using System.Text;
using WorkbenchAPI;
using WorkbenchAPI.Authorization;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Middleware;
using WorkbenchAPI.Models;
using WorkbenchAPI.Models.Validators;
using WorkbenchAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(); //validacja
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddScoped<IPasswordHasher<Client>, PasswordHasher<Client>>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

//autentykacja

var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings); //wykorzystany w serwisie do logowania
//konfiguracja autentykacji
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

//wlasna polityka autoryzacji
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Polish"));
    options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
});
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); //serwisy automapera
builder.Services.AddScoped<IValidator<RegisterClientDto>, RegiresterClientDtoValidator>(); //validacja

builder.Services.AddScoped<ShopSeeder>();

builder.Services.AddDbContext<WorkbenchDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("WorkbenchDbConnection")));

builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<ShopSeeder>();
seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//sprawdzenie kazdej requesta przez autentykacje
app.UseAuthentication();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

app.UseHttpsRedirection();

//autoryzacja
app.UseAuthorization();

app.MapControllers();

app.Run();
