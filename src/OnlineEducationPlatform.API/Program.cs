using System.Text;
using System.Reflection;
using MediatR;
using ContentManagement.Application.Services;
using ContentManagement.Domain.Repositories;
using ContentManagement.Infrastructure.Persistence;
using ContentManagement.Infrastructure.Repositories;
using IdentityService.Application.Services;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Repositories;
using StudentManagement.Application.Services;
using StudentManagement.Domain.Repositories;
using StudentManagement.Infrastructure.Persistence;
using StudentManagement.Infrastructure.Repositories;
using PaymentBilling.Application.Services;
using PaymentBilling.Domain.Repositories;
using PaymentBilling.Infrastructure.Persistence;
using PaymentBilling.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using IdentityService.Application.Interfaces;
using StudentManagement.Application.Interfaces;
using PaymentBilling.Application.Interfaces; // Mantido
// Removido: using PaymentBilling.Infrastructure.Context; pois o DbContext está em PaymentBilling.Infrastructure.Persistence
// Removido: using ContentManagement.Domain.Interfaces; pois ICourseRepository está em ContentManagement.Domain.Repositories

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OnlineEducationPlatform.API",
        Version = "v1",
    });

    // Configure Swagger to use JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new List<string>()
        }
    });
});

// Configuração dos DbContexts
var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

var contentManagementConnectionString = builder.Configuration.GetConnectionString("ContentManagementConnection");
builder.Services.AddDbContext<ContentManagementDbContext>(options =>
    options.UseSqlServer(contentManagementConnectionString));

var studentManagementConnectionString = builder.Configuration.GetConnectionString("StudentManagementConnection");
builder.Services.AddDbContext<StudentManagementDbContext>(options =>
    options.UseSqlServer(studentManagementConnectionString));

var paymentBillingConnectionString = builder.Configuration.GetConnectionString("PaymentBillingConnection");
builder.Services.AddDbContext<PaymentBillingDbContext>(options =>
    options.UseSqlServer(paymentBillingConnectionString));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IdentityService.Domain.Repositories.IAlunoRepository, IdentityService.Infrastructure.Repositories.AlunoRepository>(); // Persona Aluno (IdentityService)
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<StudentManagement.Domain.Repositories.IAlunoRepository, StudentManagement.Infrastructure.Repositories.AlunoRepository>(); // Entidade Aluno (StudentManagement)
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();

// Register Application Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICursoAppService, CursoAppService>();
builder.Services.AddScoped<IAlunoAppService, AlunoAppService>();
builder.Services.AddScoped<IPagamentoAppService, PagamentoAppService>();

// Register MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(IdentityService.Application.Events.AlunoDaIdentidadeRegistradoEvent).Assembly,
        typeof(StudentManagement.Application.EventHandlers.AlunoDaIdentidadeRegistradoEventHandler).Assembly,
        typeof(PaymentBilling.Application.Services.PagamentoAppService).Assembly // Para futuros eventos/handlers do PaymentBilling
    );
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured."));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Em produção, defina como true
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey), // Usar a secretKey derivada de JwtSettings:Secret
            ClockSkew = TimeSpan.Zero
        };
    });

// Configure CORS
// builder.Services.AddCors(...); // Se necessário, configure o CORS

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseCors("AllowSpecificOrigins"); // Se CORS estiver configurado

// app.UseMiddleware<ExceptionMiddleware>(); // Se você tiver um middleware de exceção customizado

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    // Aqui você pode adicionar lógica para aplicar migrations automaticamente ou semear dados iniciais, se necessário.
    // Exemplo:
    // var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    // identityDb.Database.Migrate();
    // var contentDb = scope.ServiceProvider.GetRequiredService<ContentManagementDbContext>();
    // contentDb.Database.Migrate();
    // etc.
}

app.Run();
