# Projeto - Cidades ESG Inteligentes

API REST em ASP.NET Core 8 criada a partir do projeto anterior de gestão de atendimentos em unidades de saúde. Nesta fase, o projeto foi adaptado para um cenário de DevOps, com containerização, orquestração via Docker Compose e pipeline CI/CD com build, testes e deploy em ambientes de staging e produção.

O contexto ESG aparece no eixo social: a aplicação apoia o controle de atendimentos, pacientes, unidades de saúde e alertas de capacidade, ajudando uma cidade inteligente a monitorar a qualidade do atendimento público.

## Como executar localmente com Docker

1. Copie o arquivo de exemplo de variáveis:

```bash
cp .env.example .env
```

2. Suba a aplicação e o banco SQL Server:

```bash
docker compose --env-file .env up --build
```

3. Acesse a API:

```text
http://localhost:8080/health
```

4. Para gerar token JWT:

```http
POST http://localhost:8080/auth/token
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

5. Para executar os testes localmente:

```bash
dotnet test SusNet8.sln
```

### Ambientes com Docker Compose

Ambiente local:

```bash
docker compose --env-file .env up --build
```

Ambiente de staging:

```bash
docker compose --env-file .env -f docker-compose.staging.yml up -d --build
```

URL de staging:

```text
http://localhost:8081/health
```

Ambiente de produção:

```bash
docker compose --env-file .env -f docker-compose.production.yml up -d --build
```

URL de produção:

```text
http://localhost:8082/health
```

Também existe o script:

```bash
./scripts/deploy-compose.sh staging
./scripts/deploy-compose.sh production
```

## Pipeline CI/CD

Ferramenta utilizada: GitHub Actions.

Arquivo do pipeline:

```text
.github/workflows/ci-cd.yml
```

O pipeline roda em push, pull request para a branch `main` e execução manual pelo `workflow_dispatch`.

Etapas implementadas:

- `build-test`: faz checkout, instala .NET 8, restaura dependências, executa build em Release e roda os testes xUnit.
- `docker-image`: valida o Docker Compose, faz build da imagem Docker da API e publica a imagem como artefato.
- `deploy-staging`: valida a configuração de staging, executa `docker compose up -d --build`, aguarda o endpoint `/health` em `http://localhost:8081/health` e publica logs como artefatos.
- `deploy-production`: roda somente depois do staging, executa `docker compose up -d --build`, aguarda o endpoint `/health` em `http://localhost:8082/health` e publica logs como artefatos.

Os ambientes declarados no GitHub Actions são:

- `staging`, com URL de referência `http://localhost:8081/health`.
- `production`, com URL de referência `http://localhost:8082/health`.

Para esta entrega acadêmica, os deploys são realizados no próprio runner do GitHub Actions com Docker Compose, simulando dois ambientes separados por arquivos, portas, nomes de containers, volumes e bancos distintos. Em um cenário real, os mesmos passos podem ser conectados a uma VM, Azure App Service, Azure Container Apps, AKS ou outro provedor.

## Containerização

Conteúdo do `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Sus.Api/Sus.Api.csproj", "src/Sus.Api/"]
RUN dotnet restore "src/Sus.Api/Sus.Api.csproj"
COPY . .
WORKDIR "/src/src/Sus.Api"
RUN dotnet publish "Sus.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Sus.Api.dll"]
```

Estratégias adotadas:

- Build em múltiplos estágios para separar SDK e runtime.
- Imagem final baseada em `mcr.microsoft.com/dotnet/aspnet:8.0`, menor que a imagem com SDK.
- Publicação em Release antes de copiar os arquivos para a imagem final.
- Porta interna `8080`, compatível com o Compose e com a variável `ASPNETCORE_URLS`.
- `.dockerignore` para evitar envio de `bin`, `obj`, `.git` e arquivos temporários ao contexto de build.

Orquestração com Docker Compose:

- Serviço `api`: aplicação ASP.NET Core 8.
- Serviço `sqlserver`: banco Microsoft SQL Server 2022.
- Volume `sus_sqlserver`: persistência dos dados do banco.
- Rede `sus_network`: comunicação interna entre API e banco.
- Variáveis de ambiente via `.env.example`.
- Arquivos adicionais para staging e produção.

## Prints do funcionamento

Evidências textuais geradas na validação local:

```text
dotnet test SusNet8.sln
Resultado: Aprovado - 6 testes aprovados, 0 falhas.
```

```text
docker compose --env-file .env.example config
Resultado esperado: configuração do Compose renderizada sem erro.
```

```text
docker build -t cidades-esg-api:local .
Resultado esperado: imagem Docker criada com sucesso.
```

Evidências do pipeline:

- Build automático: etapa `Build Release` no job `build-test`.
- Testes automatizados: etapa `Testes automatizados` no job `build-test`.
- Deploy staging: job `deploy-staging`, ambiente `staging`, endpoint `/health` validado e artefato `deploy-staging` publicado.
- Deploy produção: job `deploy-production`, ambiente `production`, endpoint `/health` validado e artefato `deploy-production` publicado.

Após rodar o workflow no GitHub Actions, os prints recomendados para anexar são:

- Página geral do workflow concluído.
- Job `build-test` mostrando build e testes aprovados.
- Job `deploy-staging` mostrando validação do endpoint `/health`.
- Job `deploy-production` mostrando validação do endpoint `/health`.
- Lista de artefatos publicados, incluindo `test-results`, `docker-image`, `deploy-staging` e `deploy-production`.

As evidências finais também estão resumidas no PDF técnico entregue junto com este projeto.

## Tecnologias utilizadas

- C# 12
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2022
- JWT Bearer Authentication
- Swagger/OpenAPI
- xUnit
- Docker
- Docker Compose
- GitHub Actions

## Estrutura do projeto

```text
seu-projeto/
|-- Dockerfile
|-- docker-compose.yml
|-- docker-compose.staging.yml
|-- docker-compose.production.yml
|-- .env.example
|-- .github/workflows/ci-cd.yml
|-- collections/
|-- scripts/
|-- src/
|-- tests/
|-- README.md
`-- SusNet8.sln
```

## Checklist de entrega

| Item | OK |
| --- | --- |
| Projeto compactado em .ZIP com estrutura organizada | X |
| Dockerfile funcional | X |
| docker-compose.yml ou arquivos Kubernetes | X |
| Pipeline com etapas de build, teste e deploy | X |
| README.md com instruções e prints | X |
| Documentação técnica com evidências (PDF ou PPT) | X |
| Deploy realizado nos ambientes staging e produção | X |
