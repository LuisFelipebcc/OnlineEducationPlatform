using ContentManagement.Application.DTOs;
using ContentManagement.Domain.Aggregates;
using ContentManagement.Domain.Enums;
using ContentManagement.Domain.Repositories;
using ContentManagement.Domain.ValueObjects;
using MediatR;

namespace ContentManagement.Application.Commands.Handlers
{
    public class CriarCursoCommandHandler : IRequestHandler<CriarCursoCommand, CursoDTO>
    {
        private readonly ICursoRepository _cursoRepository;

        public CriarCursoCommandHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<CursoDTO> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
        {
            // Validação dos dados do comando
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new ArgumentException("O nome do curso é obrigatório.", nameof(request.Nome));

            if (string.IsNullOrWhiteSpace(request.DescricaoConteudo))
                throw new ArgumentException("A descrição do conteúdo programático é obrigatória.", nameof(request.DescricaoConteudo));

            if (request.Objetivos == null || !request.Objetivos.Any())
                throw new ArgumentException("Pelo menos um objetivo deve ser informado.", nameof(request.Objetivos));

            if (request.PreRequisitos == null)
                request.PreRequisitos = new List<string>();

            // Criação do Value Object ConteudoProgramatico
            var conteudoProgramatico = new ConteudoProgramatico(
                request.DescricaoConteudo,
                request.Objetivos,
                request.PreRequisitos
            );

            // Criação da entidade Curso
            var curso = new Curso(request.Nome, conteudoProgramatico);

            // Persistência no repositório
            var cursoCriado = await _cursoRepository.AddAsync(curso);

            // Mapeamento para DTO
            var cursoDTO = new CursoDTO
            {
                Id = cursoCriado.Id,
                Titulo = cursoCriado.Nome,
                Descricao = cursoCriado.ConteudoProgramatico.Descricao,
                Preco = 0, // Preço não está no comando, ajuste conforme necessário
                Duracao = 0, // Duração não está no comando, ajuste conforme necessário
                Nivel = string.Empty, // Nível não está no comando, ajuste conforme necessário
                Status = StatusCurso.Ativo,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = null,
                Aulas = new List<AulaDTO>() // Inicialmente sem aulas
            };

            return cursoDTO;
        }
    }
}