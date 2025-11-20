# This file contains exercises based on the AppInfra demo

## 0. Notifications Module

> **Objective:** Get familiar with the DemoApp, and understand how the Bootstrapper, Application Initialization, Dependency Injection, and Service Locator work.

### 0.1 Explore the AppInfraDemo solution
- Look at the `ConsoleUi` project — it is the host process.  
- Look at the `AppBoot` folder — it contains the Bootstrapper and initialization logic.  
- Look at the `Contracts` folder — it contains the service contracts exposed by the modules.

### 0.2 Notifications Services
- Explore the `Notification.Services` project:
  - `NotificationService` is used to send notifications.  
  - `StateChangeSubscriber` shows how a subscriber may be implemented.  
  - The `ConsoleUi` implements `IAmAliveSubscriber<IModule>` to write a message to the console when a new module becomes active.
- Understand how *"NotificationsModule is alive!"* appears on the console during application startup.

---

## 1. Sales Module

### 1.0 [Optional][Advanced] Add Sales Module and Sales.Services project to the solution

> This exercise is optional. To complete it, you need to go back in history and check out the `practice-1.0` tag.

Steps:
- Create a `Modules\Sales` solution folder and the corresponding `.\Modules\Sales` folder in the source tree.  
  - ⚠️ The naming conventions and folder structure matter!  
- Add the `Sales.Services` project to the solution inside this new folder.  
- Understand how an application with plugins works in .NET (see [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support)):  
  - Add `<EnableDynamicLoading>true</EnableDynamicLoading>` to the `Sales.Services.csproj` file (see `Notifications.Services.csproj` for reference).  
  - Understand how `AppBoot` loads plugins:
    - Check the `AppBoot.AssemblyLoad` namespace.  
    - Review the `BreadcrumbNameConventionPathBuilder` and `PluginAssemblyLoader` classes to see why naming and folder conventions are important.  
- Set up `Sales.Services` as a solution dependency of `ConsoleUi` (so it builds automatically when `ConsoleUi` is built for debug / F5).  
- Configure the `Sales.Services` plugin in `AppBoot`:  
  - In `ConsoleUi/Program.cs`, inside the AppBoot setup:
    - Ensure the `NameFilter` allows `Sales.*` assemblies.  
    - Add `.AddPlugin("Sales.Services")`.

### 1.1 Sales Services Module

Add a `SalesServicesModule : IModule` and make it send a notification when it becomes active.

Use `NotificationsModule` as a model and see how its *“I’m alive”* notification is displayed in the console application.

### 1.2 Change the module initialization order in a loosely coupled way

Use the `PriorityAttribute` and the `OrderByPriority<T>()` helper method from `AppBoot.SystemEx.Priority`.


------------------

## 2. Console Application

> **Objective:** Understand AppBoot basics — Modules, Dependency Injection, and Service Locator.

### 2.1 Transform the OrdersConsoleApplication into an IModule

- Make the `OrdersConsoleApplication` class implement the `IModule` interface.  
- Consider renaming it so the name clearly indicates it represents a module.  
- The `Program.Main()` method should no longer directly depend on this class.  
- After refactoring, the menu should be shown from the `IModule.Initialize()` method instead of `Main()`.

### 2.2 [Optional] Unit test the `Init()` function of the resulting module

- Identify the external dependencies of the module.  
- Discuss when to use *stubs* and when to use *mocks*.


--------------------

## 3. Create a Composite Console Application

> **Objective:** Understand how to benefit from AppBoot’s type discovery and loosely coupled modules.

### 3.1 Create a Console UI Module that discovers commands and builds a menu from them

Follow the steps below as guidance:

Step a) Create the `IConsoleCommand` interface as shown:

   ```csharp
   public interface IConsoleCommand
   {
       void Execute();
       string MenuLabel { get; }
   }
   ```

Step b) Create the `ConsoleUiModule` class that implements `IModule`.  
   It should discover all `IConsoleCommand` implementations and build a menu from them inside its `Initialize()` method.

Step c) Transform the `OrdersConsoleModule` into an `IConsoleCommand` implementation.

This setup will allow any other functional module to provide its own `IConsoleCommand` implementations.  
The `ConsoleUiModule` will automatically discover them and present them to the user for execution, resulting in a loosely coupled relationship between the modules and their UI commands.

---

### 3.2 Update the solution structure so that each module can have its own console commands, independent of the host process or UI app

Currently, the `OrdersConsoleCommand`—created from transforming the `OrdersConsoleModule` into an `IConsoleCommand`—resides in the `ConsoleApplication` project, even though it’s closely related to the *Sales* module.

We can move it to a new project inside the *Sales* module folder structure.  
The `CompositeConsoleUiModule` should then discover and use it.

The resulting `CompositeConsoleUiModule` should **not** depend directly on `Sales.Services` (to avoid coupling with implementation details), but it **should** depend on `Contracts`, as it requires access to the `IConsoleCommand` interface.

**Hints:**
1. The new project should be a class library — it can be deployed in any .NET process.  
2. The project should be loaded as a plugin, in a similar way to `Sales.Services`.

Assemblies from any module (including *Sales*) should not depend on the `ConsoleUi` assembly, which is the host process and UI.  
The dependency should go the other way around — we can invert it by moving the `IConsoleCommand` and `IConsole` interfaces into the `Contracts` assembly, under a new `ConsoleUi` folder.

> **Observation:**  
> By doing this, we decouple the application from its UI.  
> All `ConsoleCommand` implementations could be displayed and executed from another host or UI, such as a WPF or web application.

> **Observation:**  
> Review the references by generating a *Project Dependency Diagram*.

---

### 3.3 Order the menu entries

a) In a declarative manner.  
b) By module, and then by additional entries within the same module.

-----------------------------


## 4. Customer Orders Service

> **Objective:** Understand DataAccess implementation.

All the following exercises should be called through a simple UI, such as the Console UI built in the previous labs.

---

### 4.1 Create a new service in the Sales module that returns all customers with orders, ordered by company name

[Optional] Write unit tests for this service. Include tests that verify whether the `OrderBy` is applied correctly.

**Hint (LINQ query):**

```csharp
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

---

### 4.2 [Optional] Add more filters and understand how to unit test them by leveraging the `IRepository` interface’s testability

- Show only the customers whose company name starts with a string read from the console.  
- Show only the customers whose company contains a string read from the console.  
- Write unit tests for the above cases.

---

### 4.3 Create a console command that sets the status of all orders for a given customer

- Add a new operation in the `OrderingService` that sets the status of all orders for a specific customer.  
- Read the relevant customer information from the console to find the orders whose status needs to be changed.  
- The new status can be read from the console or hardcoded for simplicity.

> **Hint:**  
> See the `SalesOrderHeaderStatusValues` class for possible values of the `SalesOrderHeader.Status` property.

------------------

## 5. DataAccess.EntityInterceptors

> **Objective:** Understand and use EntityInterceptors.

Use `EntityInterceptors` and `INotificationService` to implement the following exercises:

---

### 5.1 Implement an `IStateChangeSubscriber<SalesOrderHeader>` that logs to the console when a `SalesOrderHeader` is created, deleted, or changed

The `IStateChangeSubscriber` will be notified through the `Notifications.Services.NotificationService`. This will also allow other subscribers to be notified when a `SalesOrderHeader` entity is created, deleted, or changed.

What classes do need to work together to achieve this?
How are we notified when an entity is created, deleted, or changed?

**Hints:**
- Understand the difference between `GlobalEntityInterceptors` and `EntityInterceptors`.  
- Review the `InterceptorsResolver` to see how they are applied.  
- Check the template implementations:  
  - `InterceptorsResolver : IInterceptorsResolver`  
  - `GlobalEntityInterceptor<T> : IEntityInterceptor<T>`  
- Decide which implementation should be applied in this case.
- 

The `IStateChangeSubscriber<SalesOrderHeader>`should be part of the *Sales* module. Which project should it go into?


### 5.2 Implement a default `IStateChangeSubscriber<T>` that writes to a text file when any DTO is created, deleted, or changed

This should be part of the *Notifications* module.

> **Observation:**  
> Compare and discuss the differences in how and where the notifications are triggered.

-----------------

## 6. Add Entity Auditing

> **Objective:** Understand how DataAccess enables low-cost feature additions.

### 6.1 Consistently set the `ModifiedDate` for all entities that have this column

- Currently, when modifying an order in exercise 4.3 or adding persons in exercise 7, the `ModifiedDate` is not being set.  
- One approach would be to manually update the `ModifiedDate` in all use cases where entities are created or modified. However, this is **cumbersome and error-prone**.  
- Instead, we should leverage the encapsulated Data Access layer and extend the infrastructure with an **interceptor** that automatically sets this value for all relevant entities.

The interface that should be implemented by the data model entities could look like:

```csharp
interface IAuditable
{
    DateTime ModifiedDate { get; set; }
}
```

 - Make all entities from the `Sales.DataModel`, which have the `ModifiedDate` property to implement this interface.
 - Create an global entity interceptor that sets the `ModifiedDate` to `DateTime.UtcNow` whenever an entity implementing `IAuditable` is created or modified.

 Questions:
  - Which kind of interceptor should be used here?
  - How should we register this interceptor so that it is applied to all relevant entities?

------------------

## 7. Products Management Module

---

### 7.0 Add a new module: `ProductsManagement`

> **Objective:** Understand how to add new modules to the Application Infrastructure.

- Create a new module `ProductsManagement` in the `Modules\ProductsManagement` folder.  
- Create a new project `ProductsManagement.Services` in this folder.  
- Implement the class `ProductsManagementModule : IModule` that sends a notification when it becomes active.  
- Add `ProductsManagement.Services` as a plugin in the AppBoot setup.  
- Create the `Products.DataModel` project under the `Modules\ProductsManagement` folder.  
  - Map only the product-related entities (e.g., `Product`, `ProductCategory`, `ProductModel`).  
- Create the `Products.DbContext` project in the same folder.  
  - Implement `IDbContextFactory` to provide the `DbContext` for the `Products.DataModel`.

---

### 7.1 Add a new `ProductCategory`

> **Objective:** Understand how existing Application Infrastructure capabilities can be reused by new modules.

---

### 7.1.1 Create support for reading an entity from the console

- Use the interfaces below and add them to the `Contracts` project so that they can be reused by multiple modules.

```csharp
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

- Create implementations of these interfaces in the `ConsoleUi` project.  
- Create a console command in the `ProductsManagement` module that uses these interfaces to read the data required to create a `ProductCategory` entity.  
  - In this step, only read the data — do not persist it yet.

---

### 7.1.2 Create a service that adds a new `ProductCategory` to the system

- Add a service in the `ProductsManagement` module that adds a new `ProductCategory` to the database.  
  - The service should use the `IUnitOfWork` from the `DataAccess` layer.  
- The console command created earlier (which reads the product category info) should now use this service to insert the newly read product category.

> **Observation:**  
> Notice how the interceptors created in exercises 5 and 6 are automatically triggered when a new `ProductCategory` is added.
> - Is the `NotificationService` called? Why?
> - Is the `ModifiedDate` set? Why?
>     - Discuss the options of coupling the modules through shared entity interfaces (e.g. `IAuditable`) VS module-specific entity interfaces (e.g. `IAuditable`) (Coupling vs Duplication)

--------

## 8. Persons Management Module

---

### 8.0 Add a new module: `PersonsManagement`

> **Objectives:**  
> 1. Understand how to add new modules to the Application Infrastructure.  
> 2. Add new tables to the database.  
> 3. Use services from one module within another.

Step a) Create the database schema and table
 - In the `AdventureWorksLT` database, run the `.Practice/Lab8-CreatePersonsSchema.sql` script to:
  - Create a new schema `Persons` to host the new `Person` table.  
  - Create a new table `Person` in the `Persons` schema, with columns similar to those in the `Customer` table.  

Step b) Create the module and projects
- Create a new module `PersonsManagement` in the `Modules\PersonsManagement` folder.  
- Create a new project `PersonsManagement.Services` in this folder.  
- Implement the class `PersonsManagementModule : IModule` that sends a notification when it becomes active.  
- Add `PersonsManagement.Services` as a plugin in the AppBoot setup.  

Step c) Create the DataModel and DbContext projects
- Create the `Persons.DataModel` project under the `Modules\PersonsManagement` folder.  
  - Map the `Person` table to an entity.  
- Create the `Persons.DbContext` project in the same folder.  
  - Implement `IDbContextFactory` to provide the `DbContext` for the `Persons.DataModel`.

---

### 8.1 Add a new `Person`

> **Objective:** Understand how existing Application Infrastructure capabilities can be reused by new modules.

Step a) Use the following interfaces:

```csharp
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

- Use these interfaces to read the data required to create a new `Person` entity from the console input.  
- The interfaces should already be available in the `Contracts` project, as defined in exercise 7.

---

Step b) Create a service that adds a new `Person` to the system

- Add a service in the `PersonsManagement` module that inserts a new `Person` into the database.  
  - The service should use the `IUnitOfWork` from the `DataAccess` layer.  
- The console command that reads `Person` data should now use this service to persist the new entity.

> **Observation:**  
> Notice how the interceptors created in exercises 5 and 6 are automatically triggered when a new `Person` is added.
> - Is the `NotificationService` called? Why?
> - Is the `ModifiedDate` set? Why?
>     - Discuss the options of coupling the modules through shared entity interfaces (e.g. `IAuditable`) VS module-specific entity interfaces (e.g. `IAuditable`) (Coupling vs Duplication)

---

### 8.2. Import persons as new customers

> **Objective:** Use a service from one module within another module.

- Create a service in the `PersonsManagement` module that retrieves all persons.  
- Create a service in the `Sales` module that imports persons as new customers.  
  - Decide on a key, modified date, or another criterion to identify which persons should be imported as customers and which have already been imported.
