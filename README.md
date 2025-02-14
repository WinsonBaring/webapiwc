# webapiwc
 
## Prerequisites

- install packages
```sh
dotnet restore
```
- run docker first and create a postgres database
```sh
docker run -d --name pg -e POSTGRES_PASSWORD=pg -p 5249:5432 volumes:/var/lib/postgresql/data
```

- add this alias script to your .zshrc file and run it in cli
```sh
alias ef_reset="rm -rf Migrations && dotnet ef database drop -f && dotnet ef migrations add InitialCreate && dotnet ef database update && dotnet clean && dotnet build && dotnet run"
```

- check in swagger ui
```sh
https://localhost:5249/swagger
```


