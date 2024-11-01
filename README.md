## Add a data migration

With Package Manager Console, you can add a data migration.

Choose default project as ```src\Infrastructure```

Add migration command ```dotnet ef migrations add {MigrationName} --startup-project ..\API --verbose```

Update database command ```dotnet ef database update -p .\ --startup-project ..\API --verbose```

Database in development mode will be deleted then recreated automatically.
