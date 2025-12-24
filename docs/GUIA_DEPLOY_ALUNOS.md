# 🎬 Rapha Movies - Guia de Deploy para Alunos

> **Tempo estimado:** 30-45 minutos  
> **Nível:** Iniciante (sem conhecimento prévio necessário)

---

## 📋 O que você vai precisar

✅ Uma conta Microsoft (pode criar gratuitamente)  
✅ Uma conta no GitHub (pode criar gratuitamente)  
✅ Um cartão de crédito (para verificação - **NÃO SERÁ COBRADO**)

---

## 🎯 Visão Geral

Você vai publicar um sistema de locadora de filmes na internet. O sistema tem 3 partes:

```
┌──────────────────────────────────────────────────────────────┐
│  🖥️  FRONTEND           📡 BACKEND (API)        🗄️ BANCO     │
│  (Telas que você vê)    (Lógica do sistema)    (Armazena    │
│                                                  dados)      │
│                                                              │
│  React/TypeScript       .NET 8                 SQL Server   │
│  App Service            App Service            SQL Database │
└──────────────────────────────────────────────────────────────┘
```

---

## 📚 Passo a Passo

### 🔷 ETAPA 1: Criar conta no Azure (5 min)

#### 1.1 Acessar o Azure
1. Abra o navegador
2. Acesse: **https://azure.microsoft.com/pt-br/free/**
3. Clique no botão **"Comece gratuitamente"**

#### 1.2 Fazer login ou criar conta
- **Se já tem conta Microsoft** (Hotmail, Outlook, Xbox): faça login
- **Se não tem**: clique em "Criar uma!" e siga as instruções

#### 1.3 Completar cadastro
1. Preencha seus dados pessoais
2. Verifique seu telefone (receberá SMS)
3. Adicione cartão de crédito (apenas verificação)
4. Aceite os termos e clique **"Inscrever-se"**

> ⚠️ **Importante:** O cartão é apenas para verificação. Você recebe R$1.000 de crédito grátis!

#### 1.4 Acessar o Portal
Após o cadastro, acesse: **https://portal.azure.com**

---

### 🔷 ETAPA 2: Criar Grupo de Recursos (2 min)

> 💡 Um Grupo de Recursos é como uma "pasta" que organiza tudo do seu projeto.

1. No Portal Azure, clique em **"Grupos de recursos"** (menu esquerdo)
2. Clique em **"+ Criar"**
3. Preencha:
   - **Grupo de recursos:** `rg-raphamovies-SEUNOME` (ex: rg-raphamovies-joao)
   - **Região:** `Brazil South`
4. Clique em **"Revisar + criar"**
5. Clique em **"Criar"**

✅ **Pronto!** Grupo criado.

---

### 🔷 ETAPA 3: Criar o Banco de Dados (10 min)

#### 3.1 Criar o Servidor SQL
1. Clique em **"+ Criar um recurso"**
2. Pesquise: **SQL Server**
3. Selecione **"SQL Server (servidor lógico)"**
4. Clique em **"Criar"**

#### 3.2 Configurar o Servidor
| Campo | O que colocar |
|-------|---------------|
| **Grupo de recursos** | Selecione o que você criou |
| **Nome do servidor** | `raphamovies-sql-SEUNOME` |
| **Região** | `Brazil South` |
| **Método de autenticação** | Usar autenticação SQL |
| **Login do administrador** | `sqladmin` |
| **Senha** | Crie uma senha forte (anote!) |

5. Clique em **"Revisar + criar"** → **"Criar"**
6. Aguarde 2-3 minutos

#### 3.3 Criar o Banco de Dados
1. Quando terminar, clique em **"Ir para o recurso"**
2. No menu esquerdo, clique em **"Bancos de dados SQL"**
3. Clique em **"+ Criar banco de dados"**
4. Configure:
   - **Nome:** `RaphaMoviesDB`
   - **Computação:** Clique em "Configurar" → Selecione **"Basic"** → **"Aplicar"**
5. Clique em **"Revisar + criar"** → **"Criar"**

#### 3.4 Configurar Firewall
1. Volte para o servidor SQL (clique no nome dele)
2. Menu esquerdo: **"Rede"**
3. Marque ☑️ **"Permitir que serviços do Azure acessem este servidor"**
4. Clique em **"Salvar"**

#### 3.5 Copiar Connection String
1. Vá para o banco **RaphaMoviesDB**
2. Menu esquerdo: **"Cadeias de conexão"**
3. Copie a **ADO.NET** e salve em um bloco de notas
4. **IMPORTANTE:** Substitua `{your_password}` pela sua senha real

Exemplo:
```
Server=tcp:raphamovies-sql-joao.database.windows.net,1433;Initial Catalog=RaphaMoviesDB;User ID=sqladmin;Password=SuaSenhaAqui123!;Encrypt=True;TrustServerCertificate=False;
```

#### 3.6 Criar as Tabelas
1. No banco RaphaMoviesDB, clique em **"Editor de consultas"**
2. Faça login com `sqladmin` e sua senha
3. Cole e execute cada bloco SQL abaixo (um por vez, clicando em ▶️ Executar):

**Bloco 1 - Tabela de Usuários:**
```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

**Bloco 2 - Tabela de Permissões:**
```sql
CREATE TABLE UserRoles (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('admin', 'user')),
    UNIQUE(UserId, Role)
);
```

**Bloco 3 - Tabela de Filmes:**
```sql
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

**Bloco 4 - Tabela de Aluguéis:**
```sql
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

✅ **Banco de dados pronto!**

---

### 🔷 ETAPA 4: Criar App Service do Backend (5 min)

#### 4.1 Criar o App Service
1. Clique em **"+ Criar um recurso"**
2. Pesquise: **App Service**
3. Selecione **"Aplicativo Web"** → **"Criar"**

#### 4.2 Configurar
| Campo | Valor |
|-------|-------|
| **Grupo de recursos** | Selecione o seu |
| **Nome** | `raphamovies-api-SEUNOME` |
| **Publicar** | Código |
| **Pilha de runtime** | `.NET 8 (LTS)` |
| **Sistema operacional** | Windows |
| **Região** | `Brazil South` |

**Plano do App Service:**
- Clique em **"Criar novo"**
- Nome: `plan-raphamovies-SEUNOME`
- Clique em **"Alterar tamanho"** → Selecione **"B1"** (Basic) → **"Aplicar"**

5. Clique em **"Revisar + criar"** → **"Criar"**

#### 4.3 Configurar Connection String
1. Vá para o App Service criado
2. Menu esquerdo: **"Configuração"**
3. Aba **"Cadeias de conexão"**
4. Clique **"+ Nova cadeia de conexão"**:
   - **Nome:** `DefaultConnection`
   - **Valor:** Cole sua connection string (com a senha real!)
   - **Tipo:** `SQLAzure`
5. Clique **"OK"** → **"Salvar"** → **"Continuar"**

#### 4.4 Configurar Variáveis de Ambiente
1. Na aba **"Configurações de aplicativo"**
2. Adicione cada uma clicando em **"+ Nova configuração"**:

| Nome | Valor |
|------|-------|
| `Jwt__Secret` | `MinhaChaveSecretaMuitoSegura2024RaphaMovies!@#` |
| `Jwt__Issuer` | `RaphaMovies.Api` |
| `Jwt__Audience` | `RaphaMovies.Frontend` |
| `Jwt__ExpirationMinutes` | `60` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

3. Clique **"Salvar"** → **"Continuar"**

#### 4.5 Obter Publish Profile
1. Na página principal do App Service
2. Clique em **"Baixar perfil de publicação"** (Download publish profile)
3. Um arquivo `.PublishSettings` será baixado
4. **Abra o arquivo com Bloco de Notas** e copie TODO o conteúdo

✅ **Backend configurado!**

---

### 🔷 ETAPA 5: Criar App Service do Frontend (3 min)

#### 5.1 Criar o App Service
1. **"+ Criar um recurso"** → **"App Service"** → **"Aplicativo Web"** → **"Criar"**

#### 5.2 Configurar
| Campo | Valor |
|-------|-------|
| **Grupo de recursos** | Selecione o seu |
| **Nome** | `raphamovies-frontend-SEUNOME` |
| **Publicar** | Código |
| **Pilha de runtime** | `Node 20 LTS` |
| **Sistema operacional** | Windows |
| **Região** | `Brazil South` |
| **Plano** | Selecione o que já criou (`plan-raphamovies-SEUNOME`) |

3. **"Revisar + criar"** → **"Criar"**

#### 5.3 Obter Publish Profile
1. Vá para o App Service do frontend
2. Clique em **"Baixar perfil de publicação"**
3. Abra com Bloco de Notas e copie TODO o conteúdo

✅ **Frontend configurado!**

---

### 🔷 ETAPA 6: Fazer Fork do Repositório (2 min)

> 💡 Fork = Cópia do projeto para sua conta GitHub

1. Acesse: **https://github.com/CONTA-DO-PROFESSOR/rapha-movies** (seu professor vai fornecer o link)
2. Clique no botão **"Fork"** (canto superior direito)
3. Selecione sua conta GitHub
4. Clique em **"Create fork"**
5. Aguarde... pronto! O projeto está na sua conta.

---

### 🔷 ETAPA 7: Configurar Secrets no GitHub (5 min)

> 💡 Secrets são senhas/configurações que o GitHub usa para publicar automaticamente.

#### 7.1 Acessar configurações
1. No seu repositório (fork), clique em **"Settings"** (aba superior)
2. Menu esquerdo: **"Secrets and variables"** → **"Actions"**

#### 7.2 Adicionar os Secrets
Clique em **"New repository secret"** para cada um:

| Nome do Secret | Valor |
|----------------|-------|
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Conteúdo do PublishSettings do **Frontend** |
| `AZURE_WEBAPP_NAME` | `raphamovies-frontend-SEUNOME` |
| `AZURE_BACKEND_PUBLISH_PROFILE` | Conteúdo do PublishSettings do **Backend** |
| `VITE_API_URL` | `https://raphamovies-api-SEUNOME.azurewebsites.net` |

> ⚠️ **ATENÇÃO:** Substitua "SEUNOME" pelos nomes reais que você usou nos App Services!

✅ **Secrets configurados!**

---

### 🔷 ETAPA 8: Fazer o Deploy (3 min)

#### 8.1 Ativar o Deploy Automático
1. No seu repositório, clique na aba **"Actions"**
2. Se aparecer botão **"I understand my workflows, go ahead and enable them"**, clique nele
3. Você verá os workflows listados

#### 8.2 Executar o Deploy do Backend
1. Clique em **"Build and deploy ASP.Net Core app to Azure Web App"**
2. Clique em **"Run workflow"** (botão à direita)
3. Selecione `main` e clique em **"Run workflow"**
4. Aguarde 3-5 minutos (ficará verde ✅ quando terminar)

#### 8.3 Executar o Deploy do Frontend
1. Clique em **"Build and Deploy Frontend to Azure Web App"**
2. Clique em **"Run workflow"** → `main` → **"Run workflow"**
3. Aguarde 2-3 minutos

✅ **Deploy concluído!**

---

### 🔷 ETAPA 9: Testar sua Aplicação (2 min)

#### 9.1 Testar o Backend
1. Acesse: `https://raphamovies-api-SEUNOME.azurewebsites.net/swagger`
2. Você deve ver a documentação da API

#### 9.2 Testar o Frontend
1. Acesse: `https://raphamovies-frontend-SEUNOME.azurewebsites.net`
2. Você deve ver o site da locadora de filmes!

#### 9.3 Criar um Usuário Admin
1. No site, clique em **"Entrar"** → **"Cadastre-se"**
2. Crie uma conta com seu email
3. No Azure, vá ao Editor de Consultas do banco e execute:

```sql
INSERT INTO UserRoles (Id, UserId, Role)
SELECT NEWID(), Id, 'admin'
FROM Users 
WHERE Email = 'seu-email@exemplo.com';
```

4. Agora você pode acessar o painel administrativo!

---

## 🎉 Parabéns!

Você acabou de fazer deploy de uma aplicação fullstack na nuvem! 🚀

---

## ❓ Problemas Comuns

### "O site mostra erro 500"
- Verifique se a Connection String está correta
- Verifique se as tabelas foram criadas no banco

### "O deploy falhou"
- Verifique se os secrets estão corretos
- Verifique se copiou TODO o conteúdo do PublishSettings

### "O site carrega mas não mostra filmes"
- O backend pode não estar rodando
- Teste: `https://raphamovies-api-SEUNOME.azurewebsites.net/health`

### "Não consigo fazer login"
- Verifique se criou as tabelas no banco
- Tente criar um novo usuário

---

## 📞 Precisa de Ajuda?

Entre em contato com seu professor ou monitor de turma.

---

## 📝 Checklist Final

- [ ] Conta Azure criada
- [ ] Grupo de Recursos criado
- [ ] Servidor SQL criado
- [ ] Banco de dados criado
- [ ] Tabelas criadas (4 tabelas)
- [ ] App Service Backend criado e configurado
- [ ] App Service Frontend criado
- [ ] Fork do repositório feito
- [ ] 4 Secrets configurados no GitHub
- [ ] Deploy do Backend executado ✅
- [ ] Deploy do Frontend executado ✅
- [ ] Site funcionando!

---

*Última atualização: Dezembro 2024*
