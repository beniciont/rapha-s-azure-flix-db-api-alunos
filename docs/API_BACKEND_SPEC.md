# Especificação da API Backend - Rapha Movies

Este documento descreve a API REST que deve ser implementada em .NET Core para integração com o frontend React.

## Configuração

### Tecnologias Recomendadas
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2022
- JWT para autenticação
- AutoMapper para mapeamento de DTOs

### Estrutura de Projeto Sugerida
```
RaphaMovies.Api/
├── Controllers/
│   ├── AuthController.cs
│   ├── MoviesController.cs
│   ├── RentalsController.cs
│   └── AdminController.cs
├── Models/
│   ├── User.cs
│   ├── UserRole.cs
│   ├── Movie.cs
│   ├── Rental.cs
│   └── Genre.cs
├── DTOs/
│   ├── Auth/
│   ├── Movies/
│   └── Rentals/
├── Services/
├── Data/
│   └── ApplicationDbContext.cs
└── Program.cs
```

---

## Esquema do Banco de Dados (SQL Server)

### Tabela: Users
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

### Tabela: UserRoles
```sql
CREATE TABLE UserRoles (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('admin', 'user')),
    UNIQUE(UserId, Role)
);
```

### Tabela: Movies
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

### Tabela: Rentals
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

---

## Endpoints da API

### Autenticação (`/api/auth`)

#### POST /api/auth/login
Autentica um usuário e retorna tokens JWT.

**Request:**
```json
{
  "email": "usuario@email.com",
  "password": "senha123"
}
```

**Response (200):**
```json
{
  "user": {
    "id": "uuid",
    "email": "usuario@email.com",
    "name": "Nome do Usuário",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  },
  "token": "jwt_access_token",
  "refreshToken": "jwt_refresh_token",
  "expiresAt": "2024-01-01T01:00:00Z",
  "roles": ["user"]
}
```

#### POST /api/auth/register
Registra um novo usuário.

**Request:**
```json
{
  "name": "Nome do Usuário",
  "email": "usuario@email.com",
  "password": "senha123"
}
```

**Response (201):** Mesmo formato do login.

#### POST /api/auth/logout
Invalida o token atual.

**Headers:** `Authorization: Bearer {token}`

**Response (204):** No content.

#### POST /api/auth/refresh
Renova o access token usando o refresh token.

**Request:**
```json
{
  "refreshToken": "jwt_refresh_token"
}
```

**Response (200):** Mesmo formato do login.

#### GET /api/auth/me
Retorna dados do usuário autenticado.

**Headers:** `Authorization: Bearer {token}`

**Response (200):**
```json
{
  "id": "uuid",
  "email": "usuario@email.com",
  "name": "Nome do Usuário",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "roles": ["user"]
}
```

---

### Filmes (`/api/movies`)

#### GET /api/movies
Lista filmes com paginação e filtros.

**Query Parameters:**
- `page` (int, default: 1)
- `pageSize` (int, default: 20)
- `genre` (string, optional)
- `year` (int, optional)
- `minRating` (decimal, optional)
- `sortBy` (string: title|year|rating|createdAt)
- `sortOrder` (string: asc|desc)

**Response (200):**
```json
{
  "data": [
    {
      "id": "uuid",
      "title": "Título do Filme",
      "synopsis": "Sinopse...",
      "imageUrl": "https://...",
      "backdropUrl": "https://...",
      "trailerUrl": "https://...",
      "year": 2024,
      "duration": "2h 30min",
      "rating": 8.5,
      "genre": "Ação",
      "rentalPrice": 9.90,
      "isAvailable": true,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalItems": 100,
  "totalPages": 5,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

#### GET /api/movies/{id}
Retorna um filme específico.

#### GET /api/movies/search?query={query}
Busca filmes por título ou sinopse.

#### GET /api/movies/genre/{genre}
Lista filmes de um gênero específico.

#### GET /api/movies/genres
Lista todos os gêneros disponíveis.

---

### Aluguéis (`/api/rentals`)
**Requer autenticação**

#### GET /api/rentals/my-rentals
Lista aluguéis do usuário autenticado.

**Response (200):**
```json
[
  {
    "id": "uuid",
    "userId": "uuid",
    "movieId": "uuid",
    "rentedAt": "2024-01-01T00:00:00Z",
    "dueDate": "2024-01-08T00:00:00Z",
    "returnedAt": null,
    "status": "active",
    "totalPrice": 9.90,
    "movie": { ... }
  }
]
```

#### POST /api/rentals
Cria um novo aluguel.

**Request:**
```json
{
  "movieId": "uuid",
  "rentalDays": 7
}
```

#### POST /api/rentals/{id}/return
Registra a devolução de um aluguel.

---

### Admin (`/api/admin`)
**Requer autenticação + role "admin"**

#### GET /api/admin/stats
Retorna estatísticas do sistema.

**Response (200):**
```json
{
  "totalUsers": 150,
  "totalMovies": 500,
  "activeRentals": 45,
  "overdueRentals": 3,
  "totalRevenue": 15000.00,
  "monthlyRevenue": 2500.00
}
```

#### GET /api/admin/users
Lista todos os usuários (paginado).

#### DELETE /api/admin/users/{id}
Remove um usuário.

#### PATCH /api/admin/users/{id}/role
Atualiza a role de um usuário.

#### GET /api/admin/rentals
Lista todos os aluguéis (paginado).

#### POST /api/admin/movies
Cria um novo filme.

#### PUT /api/movies/{id}
Atualiza um filme.

#### DELETE /api/movies/{id}
Remove um filme.

---

## Configuração de CORS

Configure o CORS para permitir requisições do frontend:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",     // Dev local
            "https://seu-frontend.com"   // Produção
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
```

---

## Tratamento de Erros

Todas as respostas de erro devem seguir este formato:

```json
{
  "message": "Descrição do erro",
  "code": "ERROR_CODE",
  "details": {
    "campo": ["Mensagem de validação"]
  }
}
```

### Códigos HTTP
- `200` - Sucesso
- `201` - Criado
- `204` - Sem conteúdo
- `400` - Requisição inválida
- `401` - Não autenticado
- `403` - Não autorizado
- `404` - Não encontrado
- `409` - Conflito (ex: email já existe)
- `500` - Erro interno

---

## Variáveis de Ambiente

Configure no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=seu-servidor;Database=RaphaMovies;User Id=sa;Password=sua-senha;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Secret": "sua-chave-secreta-muito-longa-e-segura",
    "Issuer": "RaphaMovies.Api",
    "Audience": "RaphaMovies.Frontend",
    "ExpirationMinutes": 60,
    "RefreshExpirationDays": 7
  }
}
```
