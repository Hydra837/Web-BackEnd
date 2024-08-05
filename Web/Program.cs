using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using Tools;
using DAL.Data;
using DAL.Repository;
using DAL.Interface;
using BLL.Interface;
using BLL.Service;
using DAL;
using BLL.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services au conteneur
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajouter la chaîne de connexion et le DbContext
builder.Services.AddSingleton(sp => new Tools.Connection(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<EFDbContextData>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Ajouter les services d'application
builder.Services.AddScoped<ICoursRepository, CoursRepository>();
builder.Services.AddScoped<ICoursService, CoursService>();
builder.Services.AddScoped<IusersRepository, UsersRepository>();
builder.Services.AddScoped<IusersService, UsersService>();
builder.Services.AddScoped<IStudent_EnrollmentRepository, Student_EnrollmentRepository>();
builder.Services.AddScoped<IStudentEnrollmentService, StudentEnrollmentService>();
builder.Services.AddScoped<IStudent_Management, Student_ManagementRepository>();
builder.Services.AddScoped<IStudentManagmentService, StudentManagementService>();

// Ajouter l'authentification JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Ajouter l'autorisation
builder.Services.AddAuthorization();

// Configurer CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // URL de votre application Angular en développement
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Enregistrer le service d'authentification
builder.Services.AddScoped<AuthenticationService>();

var app = builder.Build();

// Configurer le pipeline de requêtes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Utiliser CORS
app.UseCors("AllowAngularApp");

// Utiliser l'authentification et l'autorisation
app.UseAuthentication();
app.UseAuthorization();

// Configurer le routage
app.UseRouting();

// Configurer les fichiers statiques si nécessaire
var angularDistPath = @"C:\Users\maxim\source\repos\Projet_Ephec2\ClientApp\Client\dist";

if (Directory.Exists(angularDistPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(angularDistPath),
        RequestPath = ""
    });
}

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
