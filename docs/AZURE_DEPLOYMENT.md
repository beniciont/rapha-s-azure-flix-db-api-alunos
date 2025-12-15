# 🎬 Rapha Movies - Guia Completo de Implantação no Azure

Este documento contém todas as instruções necessárias para configurar e implantar o projeto Rapha Movies utilizando os serviços do Microsoft Azure.

---

## 📑 Índice

1. [Visão Geral da Arquitetura](#1-visão-geral-da-arquitetura)
2. [Pré-requisitos](#2-pré-requisitos)
3. [Exportar o Código do Lovable](#3-exportar-o-código-do-lovable)
4. [Criar Conta no Azure](#4-criar-conta-no-azure)
5. [Configurar Azure Blob Storage](#5-configurar-azure-blob-storage)
6. [Implantar no Azure Static Web Apps](#6-implantar-no-azure-static-web-apps)
7. [Configurar Domínio Personalizado](#7-configurar-domínio-personalizado-opcional)
8. [Gerenciar Imagens no Blob Storage](#8-gerenciar-imagens-no-blob-storage)
9. [Custos Estimados](#9-custos-estimados)
10. [Solução de Problemas](#10-solução-de-problemas)

---

## 1. Visão Geral da Arquitetura

```
┌─────────────────────────────────────────────────────────────────┐
│                         USUÁRIO                                  │
│                      (Navegador Web)                             │
└─────────────────────────┬───────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│                 AZURE STATIC WEB APPS                            │
│              (Frontend React/Vite)                               │
│                                                                  │
│  • Páginas: Home, Catálogo, Detalhes, Login, Admin              │
│  • Componentes: Header, MovieCard, Carousel, Hero               │
│  • Autenticação: Mock (simulada no frontend)                    │
│  • Dados: Mock (JSON estático)                                  │
└─────────────────────────┬───────────────────────────────────────┘
                          │
                          │ URLs das imagens
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│                 AZURE BLOB STORAGE                               │
│              (Armazenamento de Imagens)                          │
│                                                                  │
│  Container: movies/                                              │
│  ├── posters/                                                   │
│  │   ├── interestelar.jpg                                       │
│  │   ├── matrix.jpg                                             │
│  │   └── ...                                                    │
│  └── backdrops/                                                 │
│      ├── interestelar.jpg                                       │
│      └── ...                                                    │
└─────────────────────────────────────────────────────────────────┘
```

### Tecnologias Utilizadas

| Camada | Tecnologia | Serviço Azure |
|--------|------------|---------------|
| Frontend | React 18 + TypeScript + Vite | Azure Static Web Apps |
| Estilização | Tailwind CSS + shadcn/ui | - |
| Imagens | JPG/PNG/WebP | Azure Blob Storage |
| Roteamento | React Router DOM | - |

---

## 2. Pré-requisitos

### Ferramentas Necessárias

| Ferramenta | Versão | Download |
|------------|--------|----------|
| Node.js | 18+ | [nodejs.org](https://nodejs.org/) |
| Git | 2.40+ | [git-scm.com](https://git-scm.com/) |
| VS Code | Última | [code.visualstudio.com](https://code.visualstudio.com/) |
| Azure CLI | 2.50+ | [docs.microsoft.com/cli/azure](https://docs.microsoft.com/cli/azure/install-azure-cli) |

### Contas Necessárias

- [x] Conta GitHub (gratuita): [github.com](https://github.com/)
- [x] Conta Microsoft Azure (gratuita): [azure.microsoft.com](https://azure.microsoft.com/free/)

### Verificar Instalações

```bash
# Verificar Node.js
node --version
# Esperado: v18.x.x ou superior

# Verificar npm
npm --version
# Esperado: 9.x.x ou superior

# Verificar Git
git --version
# Esperado: git version 2.x.x

# Verificar Azure CLI (após instalação)
az --version
# Esperado: azure-cli 2.x.x
```

---

## 3. Exportar o Código do Lovable

### Opção A: Conectar ao GitHub (Recomendado)

1. No Lovable, clique no nome do projeto (canto superior esquerdo)
2. Selecione **"Settings"**
3. Vá para a aba **"GitHub"**
4. Clique em **"Connect to GitHub"**
5. Autorize o Lovable a acessar sua conta GitHub
6. Escolha **"Create new repository"**
7. Configure:
   - **Repository name:** `rapha-movies`
   - **Visibility:** Public ou Private
8. Clique em **"Create and push"**

O código será automaticamente enviado para o repositório.

### Opção B: Download Manual (ZIP)

1. No Lovable, clique no nome do projeto
2. Selecione **"Settings"**
3. Vá para a aba **"Export"**
4. Clique em **"Download as ZIP"**
5. Extraia o arquivo em uma pasta local

### Clonar o Repositório (se usou Opção A)

```bash
# Navegue para sua pasta de projetos
cd ~/projetos

# Clone o repositório
git clone https://github.com/SEU_USUARIO/rapha-movies.git

# Entre na pasta do projeto
cd rapha-movies

# Instale as dependências
npm install

# Teste localmente
npm run dev
```

Acesse `http://localhost:5173` para verificar se está funcionando.

---

## 4. Criar Conta no Azure

### 4.1. Criar Conta Gratuita

1. Acesse [azure.microsoft.com/free](https://azure.microsoft.com/free/)
2. Clique em **"Iniciar gratuitamente"**
3. Faça login com sua conta Microsoft (ou crie uma)
4. Preencha os dados:
   - Nome completo
   - Telefone (para verificação)
   - Cartão de crédito (apenas verificação, não será cobrado)
5. Aceite os termos e clique em **"Inscrever-se"**

**Benefícios da conta gratuita:**
- $200 de crédito por 30 dias
- 12 meses de serviços gratuitos
- Serviços sempre gratuitos (com limites)

### 4.2. Acessar o Portal Azure

1. Acesse [portal.azure.com](https://portal.azure.com)
2. Faça login com sua conta Microsoft
3. Você verá o Dashboard do Azure

### 4.3. Instalar e Configurar Azure CLI

```bash
# Windows (PowerShell como Admin)
winget install Microsoft.AzureCLI

# macOS
brew install azure-cli

# Linux (Ubuntu/Debian)
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# Fazer login no Azure CLI
az login
# Abrirá o navegador para autenticação

# Verificar assinatura ativa
az account show
```

---

## 5. Configurar Azure Blob Storage

### 5.1. Criar Grupo de Recursos

```bash
# Via Azure CLI
az group create \
  --name rg-rapha-movies \
  --location brazilsouth
```

**Via Portal Azure:**
1. Pesquise por **"Grupos de recursos"**
2. Clique em **"+ Criar"**
3. Configure:
   - **Assinatura:** Sua assinatura
   - **Grupo de recursos:** `rg-rapha-movies`
   - **Região:** Brazil South
4. Clique em **"Revisar + criar"** → **"Criar"**

### 5.2. Criar Storage Account

```bash
# Via Azure CLI
az storage account create \
  --name straphamovies \
  --resource-group rg-rapha-movies \
  --location brazilsouth \
  --sku Standard_LRS \
  --kind StorageV2 \
  --allow-blob-public-access true
```

**Via Portal Azure:**
1. Clique em **"+ Criar um recurso"**
2. Pesquise **"Storage account"** → Selecione → **"Criar"**
3. Configure:

| Campo | Valor |
|-------|-------|
| Assinatura | Sua assinatura ativa |
| Grupo de recursos | `rg-rapha-movies` |
| Nome da conta | `straphamovies` (único globalmente, apenas minúsculas e números) |
| Região | Brazil South |
| Desempenho | Standard |
| Redundância | LRS (Locally-redundant storage) |

4. Clique em **"Avançar: Avançado"**
5. Em **"Segurança"**, marque:
   - [x] Permitir acesso público ao Blob
6. Clique em **"Revisar + criar"** → **"Criar"**
7. Aguarde a implantação → **"Ir para o recurso"**

### 5.3. Criar Container para Imagens

```bash
# Via Azure CLI
az storage container create \
  --name movies \
  --account-name straphamovies \
  --public-access blob
```

**Via Portal Azure:**
1. Na Storage Account, menu lateral → **"Contêineres"**
2. Clique em **"+ Contêiner"**
3. Configure:
   - **Nome:** `movies`
   - **Nível de acesso público:** `Blob (acesso de leitura anônimo somente para blobs)`
4. Clique em **"Criar"**

### 5.4. Configurar CORS (Cross-Origin Resource Sharing)

```bash
# Via Azure CLI
az storage cors add \
  --services b \
  --methods GET HEAD OPTIONS \
  --origins "*" \
  --allowed-headers "*" \
  --exposed-headers "*" \
  --max-age 3600 \
  --account-name straphamovies
```

**Via Portal Azure:**
1. Na Storage Account → **"Configurações"** → **"Compartilhamento de recursos (CORS)"**
2. Em **"Serviço Blob"**, adicione:

| Campo | Valor |
|-------|-------|
| Origens permitidas | `*` |
| Métodos permitidos | `GET, HEAD, OPTIONS` |
| Cabeçalhos permitidos | `*` |
| Cabeçalhos expostos | `*` |
| Idade máxima | `3600` |

3. Clique em **"Salvar"**

### 5.5. Obter Connection String (para upload via CLI)

```bash
# Obter connection string
az storage account show-connection-string \
  --name straphamovies \
  --resource-group rg-rapha-movies \
  --query connectionString \
  --output tsv

# Salvar como variável de ambiente (Linux/Mac)
export AZURE_STORAGE_CONNECTION_STRING="<sua-connection-string>"

# Windows PowerShell
$env:AZURE_STORAGE_CONNECTION_STRING="<sua-connection-string>"
```

---

## 6. Implantar no Azure Static Web Apps

### 6.1. Criar Static Web App via Portal

1. No Portal Azure, clique em **"+ Criar um recurso"**
2. Pesquise **"Static Web Apps"** → Selecione → **"Criar"**
3. Configure:

**Aba Básico:**
| Campo | Valor |
|-------|-------|
| Assinatura | Sua assinatura |
| Grupo de recursos | `rg-rapha-movies` |
| Nome | `swa-rapha-movies` |
| Tipo de plano | Gratuito |
| Região | Brazil South |
| Origem da implantação | GitHub |

4. Clique em **"Entrar com o GitHub"** e autorize
5. Configure o repositório:

| Campo | Valor |
|-------|-------|
| Organização | Seu usuário GitHub |
| Repositório | `rapha-movies` |
| Branch | `main` |

6. Configure o Build:

| Campo | Valor |
|-------|-------|
| Predefinições de build | Vite |
| Local do aplicativo | `/` |
| Local da API | (deixe vazio) |
| Local de saída | `dist` |

7. Clique em **"Revisar + criar"** → **"Criar"**

### 6.2. Verificar Workflow do GitHub Actions

O Azure cria automaticamente um arquivo de workflow em `.github/workflows/`. Verifique:

```yaml
# .github/workflows/azure-static-web-apps-xxx.yml
name: Azure Static Web Apps CI/CD

on:
  push:
    branches:
      - main
  pull_request:
    types: [opened, synchronize, reopened, closed]
    branches:
      - main

jobs:
  build_and_deploy_job:
    runs-on: ubuntu-latest
    name: Build and Deploy Job
    steps:
      - uses: actions/checkout@v3
        with:
          submodules: true
          lfs: false
      - name: Build And Deploy
        id: builddeploy
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN_XXX }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: "upload"
          app_location: "/"
          api_location: ""
          output_location: "dist"
```

### 6.3. Acompanhar Implantação

1. No GitHub, vá para **Actions**
2. Você verá o workflow rodando
3. Aguarde o build (2-5 minutos)
4. Quando concluído (✓ verde), o site está no ar

### 6.4. Obter URL do Site

**Via Portal Azure:**
1. Vá para o recurso **Static Web App**
2. Na visão geral, copie a **URL** (ex: `https://blue-cliff-0a1b2c3d4.azurestaticapps.net`)

**Via Azure CLI:**
```bash
az staticwebapp show \
  --name swa-rapha-movies \
  --resource-group rg-rapha-movies \
  --query defaultHostname \
  --output tsv
```

---

## 7. Configurar Domínio Personalizado (Opcional)

### 7.1. Adicionar Domínio Personalizado

1. No Portal Azure → Static Web App → **"Domínios personalizados"**
2. Clique em **"+ Adicionar"**
3. Escolha **"Domínio personalizado no DNS externo"**
4. Digite seu domínio (ex: `raphamovies.com.br`)
5. Copie o registro CNAME fornecido

### 7.2. Configurar DNS

No painel do seu provedor de domínio, adicione:

| Tipo | Nome | Valor |
|------|------|-------|
| CNAME | www | `blue-cliff-0a1b2c3d4.azurestaticapps.net` |
| CNAME | @ | `blue-cliff-0a1b2c3d4.azurestaticapps.net` |

### 7.3. Validar e Ativar SSL

1. Volte ao Portal Azure
2. Clique em **"Validar"**
3. Após validação, o SSL é configurado automaticamente

---

## 8. Gerenciar Imagens no Blob Storage

### 8.1. Upload via Azure CLI

```bash
# Upload de um arquivo
az storage blob upload \
  --account-name straphamovies \
  --container-name movies \
  --name posters/interestelar.jpg \
  --file ./images/interestelar-poster.jpg \
  --content-type image/jpeg

# Upload de múltiplos arquivos
az storage blob upload-batch \
  --account-name straphamovies \
  --destination movies/posters \
  --source ./images/posters \
  --content-type image/jpeg
```

### 8.2. Upload via Portal Azure

1. Storage Account → Contêineres → `movies`
2. Clique em **"Carregar"**
3. Arraste ou selecione as imagens
4. Em **"Avançado"**, defina o caminho:
   - Para posters: `posters/nome-do-filme.jpg`
   - Para backdrops: `backdrops/nome-do-filme.jpg`
5. Clique em **"Carregar"**

### 8.3. Upload via Azure Storage Explorer

1. Baixe: [Azure Storage Explorer](https://azure.microsoft.com/features/storage-explorer/)
2. Instale e faça login com sua conta Azure
3. Navegue: Storage Accounts → `straphamovies` → Blob Containers → `movies`
4. Clique em **"Upload"** → **"Upload Files"**
5. Selecione as imagens e configure o destino

### 8.4. Obter URLs das Imagens

**Formato da URL:**
```
https://straphamovies.blob.core.windows.net/movies/posters/interestelar.jpg
https://straphamovies.blob.core.windows.net/movies/backdrops/interestelar.jpg
```

**Via CLI:**
```bash
# Listar blobs
az storage blob list \
  --account-name straphamovies \
  --container-name movies \
  --query "[].name" \
  --output table

# Obter URL de um blob
az storage blob url \
  --account-name straphamovies \
  --container-name movies \
  --name posters/interestelar.jpg \
  --output tsv
```

### 8.5. Atualizar URLs no Código

Edite o arquivo `src/data/movies.ts` com as URLs do Azure:

```typescript
export const movies: Movie[] = [
  {
    id: "1",
    title: "Interestelar",
    synopsis: "Uma equipe de exploradores...",
    year: 2014,
    duration: "2h 49min",
    genre: "Ficção Científica",
    rating: 8.7,
    imageUrl: "https://straphamovies.blob.core.windows.net/movies/posters/interestelar.jpg",
    backdropUrl: "https://straphamovies.blob.core.windows.net/movies/backdrops/interestelar.jpg"
  },
  // ... outros filmes
];
```

Após atualizar, faça commit e push:

```bash
git add .
git commit -m "feat: atualizar URLs das imagens para Azure Blob Storage"
git push origin main
```

O GitHub Actions irá automaticamente reimplantar o site.

---

## 9. Custos Estimados

### Conta Gratuita Azure (Primeiros 12 meses)

| Serviço | Limite Gratuito | Custo Excedente |
|---------|-----------------|-----------------|
| Static Web Apps | 100GB banda/mês | Plano Free ilimitado |
| Blob Storage | 5GB armazenamento | ~R$0,10/GB |
| Transferência de dados | 5GB/mês | ~R$0,40/GB |

### Estimativa Mensal (Projeto Educacional)

| Item | Uso Estimado | Custo |
|------|--------------|-------|
| Static Web Apps | < 100GB | **Gratuito** |
| Blob Storage | ~500MB | **~R$0,05** |
| Transferência | ~2GB | **Gratuito** |
| **Total Mensal** | - | **< R$1,00** |

---

## 10. Solução de Problemas

### Erro: "Blob not found" (404)

**Causa:** URL incorreta ou blob não existe.

**Solução:**
1. Verifique se o nome do arquivo está correto (case-sensitive)
2. Confirme que o container tem acesso público
3. Teste a URL diretamente no navegador

### Erro: CORS bloqueando imagens

**Causa:** CORS não configurado.

**Solução:**
```bash
az storage cors clear --services b --account-name straphamovies
az storage cors add \
  --services b \
  --methods GET HEAD OPTIONS \
  --origins "*" \
  --allowed-headers "*" \
  --account-name straphamovies
```

### Build falha no GitHub Actions

**Causa:** Dependências ou configuração incorreta.

**Solução:**
1. Vá para GitHub → Actions → Clique no workflow com falha
2. Verifique os logs de erro
3. Problemas comuns:
   - Falta de `npm install`
   - Variável de ambiente não configurada
   - Erro de sintaxe no código

### Site não atualiza após push

**Causa:** Cache ou workflow não executou.

**Solução:**
1. Verifique se o workflow foi executado no GitHub Actions
2. Limpe o cache do navegador (Ctrl+Shift+R)
3. Aguarde propagação do CDN (até 5 minutos)

### Imagens não carregam no site

**Causa:** URLs incorretas ou container privado.

**Solução:**
1. Teste a URL da imagem diretamente no navegador
2. Verifique se o container está com acesso "Blob"
3. Confirme que não há typos na URL

---

## 📚 Referências

- [Documentação Azure Static Web Apps](https://docs.microsoft.com/azure/static-web-apps/)
- [Documentação Azure Blob Storage](https://docs.microsoft.com/azure/storage/blobs/)
- [Azure CLI Reference](https://docs.microsoft.com/cli/azure/)
- [Vite Build Configuration](https://vitejs.dev/guide/build.html)
- [React Router](https://reactrouter.com/)

---

## ✅ Checklist Final

- [ ] Código exportado do Lovable para GitHub
- [ ] Conta Azure criada e configurada
- [ ] Resource Group `rg-rapha-movies` criado
- [ ] Storage Account `straphamovies` criada
- [ ] Container `movies` com acesso público
- [ ] CORS configurado no Blob Storage
- [ ] Imagens uploaded para o Blob Storage
- [ ] Static Web App criada e conectada ao GitHub
- [ ] Workflow do GitHub Actions funcionando
- [ ] URLs das imagens atualizadas no código
- [ ] Site acessível via URL do Azure

---

**Desenvolvido para fins educacionais**  
Rapha Movies © 2024
