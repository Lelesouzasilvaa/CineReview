# CineReview API

API REST desenvolvida em **.NET 8** para gerenciamento de filmes, séries, usuários e avaliações.

---

# Tecnologias Utilizadas

## .NET 8  
## Entity Framework Core 8  
## SQLite  
## Swagger / Swashbuckle  
## JWT Authentication (opcional)  
## Migrations via EF Core  

---

# Estrutura do Projeto

```
CineReview.Api/
│── Controllers/
│   ├── FilmesController.cs
│   ├── SeriesController.cs
│   ├── UsuariosController.cs
│   └── AvaliacoesController.cs
│
│── Data/
│   ├── CineReviewContext.cs
│   └── SeedData.cs
│
│── Models/
│   ├── Filme.cs
│   ├── Serie.cs
│   ├── Usuario.cs
│   └── Avaliacao.cs
│
│── Migrations/
│
│── appsettings.json
│── Program.cs
│── CineReview.Api.csproj
```

---

# Como Executar o Projeto

## 1. Restaurar dependências

```bash
dotnet restore
```

## 2. Aplicar as migrações

```bash
dotnet ef database update
```

## 3. Rodar o servidor

```bash
dotnet run
```

Servidor disponível em:

```
http://localhost:5232
```

---

# Documentação (Swagger)

Acesse para testar os endpoints:

```
http://localhost:5232/swagger
```

---

# Banco de Dados

O projeto utiliza **SQLite**, com arquivo gerado automaticamente:

```
CineReviewDB.db
```

## Entidades Mapeadas

### Filme  
### Serie  
### Usuario  
### Avaliacao  

Com relacionamentos configurados via Entity Framework Core.

---

# Endpoints Principais

## Filmes
| Método | Rota           | Descrição               |
|-------|----------------|-------------------------|
| GET   | /api/filmes    | Lista todos os filmes   |
| POST  | /api/filmes    | Cria um novo filme      |

## Séries
| Método | Rota           | Descrição                |
|-------|----------------|--------------------------|
| GET   | /api/series    | Lista todas as séries    |
| POST  | /api/series    | Cria uma nova série      |

## Usuários
| Método | Rota            | Descrição          |
|--------|-----------------|--------------------|
| POST   | /api/usuarios   | Cadastra usuário   |

## Avaliações
| Método | Rota               | Descrição            |
|-------|---------------------|----------------------|
| POST  | /api/avaliacoes     | Envia avaliação      |

---

# Configuração via *appsettings.json*

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=CineReviewDB.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
