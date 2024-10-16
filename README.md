
# Controle Financeiro Pessoal API

Bem-vindo à API de Controle Financeiro Pessoal! Esta API permite gerenciar créditos, débitos e outras operações financeiras pessoais.

## Índice

- [Tecnologias](#tecnologias)
- [Endpoints](#endpoints)
- [Autenticação](#autenticação)
- [Como Executar](#como-executar)
- [Licença](#licença)

## Tecnologias

- .NET 8
- ASP.NET Core
- MongoDB
- Swagger para documentação

## Endpoints

### Créditos

- **GET** `/api/credito` - Obtém todos os créditos.
- **GET** `/api/credito/{id}` - Obtém um crédito específico pelo ID.
- **POST** `/api/credito` - Cria um novo crédito.
- **PUT** `/api/credito/{id}` - Atualiza um crédito existente pelo ID.
- **DELETE** `/api/credito/{id}` - Deleta um crédito pelo ID.

### Débitos

- **GET** `/api/debito` - Obtém todos os débitos.
- **GET** `/api/debito/{id}` - Obtém um débito específico pelo ID.
- **POST** `/api/debito` - Cria um novo débito.
- **PUT** `/api/debito/{id}` - Atualiza um débito existente pelo ID.
- **DELETE** `/api/debito/{id}` - Deleta um débito pelo ID.

### Reservas

- **GET** `/api/reserva` - Obtém todas as reservas.
- **GET** `/api/reserva/{id}` - Obtém uma reserva específica pelo ID.
- **POST** `/api/reserva` - Cria uma nova reserva.
- **PUT** `/api/reserva/{id}` - Atualiza uma reserva existente pelo ID.
- **DELETE** `/api/reserva/{id}` - Deleta uma reserva pelo ID.
- **POST** `/api/{id}/usar` - Fazer o Uso de um valor da Reserva.
- **POST** `/api/{id}/adicionar` - Adicionar (Somar) um valor a Reserva.
- **GET** `/api/{id}/transacoes` - Obtém todas as transações das reservas.

### Saldo Final

- **GET** `/api/saldofinal` - Obtém o saldo final.
- **POST** `/api/saldofinal` - Cria um novo saldo final.

### DashBoard

- **GET** `/api/creditos/mes` - Créditos no mês corrente.
- **GET** `/api/debitos/mes` - Débitos no mês corrente.
- **GET** `/api/reservas/mes` - Reservas no mês corrente.
- **GET** `/api/saldofinal/mes` - Saldo Final no mês corrente.
- **GET** `/api/reservas/uso` - Uso das Reservas.
- **GET** `/api/reservas/saldo` - Saldo das Reservas por Período.
- **GET** `/api/reservas/historico` - Histórico de Reservas.
- **GET** `/api/reservas/evolucao` - Evolução das Reservas por mês.
- **GET** `/api/reservas/valores` - Valores Adicionados ou Utilizados das Reservas por Período.
- **GET** `/api/reservas/acumuladas` - Reservas Acumuladadas.

### Usuários

- **POST** `/api/usuario/login` - Faz login e gera um token JWT.
- **POST** `/api/usuario` - Cria um novo usuário.

## Autenticação

A API utiliza JSON Web Tokens (JWT) para autenticação. Ao fazer login com um usuário, um token JWT será retornado. Este token deve ser incluído no cabeçalho `Authorization` para acessar os endpoints protegidos.

## Como Executar

1. Clone este repositório.
2. Certifique-se de ter o .NET 8 instalado.
3. Configure as configurações do MongoDB no arquivo `appsettings.json`.
4. Execute o projeto utilizando o comando:
   ```bash
   dotnet run
   ```
5. Acesse a documentação da API através do Swagger em `http://localhost:5000`.
