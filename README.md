# Checkpoint 1 — API de Plataforma de Cursos Online

## Sobre o projeto

Este projeto é um desafio prático do curso de desenvolvimento backend .NET da Alura. O objetivo é construir uma **API REST** que gerencia cursos, estudantes e matrículas de uma plataforma de cursos online, simulando o dia a dia de uma pessoa desenvolvedora .NET no mercado de trabalho.

Ao final do projeto, a API contará com:

- CRUDs completos para as entidades principais
- Autenticação e autorização
- Banco de dados persistindo as informações
- Documentação completa da API

## Tecnologias

- [.NET 9](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- OpenAPI (Swagger)

## Como executar

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Rodando a aplicação

```bash
dotnet run
```

A API estará disponível em `https://localhost:{porta}`. A documentação OpenAPI pode ser acessada em `/openapi/v1.json` no ambiente de desenvolvimento.

## Estrutura do projeto

```
DotnetCheckpoint1/
├── Controllers/        # Controllers da API
├── Properties/         # Configurações de execução
├── appsettings.json    # Configurações da aplicação
└── Program.cs          # Ponto de entrada da aplicação
```
