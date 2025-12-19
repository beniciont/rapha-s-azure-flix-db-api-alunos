# Rapha Movies API - Backend .NET Core

## 📋 Pré-requisitos

Antes de começar, você precisa instalar:

1. **.NET 8 SDK** - [Download aqui](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **Visual Studio 2022** ou **VS Code** com extensão C#
3. **SQL Server** (LocalDB para desenvolvimento ou Azure SQL para produção)

## 🚀 Como Executar Localmente

### 1. Abra o Terminal na pasta do projeto

```bash
cd backend/RaphaMovies.API
```

### 2. Restaure as dependências

```bash
dotnet restore
```

### 3. Configure o banco de dados

O projeto usa LocalDB por padrão em desenvolvimento. Crie a migração inicial:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Execute a aplicação

```bash
dotnet run
```

A API estará disponível em:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger**: https://localhost:5001/swagger

## 🔐 Credenciais Padrão

Após a primeira execução, um usuário admin é criado automaticamente:

- **Email**: admin@raphamovies.com
- **Senha**: Admin@123

## 📁 Estrutura do Projeto

```
RaphaMovies.API/
├── Controllers/          # Endpoints da API
│   ├── AuthController.cs
│   ├── MoviesController.cs
│   ├── RentalsController.cs
│   └── AdminController.cs
├── Data/                 # Contexto do banco
│   └── ApplicationDbContext.cs
├── DTOs/                 # Objetos de transferência
│   ├── AuthDTOs.cs
│   ├── MovieDTOs.cs
│   ├── RentalDTOs.cs
│   └── AdminDTOs.cs
├── Models/               # Entidades do banco
│   ├── User.cs
│   ├── UserRole.cs
│   ├── Movie.cs
│   └── Rental.cs
├── Services/             # Lógica de negócio
│   ├── AuthService.cs
│   ├── MovieService.cs
│   ├── RentalService.cs
│   └── AdminService.cs
├── appsettings.json      # Configurações
└── Program.cs            # Ponto de entrada
```

## ⚙️ Configuração

### appsettings.json

Edite o arquivo `appsettings.json` para configurar:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "SUA_CONNECTION_STRING"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_JWT_SUPER_SECRETA",
    "Issuer": "RaphaMovies",
    "Audience": "RaphaMoviesApp"
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:5173",
      "https://seu-frontend.com"
    ]
  }
}
```

## 🌐 Endpoints da API

### Autenticação
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | /api/auth/login | Login |
| POST | /api/auth/register | Registro |
| POST | /api/auth/logout | Logout |
| POST | /api/auth/refresh | Refresh token |
| GET | /api/auth/me | Usuário atual |

### Filmes
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | /api/movies | Listar filmes |
| GET | /api/movies/{id} | Buscar por ID |
| GET | /api/movies/genre/{genre} | Por gênero |
| GET | /api/movies/search | Pesquisar |
| GET | /api/movies/genres | Listar gêneros |
| POST | /api/movies | Criar (admin) |
| PUT | /api/movies/{id} | Atualizar (admin) |
| DELETE | /api/movies/{id} | Remover (admin) |

### Aluguéis
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | /api/rentals/my-rentals | Meus aluguéis |
| GET | /api/rentals/{id} | Buscar por ID |
| POST | /api/rentals | Criar aluguel |
| POST | /api/rentals/{id}/return | Devolver |

### Admin
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | /api/admin/stats | Estatísticas |
| GET | /api/admin/users | Listar usuários |
| PUT | /api/admin/users/{id} | Atualizar usuário |
| DELETE | /api/admin/users/{id} | Remover usuário |

## 🚀 Deploy no Azure

### 1. Criar recursos no Azure
- Azure SQL Database
- Azure App Service (Windows)

### 2. Configurar Connection String no Azure
No App Service > Configuration > Connection strings:
- Nome: `DefaultConnection`
- Valor: sua connection string do Azure SQL
- Tipo: `SQLAzure`

### 3. Publicar via Visual Studio
1. Clique com botão direito no projeto
2. Publish > Azure > Azure App Service
3. Selecione seu App Service
4. Publicar

### 4. Ou via CLI
```bash
dotnet publish -c Release
az webapp deploy --resource-group MeuGrupo --name MeuAppService --src-path ./publish
```

## 🔧 Solução de Problemas

### Erro de conexão com banco
- Verifique se o SQL Server está rodando
- Confirme a connection string no appsettings.json

### Erro de CORS
- Adicione a URL do frontend em `Cors:AllowedOrigins`

### Erro de autenticação
- Verifique se a chave JWT tem pelo menos 32 caracteres
- Confirme que o token está sendo enviado no header `Authorization: Bearer {token}`

## 📞 Suporte

Se tiver dúvidas, abra uma issue no repositório ou entre em contato.
