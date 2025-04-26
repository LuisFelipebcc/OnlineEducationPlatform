namespace ContentManagement.Domain.Aggregates;

public class Aluno : Persona
{
    private readonly List<Matricula> _matriculas = new();
    public IReadOnlyCollection<Matricula> Matriculas => _matriculas.AsReadOnly();

    private Aluno() { }

    public Aluno(string nome, string documento) : base(nome, documento)
    {
    }

    public void Matricular(Curso curso)
    {
        if (curso == null)
            throw new ArgumentNullException(nameof(curso));

        var matricula = new Matricula(this, curso);
        _matriculas.Add(matricula);
    }
}