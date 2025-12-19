# 🎬 Rapha Movies - Guia de Deploy no Azure App Service

Este documento descreve como implantar o projeto usando **Azure App Service (Web App)** e **Azure SQL Database**.

---

## 📑 Índice

1. [Arquitetura](#1-arquitetura)
2. [Pré-requisitos](#2-pré-requisitos)
3. [Azure SQL Database](#3-azure-sql-database)
4. [Backend .NET Core (Web App)](#4-backend-net-core-web-app)
5. [Frontend React (Web App)](#5-frontend-react-web-app)
6. [GitHub Actions CI/CD](#6-github-actions-cicd)
7. [Configurações Adicionais](#7-configurações-adicionais)
8. [Troubleshooting](#8-troubleshooting)

---

## 1. Arquitetura

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   Frontend      │────▶│   Backend API   │────▶│  Azure SQL      │
│   Web App       │     │   Web App       │     │  Database       │
│   React/Vite    │     │   .NET 8        │     │  SQL Server     │
│   (IIS)         │     │   (IIS)         │     │                 │
└─────────────────┘     └─────────────────┘     └─────────────────┘
```

| Componente | Tecnologia | Serviço Azure |
|------------|------------|---------------|
| Frontend | React + Vite + TypeScript | Azure App Service (Windows) |
| Backend | .NET 8 / ASP.NET Core | Azure App Service (Windows) |
| Banco de Dados | SQL Server | Azure SQL Database |
| CI/CD | GitHub Actions | - |

---

## 2. Pré-requisitos

### Ferramentas

| Ferramenta | Versão | Download |
|------------|--------|----------|
| Node.js | 20+ | [nodejs.org](https://nodejs.org/) |
| .NET SDK | 8.0+ | [dotnet.microsoft.com](https://dotnet.microsoft.com/download) |
| Git | 2.40+ | [git-scm.com](https://git-scm.com/) |
| Azure CLI | 2.50+ | [docs.microsoft.com/cli/azure](https://docs.microsoft.com/cli/azure/install-azure-cli) |

### Contas

- [x] Conta GitHub: [github.com](https://github.com/)
- [x] Conta Microsoft Azure: [azure.microsoft.com](https://azure.microsoft.com/free/)

---

## 3. Azure SQL Database

### 3.1. Criar SQL Server

**Via Portal Azure:**
1. Pesquise **"SQL servers"** → **"+ Create"**
2. Configure:
   - **Server name:** `raphamovies-sql-server`
   - **Location:** Brazil South
   - **Authentication:** SQL authentication
   - **Admin login:** `sqladmin`
   - **Password:** (senha forte)

**Via Azure CLI:**
```bash
az sql server create \
  --name raphamovies-sql-server \
  --resource-group rg-rapha-movies \
  --location brazilsouth \
  --admin-user sqladmin \
  --admin-password "SuaSenhaForte123!"
```

### 3.2. Criar Database

**Via Portal:**
1. No SQL Server → **"SQL databases"** → **"+ Create"**
2. Configure:
   - **Database name:** `RaphaMoviesDB`
   - **Compute + storage:** Basic (5 DTU) ou Standard S0

**Via Azure CLI:**
```bash
az sql db create \
  --resource-group rg-rapha-movies \
  --server raphamovies-sql-server \
  --name RaphaMoviesDB \
  --service-objective Basic
```

### 3.3. Configurar Firewall

Permita conexões dos Azure Services e seu IP:

```bash
# Permitir Azure Services
az sql server firewall-rule create \
  --resource-group rg-rapha-movies \
  --server raphamovies-sql-server \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Permitir seu IP (para desenvolvimento)
az sql server firewall-rule create \
  --resource-group rg-rapha-movies \
  --server raphamovies-sql-server \
  --name AllowMyIP \
  --start-ip-address SEU_IP \
  --end-ip-address SEU_IP
```

### 3.4. Connection String

```
Server=tcp:raphamovies-sql-server.database.windows.net,1433;Initial Catalog=RaphaMoviesDB;Persist Security Info=False;User ID=sqladmin;Password={sua_senha};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

### 3.5. Criar Tabelas

Execute os scripts SQL do arquivo `docs/API_BACKEND_SPEC.md` usando:
- **Azure Portal:** Query Editor no SQL Database
- **SSMS:** SQL Server Management Studio
- **Azure Data Studio**

---

## 4. Backend .NET Core (Web App)

### 4.1. Criar App Service

**Via Portal:**
1. **"+ Create a resource"** → **"Web App"**
2. Configure:
   - **Name:** `raphamovies-api`
   - **Runtime stack:** .NET 8
   - **Operating System:** Windows
   - **Region:** Brazil South
   - **App Service Plan:** Basic B1 ou Standard S1

**Via Azure CLI:**
```bash
# Criar App Service Plan
az appservice plan create \
  --name plan-rapha-movies \
  --resource-group rg-rapha-movies \
  --location brazilsouth \
  --sku B1

# Criar Web App
az webapp create \
  --name raphamovies-api \
  --resource-group rg-rapha-movies \
  --plan plan-rapha-movies \
  --runtime "DOTNET|8.0"
```

### 4.2. Configurar Connection String

No Portal → App Service → **Configuration** → **Connection strings**:

| Name | Value | Type |
|------|-------|------|
| DefaultConnection | `Server=tcp:raphamovies-sql-server.database.windows.net...` | SQLAzure |

### 4.3. Configurar Application Settings

| Name | Value |
|------|-------|
| Jwt__Secret | `sua-chave-secreta-com-pelo-menos-32-caracteres` |
| Jwt__Issuer | `RaphaMovies.Api` |
| Jwt__Audience | `RaphaMovies.Frontend` |
| Jwt__ExpirationMinutes | `60` |
| ASPNETCORE_ENVIRONMENT | `Production` |

### 4.4. Configurar CORS no Backend

No `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(
            "https://raphamovies-frontend.azurewebsites.net",
            "http://localhost:5173"  // Dev local
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// No pipeline:
app.UseCors("Production");
```

### 4.5. Deploy do Backend

**Opção A - Visual Studio:**
1. Right-click no projeto → **Publish**
2. Selecione **Azure** → **Azure App Service (Windows)**
3. Selecione `raphamovies-api`
4. Clique em **Publish**

**Opção B - Azure CLI:**
```bash
# Build
dotnet publish -c Release -o ./publish

# Deploy
az webapp deploy \
  --resource-group rg-rapha-movies \
  --name raphamovies-api \
  --src-path ./publish.zip
```

---

## 5. Frontend React (Web App)

### 5.1. Criar App Service

**Via Portal:**
1. **"+ Create a resource"** → **"Web App"**
2. Configure:
   - **Name:** `raphamovies-frontend`
   - **Runtime stack:** Node 20 LTS
   - **Operating System:** Windows
   - **Region:** Brazil South
   - **App Service Plan:** Use o mesmo plano do backend

**Via Azure CLI:**
```bash
az webapp create \
  --name raphamovies-frontend \
  --resource-group rg-rapha-movies \
  --plan plan-rapha-movies \
  --runtime "NODE|20-lts"
```

### 5.2. web.config para IIS

O arquivo `public/web.config` já está configurado com:
- Roteamento SPA (todas as rotas → index.html)
- MIME types para fontes e imagens modernas
- Headers de segurança
- Compressão HTTP

### 5.3. Configurar variável da API

No frontend, a URL da API é configurada via `VITE_API_URL`:

**Desenvolvimento local** - `.env.local`:
```env
VITE_API_URL=http://localhost:5000/api
```

**Produção** - Configure no GitHub Actions (secret `VITE_API_URL`):
```
https://raphamovies-api.azurewebsites.net/api
```

---

## 6. GitHub Actions CI/CD

### 6.1. Secrets Necessários

Configure no GitHub → Settings → Secrets and variables → Actions:

| Secret | Descrição |
|--------|-----------|
| `AZURE_WEBAPP_NAME` | `raphamovies-frontend` |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | XML do Publish Profile |
| `VITE_API_URL` | `https://raphamovies-api.azurewebsites.net/api` |

### 6.2. Obter Publish Profile

1. No Portal Azure → Web App → **Download publish profile**
2. Abra o arquivo XML
3. Copie todo o conteúdo
4. Cole no secret `AZURE_WEBAPP_PUBLISH_PROFILE`

### 6.3. Workflow

O arquivo `.github/workflows/azure-webapp-deploy.yml` está configurado para:
- Executar em push na branch `main`
- Instalar dependências
- Build com variáveis de ambiente
- Deploy para Azure Web App

### 6.4. Executar Deploy

1. Faça push para a branch `main`
2. Acesse GitHub → Actions
3. Acompanhe o workflow
4. Após concluído, acesse a URL do Web App

---

## 7. Configurações Adicionais

### 7.1. Custom Domain

1. App Service → **Custom domains** → **Add custom domain**
2. Configure DNS no seu provedor:
   - CNAME: `www` → `raphamovies-frontend.azurewebsites.net`
3. Adicione certificado SSL (managed ou próprio)

### 7.2. Application Insights

1. App Service → **Application Insights** → **Turn on**
2. Selecione ou crie um recurso
3. Habilita monitoramento de performance e erros

### 7.3. Auto-scaling

1. App Service Plan → **Scale out**
2. Configure regras baseadas em:
   - CPU percentage
   - Memory percentage
   - HTTP queue length

### 7.4. Backup

1. App Service → **Backups** → **Configure**
2. Selecione storage account
3. Configure schedule

---

## 8. Troubleshooting

### Frontend não carrega

- Verifique se `web.config` está no `dist/`
- Acesse **Log stream** no portal
- Verifique se Node.js está configurado corretamente

### Erro 500 no Backend

- Verifique **Application Insights** → **Failures**
- Confirme connection string do banco
- Verifique logs em **Log stream**

### API não conecta ao banco

```bash
# Teste conexão
az sql db show-connection-string \
  --client ado.net \
  --server raphamovies-sql-server \
  --name RaphaMoviesDB
```

- Verifique firewall do SQL Server
- Confirme IP do App Service nas regras

### CORS bloqueando requisições

- Confirme URL exata no CORS (com/sem trailing slash)
- Verifique se está usando HTTPS em produção
- Teste com `AllowAnyOrigin()` temporariamente

### Build falha no GitHub Actions

- Verifique secrets configurados
- Confirme versão do Node.js
- Veja logs detalhados na Action

---

## 9. Checklist de Deploy

### Azure SQL Database
- [ ] SQL Server criado
- [ ] Database criado
- [ ] Firewall configurado (Azure Services + seu IP)
- [ ] Tabelas criadas

### Backend .NET Core
- [ ] Web App criado
- [ ] Connection string configurada
- [ ] Application settings configuradas (JWT, etc.)
- [ ] CORS configurado
- [ ] Deploy realizado

### Frontend React
- [ ] Web App criado
- [ ] GitHub secrets configurados
- [ ] Workflow rodando com sucesso
- [ ] web.config copiado para dist/

### Integração
- [ ] Frontend conecta à API
- [ ] API conecta ao banco
- [ ] Autenticação funcionando
- [ ] CRUD de filmes funcionando
- [ ] Sistema de aluguéis funcionando

---

## 10. Custos Estimados (Brazil South)

| Recurso | SKU | Custo Estimado/mês |
|---------|-----|-------------------|
| App Service Plan | B1 (1 core, 1.75 GB) | ~$13 USD |
| Azure SQL Database | Basic (5 DTU) | ~$5 USD |
| **Total** | | **~$18 USD** |

*Valores aproximados. Consulte [azure.microsoft.com/pricing](https://azure.microsoft.com/pricing/) para valores atualizados.*

Para reduzir custos em ambiente de desenvolvimento, considere:
- Usar Free tier do App Service (com limitações)
- Pausar recursos quando não estiver usando
- Usar Azure Dev/Test pricing
