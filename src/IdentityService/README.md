# Identity Service

Este serviço é responsável por gerenciar a autenticação e autorização dos usuários da plataforma de educação online.

## Funcionalidades

- Registro de usuários
- Login com JWT
- Validação de token
- Revogação de token
- Gerenciamento de usuários (CRUD)
- Alteração de senha
- Redefinição de senha
- Confirmação de email

## Tecnologias Utilizadas

- .NET 7.0
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI

## Configuração

1. Clone o repositório
2. Restaure os pacotes NuGet
3. Configure a string de conexão no arquivo `appsettings.json`
4. Execute as migrações do banco de dados
5. Execute o projeto

## Endpoints da API

### Autenticação

- `POST /api/auth/login` - Login de usuário
- `POST /api/auth/register` - Registro de usuário
- `GET /api/auth/validate` - Validação de token
- `POST /api/auth/revoke` - Revogação de token

### Usuários

- `GET /api/auth/user/{userId}` - Obter usuário por ID
- `PUT /api/auth/user/{userId}` - Atualizar usuário
- `DELETE /api/auth/user/{userId}` - Excluir usuário
- `POST /api/auth/user/{userId}/change-password` - Alterar senha
- `POST /api/auth/reset-password` - Redefinir senha
- `POST /api/auth/confirm-email` - Confirmar email

## Segurança

- Autenticação baseada em JWT
- Senhas criptografadas com ASP.NET Core Identity
- Validação de email
- Proteção contra ataques de força bruta
- Tokens com tempo de expiração configurável

## Desenvolvimento

Para contribuir com o projeto:

1. Crie uma branch para sua feature
2. Faça commit das suas alterações
3. Envie um pull request

## Licença

Este projeto está licenciado sob a licença MIT.
