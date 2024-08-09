using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Tools;
using DAL.Data;
using DAL.Repository;
using DAL.Interface;
using BLL.Interface;
using BLL.Service;
using DAL;

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
builder.Services.AddScoped<IAssignementsRepository, AssignementsRepository>();
builder.Services.AddScoped<IAssignementsService, AssignementsService>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IGradeService, GradeService>();

// Enregistrer le service d'authentification
builder.Services.AddScoped<IAuthenticationService, AuthService>();

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
