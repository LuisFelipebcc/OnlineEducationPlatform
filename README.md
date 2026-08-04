# Online Education Platform

Backend de uma plataforma de educação online desenvolvido com **ASP.NET Core**, estruturado com princípios de **Domain-Driven Design (DDD)**, **CQRS** e separação por contextos de negócio.

O projeto demonstra a modelagem de uma solução educacional modular, cobrindo gestão de conteúdo, alunos, matrículas e pagamentos.

## Objetivo

Construir uma base backend organizada para uma plataforma de cursos online, mantendo as regras de negócio separadas da infraestrutura e reduzindo o acoplamento entre os diferentes domínios da aplicação.

## Contextos de negócio

A solução está dividida nos seguintes bounded contexts:

- **Content Management** — gestão de cursos e aulas.
- **Student Management** — gestão de alunos e matrículas.
- **Payment & Billing** — processamento e acompanhamento de pagamentos.

## Arquitetura

Cada contexto segue uma divisão em camadas:

- **Domain** — entidades, value objects, contratos e regras de negócio.
- **Application** — comandos, queries e handlers responsáveis pelos casos de uso.
- **Infrastructure** — persistência, integrações e implementações técnicas.

A separação busca manter o domínio independente de detalhes de banco de dados, frameworks e mecanismos de entrega.

## Tecnologias

- .NET 7
- ASP.NET Core Web API
- Entity Framework Core
- MediatR
- SQL Server
- SQLite para desenvolvimento
- JWT Authentication
- Swagger / OpenAPI
- xUnit

## Capacidades implementadas

- Cadastro e consulta de cursos.
- Cadastro e consulta de aulas.
- Consulta de aulas por curso.
- Estrutura para gestão de alunos e matrículas.
- Estrutura para processamento de pagamentos.
- Autenticação baseada em JWT.
- Persistência com Entity Framework Core.
- Testes automatizados com xUnit.

## Execução local

### Pré-requisitos

- .NET 7 SDK
- SQL Server ou SQLite, conforme a configuração do ambiente

### Configuração

```bash
git clone https://github.com/LuisFelipebcc/OnlineEducationPlatform.git
cd OnlineEducationPlatform
dotnet restore
dotnet ef database update
dotnet run
```

Após iniciar a aplicação, consulte o Swagger para visualizar e testar os endpoints disponíveis.

## Testes

```bash
dotnet test
```

## Endpoints principais

| Método | Rota | Responsabilidade |
|---|---|---|
| `POST` | `/api/cursos` | Criar um curso |
| `GET` | `/api/cursos/{id}` | Consultar um curso |
| `POST` | `/api/aulas` | Criar uma aula |
| `GET` | `/api/aulas/{id}` | Consultar uma aula |
| `GET` | `/api/cursos/{cursoId}/aulas` | Listar aulas de um curso |

## Decisões de engenharia

- Uso de bounded contexts para evitar a concentração de responsabilidades em um único modelo.
- Aplicação de CQRS para separar operações de escrita e leitura.
- Isolamento das regras de domínio em relação à persistência.
- SQLite como alternativa de menor atrito para desenvolvimento local.
- Swagger como interface de exploração e validação da API.

## Próximas evoluções

- Atualizar o projeto para uma versão LTS atual do .NET.
- Documentar os fluxos completos de autenticação e autorização.
- Adicionar pipeline de integração contínua.
- Publicar diagrama de arquitetura e domínio.
- Adicionar métricas de cobertura de testes.
- Containerizar a aplicação e suas dependências.
- Expandir os casos de uso de matrícula e pagamentos.

## Status

Projeto de portfólio e estudo arquitetural. A documentação será evoluída junto com a validação técnica dos módulos existentes.

## Autor

**Luis Felipe da Silva Sousa**  
Backend Developer | .NET | Software Architecture | AI Engineering
