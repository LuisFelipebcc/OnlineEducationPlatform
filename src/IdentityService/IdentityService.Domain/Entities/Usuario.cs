namespace IdentityService.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string SenhaHash { get; private set; }
        public string Role { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? UltimoAcesso { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiraEm { get; private set; }

        protected Usuario() { }

        public Usuario(string nome, string email, string senhaHash, string role)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            SenhaHash = senhaHash;
            Role = role;
            Ativo = true;
            DataCriacao = DateTime.UtcNow;
        }

        public void AtualizarNome(string novoNome)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                throw new ArgumentException("O nome não pode ser vazio", nameof(novoNome));

            Nome = novoNome;
        }

        public void AtualizarEmail(string novoEmail)
        {
            if (string.IsNullOrWhiteSpace(novoEmail))
                throw new ArgumentException("O email não pode ser vazio", nameof(novoEmail));

            Email = novoEmail;
        }

        public void AtualizarSenha(string novaSenhaHash)
        {
            if (string.IsNullOrWhiteSpace(novaSenhaHash))
                throw new ArgumentException("A senha não pode ser vazia", nameof(novaSenhaHash));

            SenhaHash = novaSenhaHash;
        }

        public void Ativar()
        {
            Ativo = true;
        }

        public void Desativar()
        {
            Ativo = false;
        }

        public void RegistrarAcesso()
        {
            UltimoAcesso = DateTime.UtcNow;
        }

        public void AtualizarRefreshToken(string refreshToken, DateTime expiraEm)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiraEm = expiraEm;
        }

        public void LimparRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiraEm = null;
        }
    }
}