# 🎬 Rapha Movies - Guia Completo de Deploy no Azure

## Para quem é este guia?

Este guia foi criado para pessoas **sem experiência técnica** que precisam configurar a aplicação no Azure. Cada passo inclui instruções detalhadas com imagens descritivas.

---

## 📑 Índice

1. [Visão Geral](#1-visão-geral)
2. [Criar Conta no Azure](#2-criar-conta-no-azure)
3. [Criar Grupo de Recursos](#3-criar-grupo-de-recursos)
4. [Configurar Azure SQL Database](#4-configurar-azure-sql-database)
5. [Criar App Service para o Backend](#5-criar-app-service-para-o-backend-api)
6. [Criar App Service para o Frontend](#6-criar-app-service-para-o-frontend)
7. [Configurar GitHub para Deploy Automático](#7-configurar-github-para-deploy-automático)
8. [Testar a Aplicação](#8-testar-a-aplicação)
9. [Solução de Problemas](#9-solução-de-problemas)
10. [Glossário de Termos](#10-glossário-de-termos)

---

## 1. Visão Geral

### O que vamos criar?

```
┌─────────────────────────────────────────────────────────────────────┐
│                        INTERNET (Usuários)                          │
└─────────────────────────────┬───────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    FRONTEND (App Service)                           │
│                                                                     │
│  Nome: raphamovies-frontend                                         │
│  O que faz: Mostra as telas para o usuário                         │
│  Tecnologia: React (páginas web interativas)                        │
│  URL: https://raphamovies-frontend.azurewebsites.net               │
└─────────────────────────────┬───────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    BACKEND/API (App Service)                        │
│                                                                     │
│  Nome: raphamovies-api                                              │
│  O que faz: Processa dados, login, aluguéis                        │
│  Tecnologia: .NET Core (lógica do sistema)                         │
│  URL: https://raphamovies-api.azurewebsites.net                    │
└─────────────────────────────┬───────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    BANCO DE DADOS (SQL Database)                    │
│                                                                     │
│  Nome: RaphaMoviesDB                                                │
│  O que faz: Armazena filmes, usuários, aluguéis                    │
│  Tecnologia: SQL Server (banco de dados)                           │
└─────────────────────────────────────────────────────────────────────┘
```

### Custo Estimado

| Recurso | Plano | Custo Mensal |
|---------|-------|--------------|
| App Service (Backend + Frontend) | Basic B1 | ~R$ 70 |
| SQL Database | Basic | ~R$ 25 |
| **Total Estimado** | | **~R$ 95/mês** |

*Valores aproximados em Reais. O Azure oferece R$ 1.000 de crédito grátis para novos usuários.*

---

## 2. Criar Conta no Azure

### Passo 2.1: Acessar o site do Azure

1. Abra seu navegador (Chrome, Firefox, Edge)
2. Digite na barra de endereços: **https://azure.microsoft.com/pt-br/free/**
3. Pressione **Enter**

### Passo 2.2: Iniciar cadastro gratuito

1. Na página que abrir, clique no botão verde **"Comece gratuitamente"**
2. Você será redirecionado para a página de login da Microsoft

### Passo 2.3: Criar ou usar conta Microsoft

**Se você já tem conta Microsoft (Outlook, Hotmail, Xbox):**
1. Digite seu email
2. Clique em **"Avançar"**
3. Digite sua senha
4. Clique em **"Entrar"**

**Se você NÃO tem conta Microsoft:**
1. Clique em **"Criar uma!"**
2. Digite um email (pode ser Gmail ou outro)
3. Crie uma senha forte
4. Siga as instruções para verificar seu email

### Passo 2.4: Preencher dados de cadastro

Você precisará fornecer:

1. **Informações pessoais:**
   - Nome completo
   - País: Brasil
   - Data de nascimento

2. **Verificação por telefone:**
   - Digite seu número de celular
   - Clique em **"Enviar SMS"**
   - Digite o código que você receber

3. **Verificação de cartão de crédito:**
   - ⚠️ **IMPORTANTE:** O cartão é apenas para verificação
   - **Você NÃO será cobrado** se usar apenas recursos gratuitos
   - Digite os dados do cartão
   - Confirme

### Passo 2.5: Aceitar termos

1. Marque a caixa ☑️ concordando com os termos
2. Clique em **"Inscrever-se"**
3. Aguarde alguns segundos
4. Você verá a mensagem **"Bem-vindo ao Microsoft Azure!"**

### Passo 2.6: Acessar o Portal Azure

1. Clique em **"Ir para o portal"**
2. Ou acesse diretamente: **https://portal.azure.com**
3. Você verá o painel principal do Azure (Dashboard)

---

## 3. Criar Grupo de Recursos

> 💡 **O que é um Grupo de Recursos?**
> É uma "pasta" que organiza todos os recursos do seu projeto. Assim fica fácil gerenciar, ver custos e deletar tudo junto se precisar.

### Passo 3.1: Abrir criação de Grupo de Recursos

1. No Portal Azure, olhe no menu à esquerda
2. Clique em **"Grupos de recursos"**
   - Se não aparecer, clique em **"Todos os serviços"** e pesquise "Grupos de recursos"
3. Clique no botão **"+ Criar"** (canto superior esquerdo)

### Passo 3.2: Configurar o Grupo

Na tela que abrir, preencha:

| Campo | O que colocar |
|-------|---------------|
| **Assinatura** | Selecione "Azure subscription 1" ou "Pay-As-You-Go" |
| **Grupo de recursos** | Digite: `rg-rapha-movies` |
| **Região** | Selecione: `Brazil South` |

### Passo 3.3: Criar

1. Clique no botão **"Revisar + criar"** (parte inferior)
2. Aguarde a validação (alguns segundos)
3. Clique em **"Criar"**
4. Aguarde a mensagem **"Implantação concluída"**

✅ **Sucesso!** Seu grupo de recursos foi criado.

---

## 4. Configurar Azure SQL Database

### Passo 4.1: Criar o Servidor SQL

> 💡 **O que é o Servidor SQL?**
> É o "computador virtual" que vai rodar seu banco de dados. Primeiro criamos o servidor, depois o banco dentro dele.

1. No Portal Azure, clique em **"+ Criar um recurso"** (canto superior esquerdo)
2. Na barra de pesquisa, digite: **SQL Server**
3. Nos resultados, clique em **"SQL Server (servidor lógico)"**
4. Clique em **"Criar"**

### Passo 4.2: Configurar o Servidor SQL

Preencha os campos:

| Campo | O que colocar | Explicação |
|-------|---------------|------------|
| **Assinatura** | Sua assinatura | Já vem preenchido |
| **Grupo de recursos** | `rg-rapha-movies` | Selecione o que criamos |
| **Nome do servidor** | `raphamovies-sqlserver` | Nome único (só letras minúsculas e números) |
| **Localização** | `Brazil South` | Mais próximo dos usuários |
| **Método de autenticação** | Selecione: "Usar autenticação SQL" | |
| **Logon do administrador** | `sqladmin` | Nome do usuário administrador |
| **Senha** | Crie uma senha forte | Mínimo 8 caracteres, com maiúscula, número e símbolo |
| **Confirmar senha** | Repita a senha | |

⚠️ **IMPORTANTE:** Anote a senha em um local seguro! Você vai precisar dela depois.

### Passo 4.3: Criar o Servidor

1. Clique em **"Revisar + criar"**
2. Aguarde a validação
3. Clique em **"Criar"**
4. Aguarde 2-5 minutos para a implantação

### Passo 4.4: Criar o Banco de Dados

1. Quando a implantação terminar, clique em **"Ir para o recurso"**
2. No menu à esquerda do servidor, clique em **"Bancos de dados SQL"**
3. Clique em **"+ Criar banco de dados"**

### Passo 4.5: Configurar o Banco de Dados

**Aba "Básico":**

| Campo | O que colocar |
|-------|---------------|
| **Nome do banco de dados** | `RaphaMoviesDB` |
| **Servidor** | Já vem preenchido |
| **Deseja usar pool elástico?** | Não |
| **Ambiente de carga de trabalho** | Desenvolvimento |

**Computação + armazenamento:**
1. Clique em **"Configurar banco de dados"**
2. Selecione **"Basic"** (o mais barato, ~$5/mês)
3. Clique em **"Aplicar"**

### Passo 4.6: Criar o Banco

1. Clique em **"Revisar + criar"**
2. Clique em **"Criar"**
3. Aguarde 1-2 minutos

### Passo 4.7: Configurar Firewall (Permitir Conexões)

> 💡 **Por que isso é necessário?**
> Por segurança, o Azure bloqueia todas as conexões por padrão. Precisamos permitir que nossos App Services se conectem.

1. Volte para o **Servidor SQL** (clique em "raphamovies-sqlserver" no breadcrumb)
2. No menu à esquerda, clique em **"Rede"** (ou "Networking")
3. Em **"Exceções"**, marque ☑️ **"Permitir que serviços e recursos do Azure acessem este servidor"**
4. Clique em **"Salvar"**

### Passo 4.8: Obter a Connection String

> 💡 **O que é Connection String?**
> É o "endereço completo" do banco de dados que o sistema usa para se conectar.

1. Vá para o **Banco de dados** `RaphaMoviesDB`
2. No menu à esquerda, clique em **"Cadeias de conexão"** (ou "Connection strings")
3. Copie a **ADO.NET (SQL authentication)**
4. Ela será parecida com:

```
Server=tcp:raphamovies-sqlserver.database.windows.net,1433;Initial Catalog=RaphaMoviesDB;Persist Security Info=False;User ID=sqladmin;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

5. **IMPORTANTE:** Substitua `{your_password}` pela senha real que você criou
6. Salve essa string em um arquivo de texto - você vai usar depois

### Passo 4.9: Criar as Tabelas do Banco

1. No banco de dados `RaphaMoviesDB`, clique em **"Editor de consultas (preview)"** no menu à esquerda
2. Faça login:
   - Login: `sqladmin`
   - Senha: sua senha
3. Cole e execute estes comandos SQL (um de cada vez):

```sql
-- Tabela de Usuários
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

Clique em **"Executar"** (▶️)

```sql
-- Tabela de Roles (Permissões)
CREATE TABLE UserRoles (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('admin', 'user')),
    UNIQUE(UserId, Role)
);
```

Clique em **"Executar"** (▶️)

```sql
-- Tabela de Filmes
CREATE TABLE Movies (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(255) NOT NULL,
    Synopsis NVARCHAR(MAX) NOT NULL,
    ImageUrl NVARCHAR(500) NOT NULL,
    BackdropUrl NVARCHAR(500) NOT NULL,
    TrailerUrl NVARCHAR(500) NULL,
    Year INT NOT NULL,
    Duration NVARCHAR(20) NOT NULL,
    Rating DECIMAL(3,1) NOT NULL,
    Genre NVARCHAR(50) NOT NULL,
    RentalPrice DECIMAL(10,2) NOT NULL DEFAULT 9.90,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

Clique em **"Executar"** (▶️)

```sql
-- Tabela de Aluguéis
CREATE TABLE Rentals (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    MovieId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Movies(Id),
    RentedAt DATETIME2 DEFAULT GETUTCDATE(),
    DueDate DATETIME2 NOT NULL,
    ReturnedAt DATETIME2 NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('active', 'returned', 'overdue')),
    TotalPrice DECIMAL(10,2) NOT NULL
);

CREATE INDEX IX_Rentals_UserId ON Rentals(UserId);
CREATE INDEX IX_Rentals_Status ON Rentals(Status);
```

Clique em **"Executar"** (▶️)

✅ **Sucesso!** Banco de dados configurado!

---

## 5. Criar App Service para o Backend (API)

> 💡 **O que é o Backend/API?**
> É o "cérebro" do sistema que processa login, busca filmes no banco, registra aluguéis, etc.

### Passo 5.1: Criar o App Service

1. No Portal Azure, clique em **"+ Criar um recurso"**
2. Pesquise: **App Service**
3. Selecione **"Aplicativo Web"** (Web App)
4. Clique em **"Criar"**

### Passo 5.2: Configurar o App Service

**Aba "Básico":**

| Campo | O que colocar |
|-------|---------------|
| **Assinatura** | Sua assinatura |
| **Grupo de recursos** | `rg-rapha-movies` |
| **Nome** | `raphamovies-api` (será: raphamovies-api.azurewebsites.net) |
| **Publicar** | Código |
| **Pilha de runtime** | `.NET 8 (LTS)` |
| **Sistema operacional** | Windows |
| **Região** | `Brazil South` |

**Plano do App Service:**
1. Clique em **"Criar novo"** em Plano do Windows
2. Nome: `plan-rapha-movies`
3. Clique em **"Alterar tamanho"**
4. Selecione **"B1"** (Basic) na aba "Desenvolvimento/Teste"
5. Clique em **"Aplicar"**

### Passo 5.3: Criar

1. Clique em **"Revisar + criar"**
2. Clique em **"Criar"**
3. Aguarde 1-2 minutos

### Passo 5.4: Configurar Connection String

1. Vá para o App Service `raphamovies-api`
2. No menu à esquerda, clique em **"Configuração"** (ou "Configuration")
3. Clique na aba **"Cadeias de conexão"** (Connection strings)
4. Clique em **"+ Nova cadeia de conexão"**
5. Preencha:

| Campo | Valor |
|-------|-------|
| **Nome** | `DefaultConnection` |
| **Valor** | Cole a connection string que você salvou (com a senha real) |
| **Tipo** | `SQLAzure` |

6. Clique em **"OK"**
7. Clique em **"Salvar"** (no topo)
8. Confirme clicando em **"Continuar"**

### Passo 5.5: Configurar Variáveis de Ambiente

1. Ainda em **"Configuração"**, clique na aba **"Configurações de aplicativo"**
2. Adicione cada uma dessas configurações clicando em **"+ Nova configuração de aplicativo"**:

| Nome | Valor |
|------|-------|
| `Jwt__Secret` | `SuaChaveSecretaComPeloMenos32CaracteresParaSerSegura123!` |
| `Jwt__Issuer` | `RaphaMovies.Api` |
| `Jwt__Audience` | `RaphaMovies.Frontend` |
| `Jwt__ExpirationMinutes` | `60` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

3. Clique em **"Salvar"**
4. Confirme clicando em **"Continuar"**

✅ **App Service do Backend configurado!**

---

## 6. Criar App Service para o Frontend

> 💡 **O que é o Frontend?**
> É a parte visual do sistema - as telas que os usuários veem e interagem.

### Passo 6.1: Criar o App Service

1. No Portal Azure, clique em **"+ Criar um recurso"**
2. Pesquise: **App Service**
3. Selecione **"Aplicativo Web"**
4. Clique em **"Criar"**

### Passo 6.2: Configurar

**Aba "Básico":**

| Campo | O que colocar |
|-------|---------------|
| **Assinatura** | Sua assinatura |
| **Grupo de recursos** | `rg-rapha-movies` |
| **Nome** | `raphamovies-frontend` |
| **Publicar** | Código |
| **Pilha de runtime** | `Node 20 LTS` |
| **Sistema operacional** | Windows |
| **Região** | `Brazil South` |
| **Plano do Windows** | Selecione `plan-rapha-movies` (o que já criamos) |

### Passo 6.3: Criar

1. Clique em **"Revisar + criar"**
2. Clique em **"Criar"**
3. Aguarde 1-2 minutos

✅ **App Service do Frontend criado!**

---

## 7. Configurar Deploy Automático via GitHub

> 💡 **Por que isso é especial neste projeto?**
> O backend (.NET Core) e o frontend (React) estão **no mesmo repositório**, mas precisam ser publicados em **App Services diferentes**. Os workflows do GitHub Actions já estão configurados para lidar com isso automaticamente.

### Como funciona?

```
┌─────────────────────────────────────────────────────────────────────┐
│                    REPOSITÓRIO GITHUB                                │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  📁 / (raiz)              → Frontend React/Vite                     │
│  📁 /backend/             → Backend .NET Core                       │
│  📁 /docs/                → Documentação                            │
│                                                                      │
├─────────────────────────────────────────────────────────────────────┤
│                    WORKFLOWS (GitHub Actions)                        │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  🔄 azure-webapp-deploy.yml                                         │
│     ✓ Executa quando há mudanças FORA de /backend                   │
│     ✓ Faz deploy do FRONTEND para raphamovies-frontend              │
│                                                                      │
│  🔄 main_raphamovies-api-hml.yml                                    │
│     ✓ Executa quando há mudanças em /backend                        │
│     ✓ Faz deploy do BACKEND para raphamovies-api-hml                │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### Passo 7.1: Conectar Lovable ao GitHub

1. No Lovable, clique no nome do projeto (canto superior esquerdo)
2. Clique em **"Settings"**
3. Clique na aba **"GitHub"**
4. Clique em **"Connect to GitHub"**
5. Autorize o Lovable a acessar sua conta GitHub
6. Clique em **"Create Repository"**
7. Nome sugerido: `rapha-movies`
8. Clique em **"Create and push"**

### Passo 7.2: Configurar Deploy do Backend via Deployment Center

> 💡 **O que é Deployment Center?**
> É uma forma fácil de conectar seu App Service ao GitHub e configurar deploy automático diretamente pelo Portal Azure.

**⚠️ IMPORTANTE:** O Azure Deployment Center vai tentar criar um novo workflow. Como já temos os workflows configurados, você tem duas opções:

#### Opção A: Usar os Workflows Existentes (Recomendado)

Os workflows já estão prontos no repositório:
- `.github/workflows/main_raphamovies-api-hml.yml` → Backend
- `.github/workflows/azure-webapp-deploy.yml` → Frontend

Você só precisa configurar os **Secrets** no GitHub (veja Passo 7.3).

#### Opção B: Usar Deployment Center (Se preferir configurar pelo Azure)

1. No Portal Azure, vá para o App Service do backend (`raphamovies-api-hml`)
2. No menu à esquerda, clique em **"Centro de Implantação"** (Deployment Center)
3. Configure:
   - **Origem**: GitHub
   - **Organização**: Seu usuário/organização do GitHub
   - **Repositório**: `rapha-movies`
   - **Branch**: `main`
4. **IMPORTANTE**: O Azure vai perguntar sobre o tipo de build
   - Selecione: **.NET Core**
5. Clique em **"Salvar"**

**⚠️ Se o Azure criar um novo workflow:**

O workflow criado automaticamente não sabe que o projeto .NET está em `/backend/RaphaMovies.API/`. 

O Azure vai criar um arquivo na pasta `.github/workflows/` com um nome como `main_raphamovies-api-hml.yml` (ou similar). Você precisará editar esse arquivo no GitHub:

**Arquivo a editar:** `.github/workflows/main_raphamovies-api-hml.yml` (ou o nome que o Azure gerou)

```yaml
# Adicione esta linha no início do arquivo (após o bloco 'on:'):
env:
  BACKEND_PATH: backend/RaphaMovies.API

# E altere todos os comandos dotnet para usar working-directory:
- name: Restore dependencies
  run: dotnet restore
  working-directory: ${{ env.BACKEND_PATH }}

- name: Build with dotnet
  run: dotnet build --configuration Release --no-restore
  working-directory: ${{ env.BACKEND_PATH }}

- name: Publish
  run: dotnet publish -c Release -o ./publish --no-build
  working-directory: ${{ env.BACKEND_PATH }}
```

### Passo 7.3: Configurar Secrets no GitHub

> 💡 **O que são Secrets?**
> São informações sensíveis (como senhas) que ficam guardadas de forma segura no GitHub.

1. Vá para seu repositório no GitHub (github.com/seu-usuario/rapha-movies)
2. Clique na aba **"Settings"** (engrenagem)
3. No menu à esquerda, clique em **"Secrets and variables"** → **"Actions"**
4. Clique em **"New repository secret"**

---

#### 7.3.1 - Secrets para o BACKEND (configuramos no 7.2)

O workflow do backend usa autenticação **OIDC** (sem senha). Quando você conectou via Deployment Center, o Azure criou automaticamente estes secrets:
- `AZUREAPPSERVICE_CLIENTID_xxx`
- `AZUREAPPSERVICE_TENANTID_xxx`
- `AZUREAPPSERVICE_SUBSCRIPTIONID_xxx`

**Verifique se eles existem** no GitHub em Settings → Secrets → Actions.

---

#### 7.3.2 - Secrets para o FRONTEND

Agora configure os secrets do frontend:

**Secret 1:**
| Campo | Valor |
|-------|-------|
| Name | `AZURE_WEBAPP_NAME` |
| Secret | `raphamovies-frontend` |

Clique em **"Add secret"**

**Secret 2:**
| Campo | Valor |
|-------|-------|
| Name | `VITE_API_URL` |
| Secret | `https://raphamovies-api-hml.azurewebsites.net/api` |

Clique em **"Add secret"**

**Secret 3 - Publish Profile do Frontend:**

Para este, precisamos obter do Azure:

1. Vá para o Portal Azure
2. Acesse o App Service `raphamovies-frontend`
3. Clique em **"Obter perfil de publicação"** (ou "Get publish profile") - botão no topo
4. Um arquivo `.PublishSettings` será baixado
5. Abra este arquivo com o Bloco de Notas
6. Selecione TODO o conteúdo (Ctrl+A) e copie (Ctrl+C)

Volte ao GitHub:
| Campo | Valor |
|-------|-------|
| Name | `AZURE_WEBAPP_PUBLISH_PROFILE` |
| Secret | Cole todo o conteúdo do arquivo |

Clique em **"Add secret"**

---

#### Secrets para o Backend (OIDC - Federated Identity):

O workflow do backend usa autenticação OIDC. O Azure Deployment Center configura automaticamente estes secrets:
- `AZUREAPPSERVICE_CLIENTID_xxx`
- `AZUREAPPSERVICE_TENANTID_xxx`
- `AZUREAPPSERVICE_SUBSCRIPTIONID_xxx`

**⚠️ ERRO COMUM: "No matching federated identity record found"**

Se você receber este erro no GitHub Actions:
```
Error: AADSTS700213: No matching federated identity record found for presented assertion subject 'repo:seu-usuario/seu-repo:environment:Production'
```

Isso significa que a **Federated Identity Credential** não está configurada corretamente. Siga os passos abaixo:

### Passo 7.3.1: Configurar Federated Identity Credential

1. Acesse o [Portal Azure](https://portal.azure.com)
2. Vá para **Microsoft Entra ID** (antigo Azure Active Directory)
3. No menu à esquerda, clique em **App registrations**
4. Encontre o App Registration criado pelo Deployment Center (ou crie um novo)

**No App Registration:**

5. Clique em **Certificates & secrets**
6. Clique na aba **Federated credentials**
7. Clique em **+ Add credential**
8. Selecione **GitHub Actions deploying Azure resources**
9. Preencha EXATAMENTE como está no workflow:

| Campo | Valor | Explicação |
|-------|-------|------------|
| **Organization** | `raphasi` | Seu usuário ou organização do GitHub |
| **Repository** | `rapha-s-azure-flix-db-api` | Nome do repositório (verifique no GitHub) |
| **Entity type** | `Environment` | Porque o workflow usa `environment: Production` |
| **Environment name** | `Production` | Exatamente como está no workflow (maiúscula) |
| **Name** | `github-actions-production` | Qualquer nome descritivo |

10. Clique em **Add**

### Passo 7.3.2: Verificar Permissões do App Registration

O App Registration precisa de permissão **Contributor** no Web App:

1. Vá para o **Web App** do backend (`raphamovies-api-hml`)
2. Clique em **Access control (IAM)**
3. Clique em **+ Add** → **Add role assignment**
4. Selecione a role **Contributor**
5. Na aba **Members**, clique em **+ Select members**
6. Pesquise e selecione o App Registration
7. Clique em **Review + assign**

### Alternativa: Usar Publish Profile (Mais Simples)

Se preferir não usar OIDC, você pode modificar o workflow do backend para usar **Publish Profile** igual ao frontend:

1. Obtenha o Publish Profile do App Service do backend:
   - Vá para o App Service `raphamovies-api-hml`
   - Clique em **"Obter perfil de publicação"**
   - Copie todo o conteúdo do arquivo

2. Crie um secret no GitHub:
   | Name | Secret |
   |------|--------|
   | `AZURE_BACKEND_PUBLISH_PROFILE` | Conteúdo do arquivo |

3. Modifique o workflow `.github/workflows/main_raphamovies-api-hml.yml`:
   - Remova a seção de login OIDC
   - Use o publish-profile igual ao frontend

Exemplo de como ficaria o job de deploy:

```yaml
deploy:
  runs-on: ubuntu-latest
  needs: build
  environment:
    name: 'Production'
    url: ${{ steps.deploy-to-webapp.outputs.webapp-url }}

  steps:
    - name: Download artifact from build job
      uses: actions/download-artifact@v4
      with:
        name: .net-app
        path: ./publish

    - name: Deploy to Azure Web App
      id: deploy-to-webapp
      uses: azure/webapps-deploy@v3
      with:
        app-name: 'raphamovies-api-hml'
        publish-profile: ${{ secrets.AZURE_BACKEND_PUBLISH_PROFILE }}
        package: ./publish
```

### Passo 7.4: Executar o Deploy

1. No GitHub, vá para a aba **"Actions"**
2. Você verá dois workflows:
   - **"Build and Deploy Frontend to Azure Web App"**
   - **"Build and deploy ASP.Net Core app to Azure Web App - raphamovies-api-hml"**
3. Para executar manualmente, clique no workflow → **"Run workflow"** → **"Run workflow"**
4. Aguarde o processo (2-5 minutos cada)
5. Quando ficar verde (✓), o deploy foi concluído!

### Passo 7.5: Verificar o Deploy

**Frontend:**
1. Acesse: `https://raphamovies-frontend.azurewebsites.net`
2. O site deve carregar

**Backend:**
1. Acesse: `https://raphamovies-api-hml.azurewebsites.net/swagger`
2. Você deve ver a documentação Swagger da API

---

## 8. Testar a Aplicação

### 8.1: Verificar o Frontend

1. Acesse: `https://raphamovies-frontend.azurewebsites.net`
2. Deve aparecer a página inicial do Rapha Movies
3. Se aparecer erro, veja a seção de Solução de Problemas

### 8.2: Verificar o Backend

1. Acesse: `https://raphamovies-api-hml.azurewebsites.net/swagger`
2. Você deve ver a documentação Swagger da API
3. Teste o endpoint `GET /api/movies` para verificar se retorna dados

> ✅ **O backend .NET Core já está incluído neste projeto!** 
> Ele está na pasta `backend/RaphaMovies.API/` e será publicado automaticamente via GitHub Actions.

### 8.3: Verificar o Banco

1. No Portal Azure, vá para `RaphaMoviesDB`
2. Clique em **"Editor de consultas"**
3. Execute: `SELECT * FROM Movies`
4. Se não houver erro, as tabelas estão corretas

---

## 9. Solução de Problemas

### Problema: Página em branco no Frontend

**Possíveis causas e soluções:**

1. **Verificar logs:**
   - No Portal Azure → App Service → **"Fluxo de logs"** (Log stream)
   - Procure por mensagens de erro

2. **Verificar se o build foi bem-sucedido:**
   - No GitHub → Actions → Clique no workflow mais recente
   - Veja se todos os passos estão verdes

### Problema: Erro 500 (Internal Server Error)

1. **Verificar Application Insights:**
   - No App Service → **"Application Insights"** → **"Falhas"**

2. **Verificar configurações:**
   - App Service → Configuração → Verifique se todas as variáveis estão corretas

### Problema: API não conecta ao banco

1. **Verificar firewall:**
   - SQL Server → Rede → Deve ter "Permitir serviços Azure" marcado

2. **Verificar connection string:**
   - A senha está correta?
   - O nome do servidor está correto?

### Problema: CORS (bloqueio de requisições)

Se o frontend não consegue chamar a API, pode ser CORS:

1. No backend .NET, verifique se o CORS está configurado para:
   ```
   https://raphamovies-frontend.azurewebsites.net
   ```

---

## 10. Glossário de Termos

| Termo | Significado |
|-------|-------------|
| **App Service** | Serviço do Azure para hospedar aplicações web |
| **SQL Database** | Banco de dados na nuvem do Azure |
| **Frontend** | Parte visual do sistema (o que o usuário vê) |
| **Backend/API** | Parte lógica do sistema (processa dados) |
| **Connection String** | "Endereço" para conectar ao banco de dados |
| **Deploy** | Processo de publicar/atualizar a aplicação |
| **GitHub Actions** | Automação que faz o deploy automaticamente |
| **Secret** | Informação sensível guardada de forma segura |
| **CORS** | Configuração de segurança para permitir requisições entre sites diferentes |
| **IIS** | Servidor web do Windows que roda as aplicações |

---

## Próximos Passos

Após completar este guia, você terá:

- ✅ Conta Azure configurada
- ✅ Banco de dados SQL criado e com tabelas
- ✅ App Service do Frontend funcionando
- ✅ Deploy automático configurado

**O que falta:**

1. **Desenvolver o Backend .NET** - Contratar um desenvolvedor ou usar o documento `API_BACKEND_SPEC.md` como referência
2. **Publicar o Backend** - Depois de desenvolvido, fazer deploy no App Service do backend
3. **Inserir dados de teste** - Adicionar filmes no banco de dados
4. **Configurar domínio personalizado** (opcional) - Usar seu próprio domínio

---

## Suporte

Se precisar de ajuda:

1. **Documentação Azure:** https://docs.microsoft.com/azure
2. **Suporte Azure:** Portal Azure → Ajuda + suporte
3. **Comunidade:** Stack Overflow (tag: azure-app-service)
