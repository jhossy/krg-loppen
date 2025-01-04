
# Introduction

# Database
Ref. https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

1) Change to ./src

2) Initial create: 
 
`dotnet ef migrations add InitialCreate --output-dir Migrations --project Krg.Database --startup-project Krg.Website`

3) Apply migrations locally:

`dotnet ef database update --project Krg.Database --startup-project Krg.Website`

4) Add new migration:
 
`dotnet ef migrations add AddBlogCreatedTimestamp`

# Identity

1) Change to ./src

2) dotnet ef migrations add InitialIdentityCreate --output-dir IdentityMigrations --project Krg.Database --startup-project Krg.Website --context ApplicationDbContext

3dotnet ef database update --project Krg.Database --startup-project Krg.Website --context ApplicationDbContext