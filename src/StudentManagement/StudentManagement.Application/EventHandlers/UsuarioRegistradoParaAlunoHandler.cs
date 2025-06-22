using MediatR;
using Shared.Domain.Events;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;

namespace StudentManagement.Application.EventHandlers
{
    public class UsuarioRegistradoParaAlunoHandler : INotificationHandler<UsuarioRegistradoEvent>
    {
        private readonly IAlunoAppService _alunoAppService;

        public UsuarioRegistradoParaAlunoHandler(IAlunoAppService alunoAppService)
        {
            _alunoAppService = alunoAppService;
        }

        public async Task Handle(UsuarioRegistradoEvent notification, CancellationToken cancellationToken)
        {
            // Mapeia os dados do evento para o DTO correto
            var criarAlunoDto = new CriarAlunoSincronizadoDto
            {
                Id = notification.UsuarioId,
                NomeCompleto = notification.NomeCompleto,
                Email = notification.Email
            };

            // Cria o aluno no contexto de StudentManagement
            await _alunoAppService.CriarAlunoSincronizadoAsync(criarAlunoDto);

            // Log opcional
            System.Console.WriteLine($"[StudentManagement] Recebido UsuarioRegistradoEvent: {notification.Email}. Aluno criado.");
        }
    }
}