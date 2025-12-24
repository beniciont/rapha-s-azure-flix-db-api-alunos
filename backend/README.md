# 🎬 Rapha Movies API - Guia Completo para Iniciantes

Este guia vai te ensinar passo a passo como configurar e rodar o backend da aplicação Rapha Movies.

---

## 📑 Índice

1. [O que você vai precisar instalar](#1-o-que-você-vai-precisar-instalar)
2. [Instalando o .NET 8](#2-instalando-o-net-8)
3. Instalando o editor (escolha um):
   - [3A. Visual Studio 2022](#3a-instalando-o-visual-studio-2022-opção-mais-fácil) (mais fácil, recomendado para iniciantes)
   - [3B. VS Code](#3b-instalando-o-vs-code-opção-mais-leve) (mais leve, para quem já conhece)
4. [Baixando o código do backend](#4-baixando-o-código-do-backend)
5. Abrindo o projeto:
   - [5A. No Visual Studio](#5a-abrindo-o-projeto-no-visual-studio)
   - [5B. No VS Code](#5b-abrindo-o-projeto-no-vs-code)
6. Configurando o banco:
   - [6A. Visual Studio](#6a-configurando-o-banco-de-dados-visual-studio)
   - [6B. VS Code](#6b-configurando-o-banco-de-dados-vs-code)
7. Executando a API:
   - [7A. Visual Studio](#7a-executando-a-api-visual-studio)
   - [7B. VS Code](#7b-executando-a-api-vs-code)
8. [Testando a API com Swagger](#8-testando-a-api-com-swagger)
9. [Publicando no Azure](#9-publicando-no-azure)
10. [Conectando o Frontend](#10-conectando-o-frontend)
11. [Solução de Problemas](#11-solução-de-problemas)

---

## 1. O que você vai precisar instalar

Antes de começar, você precisa instalar 2 programas no seu computador.

### 🤔 Visual Studio ou VS Code?

Você pode escolher entre duas opções de editor:

| Característica | Visual Studio 2022 | VS Code |
|----------------|-------------------|---------|
| **Tamanho** | ~8 GB | ~300 MB |
| **Instalação** | 30-60 minutos | 5-10 minutos |
| **Facilidade** | Mais fácil (tudo integrado) | Requer extensões |
| **Para quem** | Iniciantes | Quem já conhece VS Code |

**Minha recomendação**:
- Se você **nunca programou antes** → Use o **Visual Studio 2022** (seção 3A)
- Se você **já usa VS Code** → Use o **VS Code** (seção 3B)

### Programas necessários

| Programa | Para que serve | Tamanho |
|----------|---------------|---------|
| .NET 8 SDK | Permite rodar código .NET | ~500 MB |
| Visual Studio 2022 **OU** VS Code | Editor para abrir e rodar o projeto | ~8 GB ou ~300 MB |

---

## 2. Instalando o .NET 8

### Passo 2.1: Acessar o site de download

1. Abra seu navegador (Chrome, Edge, Firefox)
2. Digite na barra de endereço: **https://dotnet.microsoft.com/download/dotnet/8.0**
3. Pressione **Enter**

### Passo 2.2: Baixar o instalador

1. Na página que abrir, procure a seção **.NET 8.0 (LTS)**
2. Clique no botão **Download .NET SDK x64** (para Windows)
   
   ```
   ┌─────────────────────────────────────────────┐
   │  .NET 8.0 (LTS)                             │
   │                                             │
   │  [Download .NET SDK x64]  ← Clique aqui     │
   │                                             │
   └─────────────────────────────────────────────┘
   ```

3. Um arquivo chamado algo como `dotnet-sdk-8.0.xxx-win-x64.exe` será baixado

### Passo 2.3: Instalar

1. Abra a pasta **Downloads** do seu computador
2. Dê **duplo clique** no arquivo que você baixou
3. Clique em **Sim** se aparecer uma janela perguntando permissão
4. Na janela do instalador, clique em **Install**
5. Aguarde a instalação (cerca de 2-3 minutos)
6. Clique em **Close** quando terminar

### Passo 2.4: Verificar se instalou corretamente

1. Pressione as teclas **Windows + R** ao mesmo tempo
2. Digite **cmd** e pressione **Enter**
3. Na janela preta que abrir, digite:
   ```
   dotnet --version
   ```
4. Pressione **Enter**
5. Deve aparecer algo como: `8.0.xxx`

Se aparecer um número começando com 8, a instalação foi um sucesso!

---

## 3A. Instalando o Visual Studio 2022 (Opção mais fácil)

> ⏭️ **Se você preferir usar o VS Code**, pule para a [seção 3B](#3b-instalando-o-vs-code-opção-mais-leve)

### Passo 3A.1: Acessar o site de download

1. Abra seu navegador
2. Digite na barra de endereço: **https://visualstudio.microsoft.com/pt-br/downloads/**
3. Pressione **Enter**

### Passo 3A.2: Baixar a versão Community (gratuita)

1. Na página, procure **Visual Studio 2022**
2. Abaixo de **Community** (versão gratuita), clique em **Download gratuito**

   ```
   ┌─────────────────────────────────────────────┐
   │  Visual Studio 2022                         │
   │                                             │
   │  Community          Pro          Enterprise │
   │  [Download gratuito]                        │
   │       ↑                                     │
   │   Clique aqui                               │
   └─────────────────────────────────────────────┘
   ```

### Passo 3A.3: Executar o instalador

1. Abra a pasta **Downloads**
2. Dê **duplo clique** no arquivo `VisualStudioSetup.exe`
3. Clique em **Sim** se pedir permissão
4. Aguarde carregar (pode demorar alguns minutos)

### Passo 3A.4: Selecionar os componentes

Uma janela vai abrir com várias opções. Você precisa marcar:

1. **ASP.NET e desenvolvimento Web** ← OBRIGATÓRIO
2. **Desenvolvimento do Azure** ← Opcional, mas recomendado

```
┌─────────────────────────────────────────────────────────────┐
│  Visual Studio Installer                                    │
│                                                             │
│  Cargas de trabalho                                         │
│                                                             │
│  ☑️ ASP.NET e desenvolvimento Web       ← Marque esta       │
│  ☑️ Desenvolvimento do Azure            ← Marque esta       │
│  ☐ Desenvolvimento para desktop .NET                        │
│  ☐ Desenvolvimento móvel com .NET                           │
│                                                             │
│                              [Instalar enquanto baixa]      │
└─────────────────────────────────────────────────────────────┘
```

### Passo 3A.5: Iniciar a instalação

1. Clique no botão **Instalar enquanto baixa** (canto inferior direito)
2. Aguarde a instalação (pode demorar 20-40 minutos dependendo da internet)
3. Quando terminar, clique em **Iniciar**

### Passo 3A.6: Configuração inicial

1. Na primeira vez, vai pedir para fazer login com conta Microsoft
   - Você pode clicar em **Agora não, talvez mais tarde** para pular
2. Escolha um tema de cores (Escuro ou Claro)
3. Clique em **Iniciar Visual Studio**

> ✅ **Pronto!** Agora pule para a [seção 4](#4-baixando-o-código-do-backend)

---

## 3B. Instalando o VS Code (Opção mais leve)

> ⏭️ **Se você já instalou o Visual Studio 2022**, pule para a [seção 4](#4-baixando-o-código-do-backend)

### Passo 3B.1: Baixar o VS Code

1. Abra seu navegador
2. Digite na barra de endereço: **https://code.visualstudio.com/**
3. Clique no botão grande **Download for Windows**

### Passo 3B.2: Instalar o VS Code

1. Abra a pasta **Downloads**
2. Dê **duplo clique** no arquivo `VSCodeUserSetup-xxx.exe`
3. Aceite os termos e clique em **Próximo** várias vezes
4. Marque a opção **Adicionar ao PATH** (importante!)
5. Clique em **Instalar**
6. Clique em **Concluir**

### Passo 3B.3: Instalar a extensão C#

1. Abra o **VS Code**
2. Clique no ícone de **Extensões** na barra lateral esquerda (ou pressione `Ctrl+Shift+X`)

   ```
   ┌─────────────────────────────────────────┐
   │  🔲  Explorer                            │
   │  🔍  Pesquisar                           │
   │  📦  Extensões  ← Clique aqui            │
   │  ⚙️  Configurações                       │
   └─────────────────────────────────────────┘
   ```

3. Na barra de pesquisa, digite: **C# Dev Kit**
4. Clique na extensão **C# Dev Kit** (da Microsoft)
5. Clique no botão **Install** (Instalar)

   ```
   ┌─────────────────────────────────────────────────────────────┐
   │  🔍 C# Dev Kit                                              │
   │                                                             │
   │  C# Dev Kit                                                 │
   │  Microsoft                           [Install]              │
   │  ⭐⭐⭐⭐⭐ (milhões de downloads)        ↑                  │
   │                                    Clique aqui              │
   └─────────────────────────────────────────────────────────────┘
   ```

6. Aguarde a instalação (vai instalar automaticamente a extensão C# também)

### Passo 3B.4: Reiniciar o VS Code

1. Feche o VS Code
2. Abra novamente

> ✅ **Pronto!** Agora continue para a [seção 4](#4-baixando-o-código-do-backend)

---

## 4. Baixando o código do backend

### Opção A: Se o projeto está no GitHub

1. Acesse o repositório do projeto no GitHub
2. Clique no botão verde **Code**
3. Clique em **Download ZIP**
4. Extraia o arquivo ZIP em uma pasta de sua escolha (ex: `C:\Projetos\`)

### Opção B: Se você está usando o Lovable

1. No Lovable, clique em **GitHub** no menu superior
2. Faça a conexão com sua conta GitHub (se ainda não fez)
3. O código será sincronizado automaticamente
4. Clone o repositório para seu computador:
   - Abra o **cmd** (Windows + R, digite cmd, Enter)
   - Digite:
     ```
     cd C:\Projetos
     git clone https://github.com/SEU_USUARIO/SEU_REPOSITORIO.git
     ```

---

## 5A. Abrindo o projeto no Visual Studio

> ⏭️ **Se você está usando VS Code**, pule para a [seção 5B](#5b-abrindo-o-projeto-no-vs-code)

### Passo 5A.1: Abrir o Visual Studio

1. Clique no menu **Iniciar** do Windows
2. Digite **Visual Studio 2022**
3. Clique para abrir

### Passo 5A.2: Abrir o projeto

1. Na tela inicial, clique em **Abrir um projeto ou solução**

   ```
   ┌─────────────────────────────────────────────────────────────┐
   │  Visual Studio 2022                                         │
   │                                                             │
   │  Introdução                                                 │
   │                                                             │
   │  🔵 Clonar um repositório                                   │
   │  📁 Abrir um projeto ou solução    ← Clique aqui           │
   │  📁 Abrir uma pasta local                                   │
   │  ➕ Criar um projeto                                        │
   │                                                             │
   └─────────────────────────────────────────────────────────────┘
   ```

2. Navegue até a pasta onde você salvou o projeto
3. Entre na pasta `backend` → `RaphaMovies.API`
4. Selecione o arquivo **RaphaMovies.API.csproj**
5. Clique em **Abrir**

### Passo 5A.3: Aguardar o carregamento

1. O Visual Studio vai carregar o projeto (pode demorar 1-2 minutos na primeira vez)
2. Você vai ver uma barra de progresso na parte inferior
3. Aguarde até aparecer "Pronto" na barra de status

> ✅ **Pronto!** Agora pule para a [seção 6A](#6a-configurando-o-banco-de-dados-visual-studio)

---

## 5B. Abrindo o projeto no VS Code

> ⏭️ **Se você está usando Visual Studio 2022**, pule para a [seção 6A](#6a-configurando-o-banco-de-dados-visual-studio)

### Passo 5B.1: Abrir a pasta do projeto

1. Abra o **VS Code**
2. Clique em **File** (Arquivo) → **Open Folder** (Abrir Pasta)
3. Navegue até a pasta onde você salvou o projeto
4. Selecione a pasta `backend/RaphaMovies.API`
5. Clique em **Selecionar Pasta**

### Passo 5B.2: Confiar na pasta

1. Uma janela vai aparecer perguntando se você confia nos autores
2. Clique em **Yes, I trust the authors** (Sim, confio nos autores)

### Passo 5B.3: Aguardar o carregamento

1. O VS Code vai detectar o projeto .NET automaticamente
2. Pode aparecer uma notificação pedindo para restaurar dependências
3. Clique em **Restore** se aparecer

> ✅ **Pronto!** Agora continue para a [seção 6B](#6b-configurando-o-banco-de-dados-vs-code)

---

## 6A. Configurando o banco de dados (Visual Studio)

> ⏭️ **Se você está usando VS Code**, pule para a [seção 6B](#6b-configurando-o-banco-de-dados-vs-code)

### Passo 6A.1: Abrir o Console do Gerenciador de Pacotes

1. No menu superior, clique em **Ferramentas**
2. Passe o mouse em **Gerenciador de Pacotes NuGet**
3. Clique em **Console do Gerenciador de Pacotes**

   ```
   Ferramentas
   └── Gerenciador de Pacotes NuGet
       └── Console do Gerenciador de Pacotes  ← Clique aqui
   ```

4. Uma janela vai abrir na parte inferior do Visual Studio

### Passo 6A.2: Criar as tabelas do banco de dados

1. Na janela do Console que abriu, digite:
   ```
   Add-Migration InitialCreate
   ```
2. Pressione **Enter**
3. Aguarde até aparecer "Build succeeded" (construção bem-sucedida)

### Passo 6A.3: Aplicar as tabelas no banco

1. Ainda no Console, digite:
   ```
   Update-Database
   ```
2. Pressione **Enter**
3. Aguarde até aparecer "Done" (feito)

**O que aconteceu?**
- Um banco de dados local chamado `RaphaMoviesDev` foi criado
- As tabelas de usuários, filmes e aluguéis foram criadas
- 10 filmes de exemplo foram adicionados
- Um usuário admin foi criado automaticamente

> ✅ **Pronto!** Agora pule para a [seção 7A](#7a-executando-a-api-visual-studio)

---

## 6B. Configurando o banco de dados (VS Code)

> ⏭️ **Se você está usando Visual Studio 2022**, pule para a [seção 7A](#7a-executando-a-api-visual-studio)

### Passo 6B.1: Abrir o Terminal

1. No VS Code, clique em **Terminal** no menu superior
2. Clique em **New Terminal** (Novo Terminal)
3. Um terminal vai abrir na parte inferior da tela

   ```
   ┌─────────────────────────────────────────┐
   │  Terminal  ← Clique aqui                │
   │  └── New Terminal                       │
   └─────────────────────────────────────────┘
   ```

### Passo 6B.2: Instalar a ferramenta EF Core (apenas uma vez)

1. No terminal, digite:
   ```
   dotnet tool install --global dotnet-ef
   ```
2. Pressione **Enter**
3. Se aparecer "já está instalado", tudo bem, continue

### Passo 6B.3: Restaurar as dependências

1. No terminal, digite:
   ```
   dotnet restore
   ```
2. Pressione **Enter**
3. Aguarde terminar

### Passo 6B.4: Criar as tabelas do banco de dados

1. No terminal, digite:
   ```
   dotnet ef migrations add InitialCreate
   ```
2. Pressione **Enter**
3. Aguarde aparecer "Done"

### Passo 6B.5: Aplicar as tabelas no banco

1. No terminal, digite:
   ```
   dotnet ef database update
   ```
2. Pressione **Enter**
3. Aguarde aparecer "Done"

**O que aconteceu?**
- Um banco de dados local chamado `RaphaMoviesDev` foi criado
- As tabelas de usuários, filmes e aluguéis foram criadas
- 10 filmes de exemplo foram adicionados
- Um usuário admin foi criado automaticamente

> ✅ **Pronto!** Agora continue para a [seção 7B](#7b-executando-a-api-vs-code)

---

## 7A. Executando a API (Visual Studio)

> ⏭️ **Se você está usando VS Code**, pule para a [seção 7B](#7b-executando-a-api-vs-code)

### Passo 7A.1: Iniciar a aplicação

1. No topo do Visual Studio, procure um botão verde com uma seta ▶️
2. Ao lado dele deve estar escrito **https** ou **IIS Express**
3. Clique no botão verde ▶️ (ou pressione **F5**)

   ```
   ┌──────────────────────────────────────────────────┐
   │  [▶️ https ▼]   ← Clique no botão verde          │
   └──────────────────────────────────────────────────┘
   ```

### Passo 7A.2: Aguardar o carregamento

1. Uma janela do navegador vai abrir automaticamente
2. Pode aparecer um aviso de segurança sobre certificado
   - Clique em **Avançado** e depois **Continuar mesmo assim**
3. A página do Swagger vai carregar

### Passo 7A.3: Verificar se está funcionando

Se você ver uma página parecida com esta, a API está funcionando:

```
┌─────────────────────────────────────────────────────────────┐
│  Rapha Movies API                                           │
│                                                             │
│  ▼ Admin                                                    │
│  ▼ Auth                                                     │
│  ▼ Movies                                                   │
│  ▼ Rentals                                                  │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

**Credenciais do administrador:**
- **Email**: admin@raphamovies.com
- **Senha**: Admin@123

> ✅ **Pronto!** Agora pule para a [seção 8](#8-testando-a-api-com-swagger)

---

## 7B. Executando a API (VS Code)

### Passo 7B.1: Iniciar a aplicação

1. No terminal do VS Code (que você abriu antes), digite:
   ```
   dotnet run
   ```
2. Pressione **Enter**
3. Aguarde aparecer algo como:
   ```
   info: Microsoft.Hosting.Lifetime[14]
         Now listening on: https://localhost:5001
         Now listening on: http://localhost:5000
   ```

### Passo 7B.2: Abrir o Swagger no navegador

1. Abra seu navegador
2. Digite na barra de endereço: **https://localhost:5001/swagger**
3. Pressione **Enter**
4. Se aparecer aviso de segurança, clique em **Avançado** → **Continuar**

### Passo 7B.3: Verificar se está funcionando

Se você ver uma página parecida com esta, a API está funcionando:

```
┌─────────────────────────────────────────────────────────────┐
│  Rapha Movies API                                           │
│                                                             │
│  ▼ Admin                                                    │
│  ▼ Auth                                                     │
│  ▼ Movies                                                   │
│  ▼ Rentals                                                  │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

**Credenciais do administrador:**
- **Email**: admin@raphamovies.com
- **Senha**: Admin@123

### Passo 7B.4: Parar a aplicação

- Para parar a API, volte ao terminal e pressione **Ctrl + C**

---

## 8. Testando a API com Swagger

O Swagger é uma ferramenta que permite testar a API diretamente no navegador.

### Passo 8.1: Testar o login

1. Na página do Swagger, clique em **Auth** para expandir
2. Clique em **POST /api/auth/login**
3. Clique no botão **Try it out** (lado direito)
4. No campo de texto, substitua o conteúdo por:
   ```json
   {
     "email": "admin@raphamovies.com",
     "password": "Admin@123"
   }
   ```
5. Clique no botão azul **Execute**

### Passo 8.2: Verificar a resposta

Se o login foi bem-sucedido, você verá uma resposta assim:

```json
{
  "user": {
    "id": "11111111-1111-1111-1111-111111111111",
    "email": "admin@raphamovies.com",
    "name": "Administrador"
  },
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "roles": ["admin"]
}
```

### Passo 8.3: Testar listagem de filmes

1. Clique em **Movies** para expandir
2. Clique em **GET /api/movies**
3. Clique em **Try it out**
4. Clique em **Execute**
5. Você verá a lista de 10 filmes cadastrados

---

## 9. Publicando no Azure

### Passo 9.1: Criar os recursos no Azure

Siga o guia detalhado em `docs/AZURE_DEPLOYMENT.md` para:
1. Criar um **Resource Group**
2. Criar um **Azure SQL Database**
3. Criar um **App Service**

### Passo 9.2: Configurar a Connection String

1. No Portal do Azure, acesse seu **App Service**
2. No menu lateral, clique em **Configuração**
3. Clique em **Cadeias de conexão**
4. Clique em **+ Nova cadeia de conexão**
5. Preencha:
   - **Nome**: `DefaultConnection`
   - **Valor**: (copie do Azure SQL Database)
   - **Tipo**: `SQLAzure`
6. Clique em **OK** e depois em **Salvar**

### Passo 9.3: Publicar pelo Visual Studio

1. No Visual Studio, clique com botão direito no projeto **RaphaMovies.API**
2. Clique em **Publicar...**

   ```
   RaphaMovies.API (botão direito)
   └── Publicar...  ← Clique aqui
   ```

3. Selecione **Azure** e clique em **Avançar**
4. Selecione **Serviço de Aplicativo do Azure (Windows)**
5. Faça login na sua conta Azure se necessário
6. Selecione seu App Service na lista
7. Clique em **Concluir**
8. Clique no botão **Publicar**

### Passo 9.4: Aguardar a publicação

1. O Visual Studio vai compilar e enviar o código para o Azure
2. Isso pode demorar 2-5 minutos
3. Quando terminar, o navegador vai abrir com a URL da sua API

### Passo 9.5: Testar a API no Azure

1. Adicione `/swagger` ao final da URL
   - Exemplo: `https://minha-api.azurewebsites.net/swagger`
2. Teste o login como fez localmente

---

## 10. Conectando o Frontend

Depois que a API estiver funcionando no Azure:

### Passo 10.1: Copiar a URL da API

1. No Portal do Azure, acesse seu App Service
2. Na página principal, copie a **URL** (algo como `https://raphamovies-api.azurewebsites.net`)

### Passo 10.2: Configurar no Lovable

1. Acesse o projeto no Lovable
2. Vá em **Settings** (Configurações)
3. Procure por **Environment Variables** ou **Variáveis de Ambiente**
4. Adicione:
   - **Nome**: `VITE_API_URL`
   - **Valor**: `https://sua-api.azurewebsites.net/api`
5. Salve as configurações

### Passo 10.3: Testar a aplicação

1. Acesse o frontend da aplicação
2. Tente fazer login com as credenciais do admin
3. Navegue pelo catálogo de filmes

---

## 11. Solução de Problemas

### ❌ Erro: "The term 'dotnet' is not recognized"

**Problema**: O .NET não foi instalado corretamente

**Solução**:
1. Reinstale o .NET 8 SDK
2. Reinicie o computador
3. Tente novamente

---

### ❌ Erro: "Unable to connect to SQL Server"

**Problema**: O banco de dados local não está acessível

**Solução**:
1. Abra o **SQL Server Configuration Manager**
2. Verifique se o **SQL Server (LOCALDB)** está rodando
3. Se não estiver, clique com botão direito e selecione **Iniciar**

---

### ❌ Erro: "A connection with the server could not be established"

**Problema**: O firewall está bloqueando a conexão

**Solução para Azure SQL**:
1. No Portal do Azure, acesse o SQL Server
2. Clique em **Rede**
3. Adicione seu IP nas regras de firewall

---

### ❌ Erro: "Login failed for user"

**Problema**: Credenciais do banco incorretas

**Solução**:
1. Verifique o usuário e senha no `appsettings.json`
2. Confirme que correspondem ao que você configurou no Azure SQL

---

### ❌ Erro: "CORS policy blocked"

**Problema**: O frontend não está autorizado a acessar a API

**Solução**: Configure as origens permitidas usando **uma** destas opções:

#### Opção 1: Variável de ambiente simples (RECOMENDADO)
No Azure Portal → App Service → **Configuration** → **Application settings**:
```
Nome: CORS_ORIGINS
Valor: https://seu-frontend.azurewebsites.net,https://seu-dominio.com
```
(Separe múltiplas URLs por vírgula, sem espaços)

#### Opção 2: App Settings individual por origem
```
Nome: Cors__AllowedOrigins__0
Valor: https://seu-frontend.azurewebsites.net

Nome: Cors__AllowedOrigins__1  
Valor: https://outro-dominio.com
```

#### Opção 3: appsettings.json (menos flexível)
```json
"Cors": {
  "AllowedOrigins": [
    "https://seu-frontend.lovable.app",
    "https://seu-dominio.com"
  ]
}
```

Após configurar, **reinicie o App Service** para aplicar as mudanças.

---

### ❌ A página do Swagger não abre

**Problema**: A aplicação não está rodando

**Solução**:
1. Verifique se há erros na janela **Saída** do Visual Studio
2. Pare a aplicação (Shift + F5)
3. Limpe a solução: **Compilar** → **Limpar Solução**
4. Inicie novamente (F5)

---

### ❌ Erro 500 ao fazer requisições

**Problema**: Erro interno na API

**Solução**:
1. Verifique os logs no Visual Studio (janela **Saída**)
2. Procure a mensagem de erro específica
3. Verifique se as migrações foram aplicadas

---

## 📞 Precisa de mais ajuda?

Se você ainda tiver problemas:

1. **Copie a mensagem de erro completa**
2. **Anote o que você estava tentando fazer**
3. **Volte ao Lovable e peça ajuda** - cole o erro e eu vou te ajudar a resolver!

---

## 📋 Resumo dos comandos

| Comando | O que faz |
|---------|-----------|
| `dotnet --version` | Verifica se o .NET está instalado |
| `Add-Migration InitialCreate` | Cria o script do banco de dados |
| `Update-Database` | Aplica as tabelas no banco |
| `F5` ou botão ▶️ | Inicia a aplicação |
| `Shift + F5` | Para a aplicação |

---

**Parabéns!** 🎉 Se você chegou até aqui, a API está funcionando!
