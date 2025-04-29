namespace StudentManagement.Domain.Entities;

public class Student
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string CPF { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public StudentStatus Status { get; private set; }
    public HistoricoAprendizado HistoricoAprendizado { get; private set; }
    private readonly List<Matricula> _matriculas;
    public IReadOnlyCollection<Matricula> Matriculas => _matriculas.AsReadOnly();
    private readonly List<Certificado> _certificados;
    public IReadOnlyCollection<Certificado> Certificados => _certificados.AsReadOnly();

    // Construtor protegido para o Entity Framework
    protected Student()
    {
        _matriculas = new List<Matricula>();
        _certificados = new List<Certificado>();
    }

    // Construtor público
    public Student(string nome, string email, string cpf, DateTime dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome não pode ser vazio.", nameof(nome));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email não pode ser vazio.", nameof(email));
        if (string.IsNullOrWhiteSpace(cpf)) throw new ArgumentException("CPF não pode ser vazio.", nameof(cpf));
        if (dataNascimento > DateTime.UtcNow) throw new ArgumentException("Data de nascimento não pode ser no futuro.", nameof(dataNascimento));

        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        CPF = cpf;
        DataNascimento = dataNascimento;
        Status = StudentStatus.Active;
        HistoricoAprendizado = new HistoricoAprendizado();
        _matriculas = new List<Matricula>();
        _certificados = new List<Certificado>();
    }

    public void AdicionarMatricula(Matricula matricula)
    {
        if (matricula == null)
            throw new ArgumentNullException(nameof(matricula));

        _matriculas.Add(matricula);
    }

    public void AdicionarCertificado(Certificado certificado)
    {
        if (certificado == null)
            throw new ArgumentNullException(nameof(certificado));

        _certificados.Add(certificado);
    }

    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("O nome do aluno não pode ser vazio", nameof(novoNome));

        Nome = novoNome;
    }

    public void AtualizarEmail(string novoEmail)
    {
        if (string.IsNullOrWhiteSpace(novoEmail))
            throw new ArgumentException("O email do aluno não pode ser vazio", nameof(novoEmail));

        Email = novoEmail;
    }

    public void RegistrarProgresso(Guid cursoId, int percentualConcluido)
    {
        HistoricoAprendizado.RegistrarProgresso(cursoId, percentualConcluido);
    }

    public void AlterarStatus(StudentStatus novoStatus)
    {
        Status = novoStatus;
    }
}

public enum StudentStatus
{
    Active,
    Inactive
}
