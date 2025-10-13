# This file contains exercises based on the AppInfra demo

## 0. Notifications Module

> __!Objective__: Get familiar with the DemoApp, and see how the Bootstrapper, the Application Initialization, Dependency Injection and Service Locator work

#### 0.1 Explore the AppInfraDemo solution
 - Look at the `ConsoleUi` project. It is the host process
 - Loot at the `AppBoot` folder. It contains the Bootstrapper and the initialization logic
 - Look at the `Contracts` folder. It contains the contracts for the services exposed by the modules

#### 0.2 Notifications Serivces
  - Explore the `Notification.Services` project
	- `NotificationService` is used to send notifications
	- `StateChangeSubscriber` shows how a subscriber may be implemented
	- The `ConsoleUi` implements  the `IAmAliveSubscriber<IModule>` to write a console when a new module is alive

 - Understand how "NotificationsModule is alive!" appears on the console during the application boot.

--------------------
	
## 1. Sales Module

### 1.0 [Advanced] Add Sales Module and Sales.Services project to solution

> This exercise is optional. To do it you need to go back in history and checkuout `practice-1.0` tag

Do the following steps:
 - Create the `Modules\Sales` solution folder and the correspondent `.\Modules\Sales` folder in the source tree 
	- ! the naming conventions and folder structure matters !
 - Add the `Sales.Services` project to the solution in this create folder.
 - Understand how an application with plugins works in .NET (see https://learn.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support)
	-  add `<EnableDynamicLoading>true</EnableDynamicLoading>` to the `Sales.Services.csproj` file (see `Notifications.Services.csproj` for reference)
	- Understand how `AppBoot` loads plugins
		- look at `AppBoot.AssemblyLoad` namespace
		- look at `BreadcrumbNameConventionPathBuilder` and `PluginAssemblyLoader` classes to understand why the naming and folders conventions matter.	
- Setup `Sales.Services` as a solution dependency of `ConsoleUi` (this is needed so it builds when `ConsoleUi` is build for debug (on F5))
- Configure `Sales.Services` plugin in AppBoot
	- in `ConsoleUi/Program.cs` in AppBoot setup:
		- make sure the NameFilter allows for `Sales.*.` assemblies
		- add `.AddPlugin("Sales.Services")`

#### 1.1 Sales Services Module

Add a `SalesServicesModule : IModule` and notify when it is alive.

As a model for this, look on how `NotificationsModule` is implemented and how its *I'm Alive* notification is shown in the `ConsoleApplication`.

#### 1.2. Change the order in which the modules are initialized in a loosely coupled way

Use the `PriorityAttribute` and the `OrderByPriority<T>()` helper method, from `iQuarc.SystemEx` package

------------------

## 2. Console Application

>  __!Objective__: Understand AppBoot basics: Modules, Dependency Injection, and Service Locator 

### 2.1. Transform the OrdersConsoleApplication into an IModule
	
Make this class to implement the `IModule`. 
 - You should also consider to rename it, so its name reflects that it is a module
 - The `Program.Main()` should no longer directly depend on it. With this refactor, on the `IModule.Initialize()` the menu should be shown.

### 2.2. Unit Test the `Init()` function of the resulted Module

Which is the external dependencies?
When do we use stubs and when mocks?

--------------------

## 3. Create a Composite Console Application

>  __!Objective__: Understand how to benefit from the AppBoot type discovery and loosely coupled modules

### 3.1. Create a Console Ui Module that discovers commands and builds a menu from them

Follow the following steps as guidance:

1. Create the `IConsoleCommand` interface as below:

```
public interface IConsoleCommand
{
    void Execute();
    string MenuLabel { get; }
}
```

2. Create the `ConsoleUiModule` class that implements `IModule`. It would discover all the `IConsoleCommand` implementations, and will build a menu with them on its `Initialize()` function.


3. Transform the OrdersConsoleApplication, in a `IConsoleCommand` implementation


This should allow any other functional module to provide `IConsoleCommand` implementations. The `ConsoleUiModule` will discover them and will present them to the user for execution. It results a loosely coupling between the modules and their UI commands

### 3.2. Update the solution structure in such way that each module may have its own Console Commands and it does not depend on the host process or the UI app

Now, the `OrdersConsoleCommand` resulted from transforming the `OrdersConsoleApplication` into a `IConsoleCommand` sits in the `ConsoleApplication`, but it is quite intimate with the *Sales* module

We could move it to a new console project into the *Sales* module folder structure, and the new `CompositeConsoleUiModule` should discover it and use it.

The resulted `CompositeConsoleUiModule` should not take a dependency on `Sales.Services` (as we don't want to depend on implementation details, but it would depend on `Contracts` as it needs the `IConsoleCommand` interface)

*Hints:*
 1. the project should be a class library so it can be deployed on any .NET process
 2. the project should be loaded as a plugin in a similar way with the `Sales.Services`

The assemblies from any module (including *Sales*) should not depend on the `ConsoleApplication` assembly which is the host process and the UI. The dependency should be the other way around. We should invert it by moving the `IConsoleCommand` and the `IConsole` to the `Contracts` assembly  into a new `ConsoleUi` folder.

> !Observation:
By doing this we are decoupling the application from its UI. All the `ConsoleCommand` could be displayed and executed from another host or UI, may that be an WPF app or a Web App.

> !Observation:
Look at the references by generating the *Project Dependency Diagram*


### 3.3. Order the Menu entries

a) in a declarative manner
b) by module and then by more entries in the same module

-----------------------------


## 4. Customer Orders Service

> !Objective: Understand DataAccess implementation

All below should be called through a simple UI like the Console UI built in previous exercises

### 4.1. Create a new service in the Sales module that returns all the customers which have orders, ordered by store name

[Optional] - Write unit tests for it. Include tests that verify if the OrderBy is applied.

*Hint (linq query):*
```
Customers
	 .Where(c => c.SalesOrderHeaders.Any())
            .OrderBy(c => c.CompanyName)
            .Select(c => new CustomerData
            {
                Id = c.CustomerID,
                CompanyName = c.CompanyName,
                SalesPerson = c.SalesPerson
            });
```

### 4.2. [Optional] Add more filters and understand how you could unit test those laveraging the IRepository interface testability

- Show only the customers that have the name starting with a string which was read from the console.
- Show only the customers that have in the name a string which was read from the console
- Write Unit Tests for the above

### 4.3. Create a console command that sets the status of all orders of a customer

 - Create a new operation in the `OrderingService` that sets the status of all orders of a customer
 - We should read some relevant info about the customer that we will use to find the orders to which we will change the status
 - The new status should be read from the console OR hardcoded

*See the `SalesOrderHeaderStatusValues` class for the values of the `SalesOrderHeader.Status` property*

------------------

## 5. DataAccess.EntityInterceptors

> !Objective: Understand and use EntityInterceptors

Use `EntityInterceptors` and `INotificationService` to implement:

### 5.1. Implement a `IStateChangeSubscriber<SalesOrderHeader>` that shows on the console when a `SalesOrderHeader` is created, deleted or changed 

*Hint:* This should be part of the Sales Module

### 5.2 Implement a default `IStateChangeSubscriber<T>` that writes in a text file when any DTO is created, deleted or changed
 
*Hint:* This should be part of the Notifications Module

> !Observe and discuss the differences of who triggers the notification

-----------------

## 6. Add Entity Auditing

> !Objective: Understand the DataAccess benefit of low costs when adding new features

### 6.1. We want to consistently set the `ModifiedDate` for all entities that have this column

 - Now when we modify the order in exercise 4.3 or we add persons in exercise 8 the `ModifiedDate` is not set.
 - One way would be to go in all use cases where these entities are modified / created and set the `ModifiedDate` as well. This would be cumbersome and error prone
 - We should leverage the advantage of the encapsulated Data Access and extend the infrastructure, with an interceptor that does this for all entities.

 The interface which should be implemented by the Data Model entities could look like:

```
interface IAuditable
{
    DateTime ModifiedDate {get;set;}
}
```

------------------

## 7. Add Persons Management Module

> !Objective: Understand how to add new modules to this Application Infrastructure

### 7.1. Use Services from Persons Module in Sales Module

`SalesPerson` references through `BusinessEntityID` a `Person` from Person schema.

Create a new module named `Persons` which exposes a service that gives all the persons. This service should be used by `GetOrdersInfo()` to return the person name in the `SalesOrderInfo` result.

As an alternative, the service from the `Persons` module could be used by the UI and `GetOrdersInfo()` just returns the `BusinessEntityID` key

--------------------------

## 8 Add a new `Person`

> !Objective: See how existent Application Infrastructure capabilities are used by new features of new Modules

### 8.1 Create support to read a entity from the console

Use the following interfaces:

```
interface IEntityReader
{
    IEntityFieldsReader<T> BeginEntityRead<T>();
}

interface IEntityFieldsReader<T>
{
    IEnumerable<string> GetFields();
    void SetFieldValue(string value);
    T GetEntity();
}
```

Read the data needed to create a `Person` entity

### 8.1. Create a service that adds a new `Person` to the system

The above console command which reads the person info should use this service to add the new read person.

> !Observe: how the interceptors created in ex. 5 & 6 are working when a new Person is added

