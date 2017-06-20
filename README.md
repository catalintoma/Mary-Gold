# Marigold

Source code for the Marigold reservation website

## Higlights

* [ASP Core](https://docs.microsoft.com/en-us/aspnet/core/)
* Unit of work \ repository thanks to [Arch](https://github.com/Arch/UnitOfWork/)
* Dto <-> Model mappings thanks to [AutoMapper](http://automapper.org/)
* API Documentation thanks to [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)
* Paging helpers thanks to [Hieudole](https://github.com/hieudole/PagedList.Core.Mvc)
* BLL layer for separating UI from Data layer

## Development (a lot easier with [VS Code](https://code.visualstudio.com/) and [this extension](https://github.com/OmniSharp/omnisharp-vscode))

*Tested with SQL Server express 2016*

*In order to use another database,change connection string from appsettings.json*

* Clone repository
* Restore dependencies
```
dotnet restore
```
* Run development build (will be running at [this endpoint](http://localhost:5000)
```
dotnet build
dotnet run
```

## API

You can view the API generated docs at [this endpoint](http://localhost:5000/swagger/)

The API provides an extenson point for

* viewing services
* adding a new service

## Diagrams

You can check general flow and schema diagram in the **Diagrams** folder

## TODO's

Follow [this page](https://github.com/catalintoma/Mary-Gold/labels/enhancement)
