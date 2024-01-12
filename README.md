# Angular + WebApi + EF + MySQL Template

View this document in Visual Studio Code using Preview (CTRL+K, V), or using some other markdown viewer.

## Prerequisites

* Visual Studio 2019 
* .NET Core 3.1 SDK (v3.1.404) (https://dotnet.microsoft.com/download/dotnet-core/3.1)
* NodeJS 14.15.1 (https://nodejs.org/en/)
* Angular 11 CLI
* MySQL Community Server 8+ (https://dev.mysql.com/downloads/mysql/)
* Git for Windows (https://git-scm.com/downloads)

# Setup

First, make sure NodeJS is installed. It may ask if you want to install extra tools for compiling, you may choose to skip this option, otherwise it may take a while to install the tools.

Clone this repository

## Install Angular CLI

You only need to do this if you haven't before. This will install Angular CLI globally.

```
> npm install -g @angular/cli
```

## Install node modules

From the command line, cd to the path `IBSConnect.AngularApp\ClientApp`

```
> npm install
```

This might take a while, so go make some coffee.

 Visual Studio will also perform this step if you build the project.

## Database

Update the connection string in the following locations to point to your server and use the username and password you have setup for MySQL.

* `MapTest\appsettings.json`
* `IBSConnect.AngularApp\appsettings.json`
* `IBSConnect.Database\appsettings.json`

```json
 ...
  "ConnectionStrings": {
    "Database": "server=localhost;database=database;user=root;password=password"
  }
 ...
```

Build and run the `IBSConnect.Database` application. It will create a new schema.

Build and run the `MapTest` application. It will create test data.

Start the Web application.

# IIS Setup and Deployment

https://dotnet.microsoft.com/download/dotnet-core/3.1

# Development - Client Side

TypeScript is a superset of JavaScript. It allows up to give types to variables, for better intellisense and compile-time checking.

## Angular Modules

Modules are used to group components together. There is currently only one module in the application, `AppModule` in `app.module.ts`.

## Angular Components

Angular components use templates to generate and display HTML. They act as pages and controls, and can be used interchangeably. They need to be declared in a module before other components can use them. The following command will create a component and add it to `AppModule`.

``` 
> ng g c <componentname> -m app
```

## Angular Services

Services are classes that can be injected, but do not belong to any module. They need not be imported into a module, and can be injected directly into a component. They contain only code and do not generate HTML.

To create a service, use the following command. `-o` specifies the folder where you want to place the service, which will be named `<servicename>.service.ts`, with a class name `<ServiceName>Service`.

```
> ng g s <servicename> -o app/services
```

## Routing

# Development - Server Side/C# 

## MapTest

`MapTest` is a tool for testing the Business and Data layers without going through the UI. It sets up all the dependency injection needed in `Startup.cs`. Any services added to `PHMS.AngularApp.Startup.ConfigureServices` should be added here as well.

```cs
    // Get an instance of IServiceProvider
     services = Startup.GetServices();
    // Get an instance of IUserBL
    var userBL = services.GetService<IUserBL>();
    // Call a method on the instance
    await userBL.GetUserByID(...);
```
