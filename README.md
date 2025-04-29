# Plataforma de Educação Online

Este projeto é uma plataforma de educação online desenvolvida com ASP.NET Core, seguindo os princípios do Domain-Driven Design (DDD), Command Query Responsibility Segregation (CQRS) e Test-Driven Development (TDD).

## Estrutura do Projeto

O projeto está organizado em bounded contexts (BCs) para separar as responsabilidades do domínio:

1. **ContentManagement**: Responsável pela gestão de cursos e aulas
2. **StudentManagement**: Responsável pela gestão de alunos e matrículas
3. **PaymentBilling**: Responsável pelo processamento de pagamentos

### Arquitetura

Cada bounded context segue uma arquitetura em camadas:

- **Domain**: Contém as entidades, value objects e interfaces dos repositórios
- **Application**: Contém os comandos, queries e handlers do CQRS
- **Infrastructure**: Contém as implementações dos repositórios e configurações do Entity Framework

## Tecnologias Utilizadas

- ASP.NET Core 7.0
- Entity Framework Core
- SQL Server / SQLite
- MediatR (CQRS)
- JWT Authentication
- Swagger/OpenAPI
- xUnit (Testes)

## Requisitos

- .NET 7.0 SDK
- SQL Server (para produção)
- SQLite (para desenvolvimento)

## Configuração do Ambiente

1. Clone o repositório
2. Restaure os pacotes NuGet:
   ```
   dotnet restore
   ```
3. Execute as migrações do banco de dados:
   ```
   dotnet ef database update
   ```
4. Execute o projeto:
   ```
   dotnet run
   ```

## Testes

Para executar os testes:

```
dotnet test
```

## API Endpoints

### Cursos

- `POST /api/cursos`: Criar um novo curso
- `GET /api/cursos/{id}`: Obter um curso por ID

### Aulas

- `POST /api/aulas`: Criar uma nova aula
- `GET /api/aulas/{id}`: Obter uma aula por ID
- `GET /api/cursos/{cursoId}/aulas`: Obter todas as aulas de um curso

## Autenticação

A API utiliza autenticação JWT. Para obter um token:

1. Faça login com suas credenciais
2. Use o token retornado no header `Authorization: Bearer {token}`

## Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.
