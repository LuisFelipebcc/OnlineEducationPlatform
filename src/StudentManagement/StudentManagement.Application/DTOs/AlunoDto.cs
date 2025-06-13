namespace StudentManagement.Application.DTOs
{
    public class AlunoDto
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public List<MatriculaDto> Matriculas { get; set; } = new List<MatriculaDto>();
        public List<CertificadoDto> Certificados { get; set; } = new List<CertificadoDto>();
    }
}