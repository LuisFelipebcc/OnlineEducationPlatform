using ContentManagement.Application.DTOs;
using MediatR;

namespace ContentManagement.Application.Commands
{
    public class CriarCursoCommand : IRequest<CursoDTO>
    {
        public string Nome { get; set; }
        public string DescricaoConteudo { get; set; }
        public List<string> Objetivos { get; set; }
        public List<string> PreRequisitos { get; set; }
        public decimal Preco { get; set; }
        public int Duracao { get; set; }
        public string Nivel { get; set; }

        public CriarCursoCommand()
        {
            Objetivos = new List<string>();
            PreRequisitos = new List<string>();
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new ArgumentException("O nome do curso é obrigatório.", nameof(Nome));

            if (string.IsNullOrWhiteSpace(DescricaoConteudo))
                throw new ArgumentException("A descrição do conteúdo programático é obrigatória.", nameof(DescricaoConteudo));

            if (Objetivos == null || Objetivos.Count == 0)
                throw new ArgumentException("Pelo menos um objetivo deve ser informado.", nameof(Objetivos));

            if (Preco <= 0)
                throw new ArgumentException("O preço do curso deve ser maior que zero.", nameof(Preco));

            if (Duracao <= 0)
                throw new ArgumentException("A duração do curso deve ser maior que zero.", nameof(Duracao));

            if (string.IsNullOrWhiteSpace(Nivel))
                throw new ArgumentException("O nível do curso é obrigatório.", nameof(Nivel));
        }
    }
}