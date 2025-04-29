using ContentManagement.Domain.Aggregates;
using StudentManagement.Domain.Enums;
using System;

namespace StudentManagement.Domain.Entities
{
    public class Matricula
    {
        public Guid Id { get; private set; }
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public StatusMatricula Status { get; private set; }
        public decimal? Nota { get; private set; }
        public string? Observacoes { get; private set; }

        // Propriedades de navegação
        public virtual Student Aluno { get; private set; }
        public virtual Curso Curso { get; private set; }

        // Construtor protegido para o Entity Framework
        protected Matricula() { }

        // Construtor público
        public Matricula(Guid alunoId, Guid cursoId)
        {
            Id = Guid.NewGuid();
            AlunoId = alunoId;
            CursoId = cursoId;
            DataMatricula = DateTime.UtcNow;
            Status = StatusMatricula.Ativo;
        }

        public void AtualizarStatus(StatusMatricula novoStatus)
        {
            Status = novoStatus;
        }

        public void AtualizarNota(decimal nota)
        {
            if (nota < 0 || nota > 10)
                throw new ArgumentException("A nota deve estar entre 0 e 10.", nameof(nota));

            Nota = nota;
        }

        public void Concluir()
        {
            if (Status != StatusMatricula.Ativo)
                throw new InvalidOperationException("A matrícula deve estar ativa para ser concluída.");

            DataConclusao = DateTime.UtcNow;
            Status = StatusMatricula.Concluido;
        }

        public void Cancelar(string motivo)
        {
            if (Status == StatusMatricula.Concluido)
                throw new InvalidOperationException("Não é possível cancelar uma matrícula já concluída.");

            Status = StatusMatricula.Cancelado;
            Observacoes = motivo;
        }
    }
}
