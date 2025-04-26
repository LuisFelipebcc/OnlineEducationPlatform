using ContentManagement.Domain.Aggregates;
using ContentManagement.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Context;

public class ContentManagementDbContext : DbContext
{
    public ContentManagementDbContext(DbContextOptions<ContentManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Aula> Aulas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Administrador> Administradores { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CursoConfiguration());
        modelBuilder.ApplyConfiguration(new AulaConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new PersonaConfiguration());
        modelBuilder.ApplyConfiguration(new MatriculaConfiguration());
    }
}