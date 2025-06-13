namespace ContentManagement.Application.DTOs
{
    public class CursoDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public Guid InstrutorId { get; set; }
        public DateTime DataPublicacao { get; set; }
        public decimal Preco { get; set; }
        public ConteudoProgramaticoDto ConteudoProgramatico { get; set; }
        public List<AulaDto> Aulas { get; set; } = new List<AulaDto>();
    }
}