# github-automation-api-test

Repositorio de validacao do fluxo de implementacao SDD (Spec-Driven Development).

## Proposito

Este repositorio serve como alvo de demonstracao para o fluxo de implementacao de specs
definidas em [kallebelins/github-automation](https://github.com/kallebelins/github-automation).

## Spec de Referencia

- **Spec ID:** S003
- **Titulo:** Criar projeto .NET 8 no repositorio alvo
- **Status:** approved
- **Branch:** feat/S003-criar-projeto-net-8-no-repositorio-alvo

## Pre-requisitos

- .NET SDK 8.0.x

## Estrutura

- src/GithubAutomation.Api
- tests/GithubAutomation.Api.Tests
- GithubAutomation.Api.sln

## Comandos principais

### Restaurar dependencias

dotnet restore GithubAutomation.Api.sln

### Build

dotnet build GithubAutomation.Api.sln --configuration Release --no-restore

### Executar API localmente

dotnet run --project src/GithubAutomation.Api/GithubAutomation.Api.csproj

Endpoint de health check:

GET http://localhost:5000/health

### Executar testes

dotnet test GithubAutomation.Api.sln --configuration Release --no-build
