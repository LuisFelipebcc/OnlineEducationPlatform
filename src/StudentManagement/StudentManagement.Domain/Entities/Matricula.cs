using System;
using StudentManagement.Domain.ValueObjects; // Para HistoricoAprendizado
using StudentManagement.Domain.Enums; // Adicionado

namespace StudentManagement.Domain.Entities
{
    public class Matricula // Poderia herdar de uma classe base Entity<Guid>
    {
        public Guid Id { get; private set; }
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; } // ID do curso no ContentManagement BC
        public DateTime DataMatricula { get; private set; }
        public StatusMatricula Status { get; private set; } // Modificado de bool Ativa
        public decimal PrecoPago { get; private set; } // Será definido após pagamento
        public HistoricoAprendizado? HistoricoAprendizado { get; private set; }

        // Construtor para EF Core
        private Matricula() { }

        public Matricula(Guid alunoId, Guid cursoId, decimal precoPago)
        {
            Id = Guid.NewGuid();
            AlunoId = alunoId;
            CursoId = cursoId;
            DataMatricula = DateTime.UtcNow;
            Status = StatusMatricula.PendentePagamento; // Status inicial
            PrecoPago = precoPago; // O preço do curso, não necessariamente o pago ainda
            HistoricoAprendizado = new HistoricoAprendizado(); // Inicia um histórico vazio
        }

        public void Cancelar()
        {
            if (Status == StatusMatricula.Cancelada)
            {
                throw new InvalidOperationException("Matrícula já está cancelada.");
            }
            Status = StatusMatricula.Cancelada;
            // Lógica adicional de cancelamento, se houver (ex: registrar data de cancelamento)
        }

        public void AtivarAposPagamento(decimal valorEfetivamentePago)
        {
            if (Status != StatusMatricula.PendentePagamento)
            {
                throw new InvalidOperationException("Matrícula só pode ser ativada se estiver pendente de pagamento.");
            }
            // Poderia haver uma verificação se valorEfetivamentePago corresponde ao PrecoPago esperado
            PrecoPago = valorEfetivamentePago; // Atualiza com o valor efetivamente pago
            Status = StatusMatricula.Ativa;
        }

        // Métodos para atualizar progresso, etc.
        public void RegistrarProgressoAula(Guid aulaId)
        {
            // Garante que o HistoricoAprendizado não seja nulo e atualiza-o.
            HistoricoAprendizado = (HistoricoAprendizado ?? new HistoricoAprendizado()).MarcarAulaConcluida(aulaId);
        }
    }
}