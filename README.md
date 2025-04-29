# Docker

```
docker compose up
```

# Migration

```
dotnet ef migrations add --verbose -p ./ExpenseTracker.Persistence --msbuildprojectextensionspath ./ExpenseTracker.Persistence/obj --context ExpenseTrackerDbContext -s ExpenseTracker.Api/ Initial
dotnet ef database update --verbose -p ./ExpenseTracker.Persistence --context ExpenseTrackerDbContext -s ExpenseTracker.Api/
```