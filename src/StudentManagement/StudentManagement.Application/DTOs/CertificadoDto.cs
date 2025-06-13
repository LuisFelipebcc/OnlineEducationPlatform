namespace StudentManagement.Application.DTOs
{
    public class CertificadoDto
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public string NomeCurso { get; set; }
        public DateTime DataEmissao { get; set; }
        public string CodigoVerificacao { get; set; }
    }
}