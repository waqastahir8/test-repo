# Description

This folder contains the models and migration scripts necessary to create and update the database schema for the Users API.

    NOTES: 
        1.  When running manually, all commands are being should be run from this directory - AmeriCorps.Users.Data
        2.  The DB connection string is stored in AzureKeyvault

## Dependencies

    1. Entity Framework
        - Check:  dotnet ef --version
    2. Ensure environment variable KEY_VAULT_NAME is set to AmeriCorpsKeyVault
        - export KEY_VAULT_NAME=AmeriCorpsKeyVault
        - This is the keyvault in Azure that contains the db connection string for the Users API DB

## Code-first DB Migrations

A code-first approach is being used to update the database schema for the Users API.  

The **Model** directory, contains the models used to create the users database schema.  

### Testing Migrations Locally

    1. Run postgresdb in a docker container
    2. Add an appsettings.local.json
    3. Add a connections string "UsersDBConnStr" with the connection stringof your local db.

    "ConnectionStrings": {       
    "UsersDBConnStr": "Server=localhost;Port=5432;Database=<db-name>;;Username=<user>;Password=<password>"        
    }, 

### To View list of migrations

dotnet ef migrations list

### To Update the DB Schema
    1.  Update the Model as necessary
        - Ex. Adding References list to User.cs
    2.  Create migration
        - Ex. dotnet ef migrations add AddingReferencesMigration
        NOTE: remove with:  dotnet ef migrations remove
    3.  Check that migration is listed:
        - dotnet ef migrations list
        NOTE:  (Pending) - means that migration is there but has NOT been applied to database.
    3.  Apply migration to database
        - Ex. dotnet ef database update AddingReferencesMigration
    4.  Verify database has been updated
        - NOTE: listing migrations should now NOT list any migrations as (Pending)

### Create / Use Idempotent Script

If you are not sure what migrations have been applied or need to be applied, after creating the migration above, you can create an **idempotent** SQL script.  This generates an SQL script that internally checks which migrations have been applied and which migrations need to be applied.

    1.  Create idempotent script
        - dotnet ef migrations script --idempotent > users-idempotent.sql
    2.  This script **users-idempotent.sql** can now be run in the database (or using CI) and it will apply all migrations that need to be applied.
    


