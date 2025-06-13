using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentBilling.Application.DTOs;

namespace PaymentBilling.Application.Interfaces
{
    public interface IPagamentoAppService
    {
        Task<PagamentoDto?> ProcessarPagamentoAsync(CriarPagamentoDto criarPagamentoDto);
        Task<PagamentoDto?> GetPagamentoByIdAsync(Guid id);
        Task<IEnumerable<PagamentoDto>> GetPagamentosByAlunoIdAsync(Guid alunoId);

        // Futuramente:
        // Task<PagamentoDto?> CancelarPagamentoAsync(Guid pagamentoId, Guid alunoId);
        // Task<PagamentoDto?> ReembolsarPagamentoAsync(Guid pagamentoId, decimal valorReembolso);
    }
}