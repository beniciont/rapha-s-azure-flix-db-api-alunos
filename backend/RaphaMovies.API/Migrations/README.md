# Migrações do Entity Framework

## Como Gerar Migrações

Depois de abrir o projeto no Visual Studio ou VS Code, execute os comandos abaixo no terminal:

### 1. Criar a primeira migração
```bash
dotnet ef migrations add InitialCreate
```

### 2. Aplicar a migração no banco de dados
```bash
dotnet ef database update
```

## Comandos Úteis

### Criar nova migração
```bash
dotnet ef migrations add NomeDaMigracao
```

### Reverter última migração
```bash
dotnet ef migrations remove
```

### Ver migrações pendentes
```bash
dotnet ef migrations list
```

### Gerar script SQL (para produção)
```bash
dotnet ef migrations script -o script.sql
```

## Observações

- As migrações são aplicadas automaticamente ao iniciar a aplicação (veja `Program.cs`)
- Para produção, recomenda-se usar scripts SQL gerados manualmente
- O banco de dados é criado automaticamente se não existir (LocalDB em desenvolvimento)
