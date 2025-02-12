# webapiwc
 
## Prerequisites

- .NET SDK  
- PostgreSQL (if using Postgres)  
- `dotnet-ef` tool (`dotnet tool install --global dotnet-ef`)  

## 1. Install EF Core and PostgreSQL provider

```sh
dotnet add package Microsoft.EntityFrameworkCore.Design  
dotnet add package Microsoft.EntityFrameworkCore.Tools  
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL  
```

## 2. Create a new migration

```sh
dotnet ef migrations add InitialCreate
```

## 3. Update the database

```sh
dotnet ef database update
```


## 4. Run the application

```sh
dotnet run
```

## 5. Open the Swagger UI
```sh
https://localhost:<PORT>/swagger
```