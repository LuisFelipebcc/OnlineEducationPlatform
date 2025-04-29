using System;
using ContentManagement.Domain.Entities;
using MediatR;

namespace ContentManagement.Application.Queries
{
    public class ObterCursoPorIdQuery : IRequest<Curso>
    {
        public Guid Id { get; set; }
    }
}