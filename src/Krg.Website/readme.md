
# Introduction

# Database
Ref. https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

1) Change to ./src

2) Initial create: 
 
`dotnet ef migrations add InitialCreate --output-dir Migrations/Krg --project Krg.Database --startup-project Krg.Website`

3) Apply migrations locally:

`dotnet ef database update --project Krg.Database --startup-project Krg.Website --context ApplicationDbContext`

4) Add new migration:
 
`dotnet ef migrations add AddBlogCreatedTimestamp`

5) Migration script:

`dotnet ef migrations script InitialCreate AddEventNote --project Krg.Database --context KrgContext --startup-project Krg.Website -o ./Krg.Database/Migrations/Scripts/AddEventNote.sql`

# Identity

1) Change to ./src

2) Create the migration:
`dotnet ef migrations add InitialIdentityCreate --output-dir Migrations/Identity --project Krg.Database --startup-project Krg.Website --context IdentityContext`

3) Update the database:
`dotnet ef database update --project Krg.Database --startup-project Krg.Website --context IdentityContext`