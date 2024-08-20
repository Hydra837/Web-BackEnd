using BLL.Interface;
using BLL.Service;
using DAL.Interface;
using DAL.Repository;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

// Configure database context and services
builder.Services.AddSingleton(sp => new Tools.Connection(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<EFDbContextData>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Register application services
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

// Register the authentication service
builder.Services.AddScoped<IAuthenticationService, AuthService>();

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
       policy =>
       {
           policy.WithOrigins("https://localhost:7233", "http://localhost:4200") 
                 .AllowAnyHeader()
                 .AllowAnyMethod();
       });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins"); 
app.UseAuthentication();
app.UseAuthorization();


var angularDistPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "dist");

if (Directory.Exists(angularDistPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(angularDistPath),
        RequestPath = ""
    });
}

// Use custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
