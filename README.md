# EventAPI
Repository for the purpose of Adam Tilley's technical test


## Database Setup
To setup the database follow the following instructions:
1. In command prompt, navigate to ./IventisEventApi
2. Verify the migration file exists in ./IventisEventApi/Migrations
3. If it does not, run `dotnet ef migrations add InitialCreate`
4. Run `dotnet ef database update`

This should setup the SQLite database with some inital entries in each table.
