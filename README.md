# EventAPI
Repository for the purpose of Adam Tilley's technical test

## Demo
A video demo of the api working on postman can be found here:
https://youtu.be/lyXX1yGiz30


## Pre-requisites

- .net8

### Project Pre-requisite Packages
- Microsoft.EntityFrameworkCore (8.0.6)
- Microsoft.EntityFrameworkCore.Design (8.0.6)
- Microsoft.EntityFrameworkCore.Sqlite (8.0.6)
- Microsoft.EntityFrameworkCore.Tools (8.0.6)
- Swashbuckle.AspNetCore (6.4.0) (Only used for the default web api that is added)

### Testing Pre-requisite Packages
- coverlet.collector (6.0.0)
- Microsoft.Data.SqlCLient (5.1.4)
- Microsoft.EntityFrameworkCore (8.0.6)
- Microsoft.EntityFrameworkCore.Design (8.0.6)
- Microsoft.EntityFrameworkCore.InMemory (8.0.6)
- Microsoft.EntityFrameworkCore.Sqlite (8.0.6)
- Microsoft.EntityFrameworkCore.Tools (8.0.6)
- Microsoft.NET.Test.Sdk (17.8.0)
- xunit (2.5.3)
- xunit.runner.visualstudio (2.5.3)

All packages should be able to be installed by navigating to each project folder and running `dotnet restore`

## Database Setup
To setup the database follow the following instructions:
1. In command prompt, navigate to ./IventisEventApi
2. Verify the migration file exists in ./IventisEventApi/Migrations
3. If it does not, run `dotnet ef migrations add InitialCreate`
4. Run `dotnet ef database update`

This should setup the SQLite database with some inital entries in each table.

## Proposed Improvements
1. Generalise classes where possible to reduce instances of repeated code
2. Add more validation where needed
3. Create GeoBoundingBox and GeoLocation as separate entities to increase clarity in database (might be a 1NF violation currently)
4. Rework unit tests to be independant from database (it currently causes many problems if run concurrently - if you see any commented tests this is why)
