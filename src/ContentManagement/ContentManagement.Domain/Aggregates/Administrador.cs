namespace ContentManagement.Domain.Aggregates;

public class Administrador : Persona
{
    private Administrador() { }

    public Administrador(string nome, string documento) : base(nome, documento)
    {
    }
}