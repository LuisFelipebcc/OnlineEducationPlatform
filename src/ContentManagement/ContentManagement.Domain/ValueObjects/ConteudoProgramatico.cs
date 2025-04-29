using System;
using System.Collections.Generic;

namespace ContentManagement.Domain.ValueObjects;

public class ConteudoProgramatico
{
    public string Descricao { get; private set; }
    public List<string> Objetivos { get; private set; }
    public List<string> PreRequisitos { get; private set; }

    protected ConteudoProgramatico() { }

    public ConteudoProgramatico(string descricao, List<string> objetivos, List<string> preRequisitos)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição não pode ser vazia", nameof(descricao));

        Descricao = descricao;
        Objetivos = objetivos ?? new List<string>();
        PreRequisitos = preRequisitos ?? new List<string>();
    }

    public void AtualizarDescricao(string novaDescricao)
    {
        if (string.IsNullOrWhiteSpace(novaDescricao))
            throw new ArgumentException("A descrição não pode ser vazia", nameof(novaDescricao));

        Descricao = novaDescricao;
    }

    public void AdicionarObjetivo(string objetivo)
    {
        if (string.IsNullOrWhiteSpace(objetivo))
            throw new ArgumentException("O objetivo não pode ser vazio", nameof(objetivo));

        Objetivos.Add(objetivo);
    }

    public void AdicionarPreRequisito(string preRequisito)
    {
        if (string.IsNullOrWhiteSpace(preRequisito))
            throw new ArgumentException("O pré-requisito não pode ser vazio", nameof(preRequisito));

        PreRequisitos.Add(preRequisito);
    }
}