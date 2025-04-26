using ContentManagement.Domain.Enums;

namespace ContentManagement.Domain.Aggregates;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public TipoUsuario Tipo { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }

    // Relacionamento com Persona
    public Persona Persona { get; private set; }

    private Usuario() { }

    public Usuario(string email, string senha, TipoUsuario tipo)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("Senha é obrigatória", nameof(senha));

        Id = Guid.NewGuid();
        Email = email;
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
        Tipo = tipo;
        Ativo = true;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        Email = email;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AlterarSenha(string novaSenha)
    {
        if (string.IsNullOrWhiteSpace(novaSenha))
            throw new ArgumentException("Senha é obrigatória", nameof(novaSenha));

        SenhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        DataAtualizacao = DateTime.UtcNow;
    }

    public bool VerificarSenha(string senha)
    {
        return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
    }

    public void Desativar()
    {
        Ativo = false;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Ativo = true;
        DataAtualizacao = DateTime.UtcNow;
    }
}