namespace ContentManagement.Application.DTOs
{
    public class AulaDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int DuracaoEmMinutos { get; set; }
        public int Ordem { get; set; }
        public string UrlVideo { get; set; }
    }
}