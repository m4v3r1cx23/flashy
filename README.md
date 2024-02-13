# ASP .NET Core - Flashy
ASP .NET Core Project which uses selected features:
- EntityFramework Core with Migrations
- Identity including Authentication and Authorization
- Logging
- Swagger
- etc.

## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes

### Prerequisites
- Visual Studio Code or Visual Studio 2022
- .NET 8.x SDK

### Installing
1. Clone this repository using ```git clone https://github.com/m4v3r1cx23/flashy.git [destinationPath]```
2. Navigate to newly created directory (if destinationPath wansn't provided by default folder will have repository's name - in this case "flashy") and run ```dotnet restore``` command to download all required packages

### Executing
1. Using Visual Studio Code or Visual Studio 2022 open Flashy project
2. To run project locally either use **Run -> Start Debugging** from the top menu or use console and write ```dotnet run```.
3. While using IDE it should automatically open new webbrowser window with address of: ***https://localhost:7018*** - while runing from console you need to launch said website by yourself.

### Additional Settings
1. By defautl Azure Database is used to develop locally, to change this you can replace ```GetConnectionString("FlashyConnection")``` in **Program.cs** to different one. Check **appsettings.json** for available connection strings - you can provide additional entries there and use them without any problems.
2. Additional Database settings are avialble in **Program.cs** inside ```AddDbContext<ApplicationDbContext>(options =>``` anonymous method
3. Identity settings (Authorization and Authentication) are aviable in **Program.cs** inside ```AddIdentity<User, Role>(options =>``` anonymous method
4. There are additional settings in **Program.cs** that are related to Swagger, Logging, Razor Pages and Exception Handling.

## Folder Structure
1. **Data** folder contains Migrations, Entity -> DB Model Configurations, Database Models and Database Context used to retrieve data from database as C# objects
2. **Shared** folder contains Models and Services used by both backend logic and database
3. **Pages** folder is the base root of the UI part of application, there is main **_Layout** of **About**, **Privary**, **Index** and **Error** page (where **Index** is default one)
4. Additional pages are boundled together in a moduels called **Areas** - which by convention have **AreaName -> Pages -> FeatureName -> PageName** structure (for example to see page used to edit FlashCards we need to navigate to: **Areas/Admin/Pages/FlashCards/Edit** file)
5. Due to how Razor Pages works each page is split into two files **x.cshtml** consisitng of HTML pseudo-code and **x.cshtml.cs** consisting of page logic - by convention everything is matched together by name and path (so even if two files are called the same, but in different folders there's no name clash between them)

## Migrations
1. Migrations require to have **dotnet-ef** tool installed globally - you can do this in terminal by running: ```dotnet tool install dotnet-ef -g``` command
2. You can manage migrations and database from console - simply navigate to root directory of the project you want to manage and run: ```dotnet ef -h``` to explore your options.
3. Some commands might require manually choosing DB Context - in that case you should use ```-c ApplicationDbContext```

## InProgress
There's still a lots of work to be done around this application, so everyone should consider this as unstable for the time being. UI will be rewriten to Angular due to issues with ASP.NET Core Razor Pages - backend will stay as ASP.NET Core and will introduce proper endpoints to be used by frontend