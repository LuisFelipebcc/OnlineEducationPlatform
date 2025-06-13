using IdentityService.Application.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;

namespace StudentManagement.Application.EventHandlers
{
    public class AlunoDaIdentidadeRegistradoEventHandler : INotificationHandler<AlunoDaIdentidadeRegistradoEvent>
    {
        private readonly IAlunoAppService _alunoAppService;
        private readonly ILogger<AlunoDaIdentidadeRegistradoEventHandler> _logger;

        public AlunoDaIdentidadeRegistradoEventHandler(IAlunoAppService alunoAppService, ILogger<AlunoDaIdentidadeRegistradoEventHandler> logger)
        {
            _alunoAppService = alunoAppService;
            _logger = logger;
        }

        public async Task Handle(AlunoDaIdentidadeRegistradoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Recebido AlunoDaIdentidadeRegistradoEvent para AlunoId: {AlunoId}", notification.AlunoId);
            var dto = new CriarAlunoSincronizadoDto { Id = notification.AlunoId, NomeCompleto = notification.NomeCompleto, Email = notification.Email };
            var alunoCriado = await _alunoAppService.CriarAlunoSincronizadoAsync(dto);
            // Adicionar lógica de tratamento de erro ou log se alunoCriado for null ou se ocorrer uma exceção.
            _logger.LogInformation("Aluno {Status} no StudentManagement para AlunoId: {AlunoId}", alunoCriado != null ? "criado/sincronizado" : "não pôde ser criado/sincronizado", notification.AlunoId);
        }
    }
}