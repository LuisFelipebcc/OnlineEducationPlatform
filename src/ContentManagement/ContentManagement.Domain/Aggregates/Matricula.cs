namespace ContentManagement.Domain.Aggregates;

public class Matricula
{
    public Guid Id { get; private set; }
    public Aluno Aluno { get; private set; }
    public Curso Curso { get; private set; }
    public DateTime DataMatricula { get; private set; }
    public bool Ativa { get; private set; }

    private Matricula() { }

    public Matricula(Aluno aluno, Curso curso)
    {
        if (aluno == null)
            throw new ArgumentNullException(nameof(aluno));

        if (curso == null)
            throw new ArgumentNullException(nameof(curso));

        Id = Guid.NewGuid();
        Aluno = aluno;
        Curso = curso;
        DataMatricula = DateTime.UtcNow;
        Ativa = true;
    }

    public void Cancelar()
    {
        Ativa = false;
    }

    public void Reativar()
    {
        Ativa = true;
    }
}