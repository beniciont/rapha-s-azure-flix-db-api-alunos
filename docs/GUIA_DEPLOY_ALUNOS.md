# 🎬 Rapha Movies - Guia de Deploy para Alunos

> **Tempo estimado:** 30-45 minutos  
> **Nível:** Intermediário 
---

## 📋 O que você vai precisar

✅ Uma conta Microsoft 

✅ Uma conta no GitHub (pode criar gratuitamente)  


---

## 🎯 Visão Geral

Você vai publicar um sistema de locadora de filmes na internet. O sistema tem 3 partes:

```
┌──────────────────────────────────────────────────────────────┐
│  🖥️  FRONTEND           📡 BACKEND (API)         🗄️BANCO    │
│  (Telas que você vê)    (Lógica do sistema)    (Armazena     │
│                                                  dados)      │
│                                                              │
│  React/.NET 8           .NET 8                 SQL Server    │
│  App Service            App Service            SQL Database  │
└──────────────────────────────────────────────────────────────┘
```

---

## 📚 Passo a Passo


### 🔷 ETAPA 1: Criar Grupo de Recursos (2 min)

> 💡 Um Grupo de Recursos é como uma "pasta" que organiza tudo do seu projeto.

1. No Portal Azure, clique em **"Grupos de recursos"** (menu esquerdo)
2. Clique em **"+ Criar"**
3. Preencha:
   - **Grupo de recursos:** `rg-app-prd-uks-001` (ex: rg-raphamovies-joao)
   - **Região:** `UK South`
4. Clique em **"Revisar + criar"**
5. Clique em **"Criar"**

✅ **Pronto!** Grupo criado.

---

### 🔷 ETAPA 2: Criar o Banco de Dados (10 min)

#### 2.1 Criar o Servidor SQL
1. Clique em **"+ Criar um recurso"**
2. Pesquise: **SQL Server**
3. Selecione **"SQL Server (servidor lógico)"**
4. Clique em **"Criar"**

#### 2.2 Configurar o Servidor
| Campo | O que colocar |
|-------|---------------|
| **Grupo de recursos** | Selecione o que você criou |
| **Nome do servidor** | `srv-raphamovies-db001` |
| **Região** | `UK South` |
| **Método de autenticação** | Usar autenticação SQL |
| **Login do administrador** | `adminsql` |
| **Senha** | Partiunuvem@2026 (anote!) |

5. Clique em **"Revisar + criar"** → **"Criar"**
6. Aguarde 2-3 minutos

#### 2.3 Criar o Banco de Dados
1. Quando terminar, clique em **"Ir para o recurso"**
2. No menu esquerdo, clique em **"Bancos de dados SQL"**
3. Clique em **"+ Criar banco de dados"**
4. Configure:
   - **Nome:** `sampledb`
   - **Computação:** Clique em "Configurar" → Selecione **"Basic"** → **"Aplicar"**
5. Clique em **"Revisar + criar"** → **"Criar"**

#### 2.4 Configurar Firewall
1. Volte para o servidor SQL (clique no nome dele)
2. Menu esquerdo: **"Rede"**
3. Marque ☑️ **"Permitir que serviços do Azure acessem este servidor"**
4. Clique em **"Salvar"**

#### 2.5 Configurar VM com SSMS
1. Criar uma VNET com estrutura de Subnet
2. Criar uma VM na mesma região
3. Instalar o SSMS
4. Baixa o Database já pronto em : https://stoposgraduacaotftec.blob.core.windows.net/arquivos-pos/RaphaMoviesDB.bacpac
5. Importar o DB no Azure SQL Database


### 🔷 ETAPA 3: Criar App Service do Backend (5 min)

#### 3.1 Criar App Service Plan:
- Clique em **"Criar novo App Service Plan"**
- Nome: `aplan-raphamovies001`
- Clique em **"Alterar tamanho"** → Selecione **"B1"** (Basic) → **"Aplicar"**

#### 3.2 Criar o App Service
1. Clique em **"+ Criar um recurso"**
2. Pesquise: **App Service**
3. Selecione **"Aplicativo Web"** → **"Criar"**

#### 3.2 Configurar
| Campo | Valor |
|-------|-------|
| **Grupo de recursos** | Selecione o seu |
| **Nome** | `raphamovies-api-001` |
| **Publicar** | Código |
| **Pilha de runtime** | `.NET 8 (LTS)` |
| **Sistema operacional** | Windows |
| **Região** | `UK South` |

4. Clique em **"Revisar + criar"** → **"Criar"**

#### 3.3 Configurar Connection String
1. Vá para o App Service criado
2. Menu esquerdo: **"Configuração"**
3. Aba **"Cadeias de conexão"**
4. Clique **"+ Nova cadeia de conexão"**:
   - **Nome:** `DefaultConnection`
   - **Valor:** Cole sua connection string (com a senha real!)
   - **Tipo:** `SQLAzure`
5. Clique **"OK"** → **"Salvar"** → **"Continuar"**

#### 3.4 Configurar Variáveis de Ambiente
1. Na aba **"Configurações de aplicativo"**
2. Adicione cada uma clicando em **"+ Nova configuração"**:

| Nome | Valor |
|------|-------|
| `Jwt__Secret` | `MinhaChaveSecretaMuitoSegura2024RaphaMovies!@#` |
| `Jwt__Issuer` | `RaphaMovies.Api` |
| `Jwt__Audience` | `RaphaMovies.Frontend` |
| `Jwt__ExpirationMinutes` | `60` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `CORS_ALLOW_ANY` | `true` |


> ⚠️ **Alternativa mais segura:** Em vez de `CORS_ALLOW_ANY`, você pode usar:
> - **Nome:** `CORS_ORIGINS`
> - **Valor:** `https://SEU-FRONTEND.azurewebsites.net` (ex: `https://raphamovies-frontend001.azurewebsites.net`)

3. Clique **"Salvar"** → **"Continuar"**
4. **Reinicie o App Service** (clique em "Reiniciar" no topo da página)

#### 3.6 Obter Publish Profile
1. Na página principal do App Service
2. Vá em Configuration e marque a opção SCM Basic Auth Publishing Credentials
3. Retorno no menmu Overview
4. Clique em **"Baixar perfil de publicação"** (Download publish profile)
5. Um arquivo `.PublishSettings` será baixado
6. **Abra o arquivo com Bloco de Notas** e copie TODO o conteúdo

✅ **Backend configurado!**

---

### 🔷 ETAPA 4: Criar App Service do Frontend (3 min)

#### 4.1 Criar o App Service
1. **"+ Criar um recurso"** → **"App Service"** → **"Aplicativo Web"** → **"Criar"**

#### 4.2 Configurar
| Campo | Valor |
|-------|-------|
| **Grupo de recursos** | Selecione o seu |
| **Nome** | `raphamovies-frontend-001` |
| **Publicar** | Código |
| **Pilha de runtime** | `Node 20 LTS` |
| **Sistema operacional** | Windows |
| **Região** | `UK South` |
| **Plano** | Selecione o que já criou (`plan-raphamovies-SEUNOME`) |

3. **"Revisar + criar"** → **"Criar"**

#### 4.3 Obter Publish Profile
1. Vá para o App Service do frontend
2. 2. Vá em Configuration e marque a opção SCM Basic Auth Publishing Credentials
3. Retorno no menmu Overview
3. Clique em **"Baixar perfil de publicação"**
4. Abra com Bloco de Notas e copie TODO o conteúdo

✅ **Frontend configurado!**

---

### 🔷 ETAPA 5: Fazer Fork do Repositório (2 min)

> 💡 Fork = Cópia do projeto para sua conta GitHub

1. Acesse: **(https://github.com/raphasi/rapha-s-azure-flix-db-api.git)** 
2. Clique no botão **"Fork"** (canto superior direito)
3. Selecione sua conta GitHub
4. Clique em **"Create fork"**
5. Aguarde... pronto! O projeto está na sua conta.

---

### 🔷 ETAPA 6: Configurar Secrets no GitHub (5 min)

> 💡 Secrets são senhas/configurações que o GitHub usa para publicar automaticamente.

#### 6.1 Acessar configurações
1. No seu repositório (fork), clique em **"Settings"** (aba superior)
2. Menu esquerdo: **"Secrets and variables"** → **"Actions"**

#### 6.2 Adicionar os Secrets
Clique em **"New repository secret"** para cada um:

| Nome do Secret | Valor |
|----------------|-------|
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Conteúdo do PublishSettings do **Frontend** |
| `AZURE_WEBAPP_NAME` | Nome do App Service do Frontend (ex: `raphamovies-frontend001`) |
| `AZURE_BACKEND_PUBLISH_PROFILE` | Conteúdo do PublishSettings do **Backend** |
| `AZURE_BACKEND_WEBAPP_NAME` | Nome do App Service do Backend (ex: `raphamovies-api-bend001`) |
| `VITE_API_URL` | `https://SEU-BACKEND-NAME.azurewebsites.net/api` |

> ⚠️ **ATENÇÃO IMPORTANTE:**
> - Use os nomes **exatos** dos App Services que você criou no Azure!
> - O `VITE_API_URL` **DEVE** terminar com `/api` (ex: `https://raphamovies-api-bend001.azurewebsites.net/api`)

✅ **Secrets configurados!**

---

### 🔷 ETAPA 7: Fazer o Deploy (3 min)

#### 7.1 Ativar o Deploy Automático
1. No seu repositório, clique na aba **"Actions"**
2. Se aparecer botão **"I understand my workflows, go ahead and enable them"**, clique nele
3. Você verá os workflows listados

#### 7.2 Executar o Deploy do Backend
1. Clique em **"Build and deploy ASP.Net Core app to Azure Web App"**
2. Clique em **"Run workflow"** (botão à direita)
3. Selecione `main` e clique em **"Run workflow"**
4. Aguarde 3-5 minutos (ficará verde ✅ quando terminar)

#### 7.3 Executar o Deploy do Frontend
1. Clique em **"Build and Deploy Frontend to Azure Web App"**
2. Clique em **"Run workflow"** → `main` → **"Run workflow"**
3. Aguarde 2-3 minutos

✅ **Deploy concluído!**

---

### 🔷 ETAPA 8: Testar sua Aplicação (2 min)

#### 8.1 Testar o Backend
1. Acesse: `https://raphamovies-api-SEUNOME.azurewebsites.net/swagger`
2. Você deve ver a documentação da API

#### 8.2 Testar o Frontend
1. Acesse: `https://raphamovies-frontend-SEUNOME.azurewebsites.net`
2. Você deve ver o site da locadora de filmes!

#### 8.3 Criar um Usuário Admin
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

## 📝 Checklist Final

- [ ] Grupo de Recursos criado
- [ ] Servidor SQL criado
- [ ] Banco de dados criado
- [ ] Tabelas criadas (4 tabelas)
- [ ] App Service Backend criado e configurado
- [ ] App Service Frontend criado
- [ ] Fork do repositório feito
- [ ] CORS configurado no Backend
- [ ] 5 Secrets configurados no GitHub
- [ ] Deploy do Backend executado ✅
- [ ] Deploy do Frontend executado ✅
- [ ] Site funcionando!



