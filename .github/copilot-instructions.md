# Copilot Instructions for This Repository

> Purpose: Teach GitHub Copilot (Chat, Ask, and Agent modes) how to work in this codebase. These rules also help human contributors stay consistent.

---

## 0) Project Identity
- **Architecture style:** Clean Architecture + modular boundaries
- **Primary language:** C# (.NET)
- **Patterns:** DI-first, Controlled References, Plugin Applications, Infrastructure, iFX, Hidden Frameworks
- **Packages:**  EF Core, Microsoft.Extensions.Logging, Microsoft.Extensions.DependencyInjection
- **Domains/Modules:** e.g., `Sales`, `Notifications`, `Export`

> **Copilot:** Prefer conventional solutions using the libraries above. If a library is not present, scaffold minimal, idiomatic .NET code without adding new dependencies.

---

## 1) Folder & Layering Rules (must follow)
```
repo-root/
├─ Infra/                         # Application Infrastruture (Application Framework, DataAccess, Logging, Messaging etc) 
│  ├─ AppBoot/                    # dependency injection, modules composition, app startup, plugins dynamic load
│  ├─ AppBoot.UnitTests/          # Unit tests for AppBoot
│  ├─ DataAccess/                 # Hides EF Core, IRepository and IUnitOfWork implementations
├─ Modules/                       # Functionalities grouped by domain (Sales, Notifications, Export etc).
│  ├─ Contracts/                  # Contracts shared between modules (e.g., Events, Messages, DTOs). No logic here!
│  ├─ Sales/                      # Sales module (example)
│  │  ├─ Sales.Services/          # Use-cases implementations, domain services.
│  │  ├─ Sales.DataModel/         # [Example] Entities, DTOs mapped to DB tables. No logic here! (no if, while, logical expressions etc.). NO reference to EF Core!
│  │  ├─ Sales.DbContext/         # [Example] EF DbContext for Sales module
│  │  └─ Sales.Console/           # [Optional] Console UI commands specific to sales module.
│  └─ Notifications/              # Notifications module (example)
│     └─ Notifications.Services/  # Use-cases implementations, domain services.
└─ UI/                            # User Interface layer / Clients   
   └─ ConsoleUi/                  # Console application (CLI)
```

### Dependency boundaries
- `Infra/*` → implements ports for DB, messaging, HTTP, files, dymamic load of plugins; registers via DI; no domain logic.
- `Modules/Contracts` → **no** references to anything. Only pure DTOs and interfaces. No logic
- `Modules/*` → **no** references to other modules. Only references to **Contracts** and **Infra**.
- `Modules/*/*.DataModel` → **no** logic; only entities/DTOs; no references to EF Core or other frameworks.
- `Modules/*/*.Services` → references **Contracts** and **DataModel**; NO references ot EF Core or other frameworks. Contains domain logic and use-cases.
- `UI/*` → references **Modules/Contracts** and **Infra**; NO references to **Modules/*/Services**. No domain logic.


> **Copilot:** If a change violates these rules, raise an error instead of making the change.

---

## 2) Registering in DI
- Use `ServiceAttribute` from `Infra/AppBoot` to register services in DI.
- The `ServiceAttribute` decorates the implementation class, specifying the service lifetime and the interface to register.
- Register only interfaces, not concrete classes.
- Example of the `PriceCalculator` class registered as the implementation of the `IPriceCalculator` interface:
 ```
 [Service(typeof(IPriceCalculator), ServiceLifetime.Transient)]
 class PriceCalculator : IPriceCalculator
 {
    public decimal CalculateTaxes(OrderRequest o, Customer c)
    {
    }
  }
 ```

## 3) Coding Conventions (C#)
- **Async:** Prefer async/await end-to-end. Do not block (`.Result`, `.Wait()`), avoid `async void` (except event handlers).
- **Nullability:** Treat nullable warnings as errors; initialize properly.
- **Exceptions:** Throw boundary-specific exceptions from Infra
- **Logging:** Use `ILogger<T>`
- **Mapping:** Use manual mapping rather the frameworks.
- **Immutability:** Prefer records for value objects; entities keep private setters where possible.
- **Configuration:** Use `IOptions<T>`; **never** hardcode secrets or connection strings.
- **Comments:** Avoid using comments. Prefer explicit code.

---

## 5) AppBoot Plugin Model
- AppBoot supports dynamic loading of modules as plugins at runtime.
- In `Program.cs` where AppBoot is configured, use `.AddPlugin()` to specify the modules that should be loaded as plugins.
- Load as plugins all assemblies that are not referenced by any other assembly at compile time (i.e., have no incoming `ProjectReference` in the solution).
- Each call to `.AddPlugin()` creates a LoadContext isolated for that module assembly; dependent assemblies passed in the dependency array are loaded into the same LoadContext.
- `AddPlugin()` accepts modules names, which are built by convvention as `{ModuleName}.{AssemblySuffix}`. 
  - The `ModuleName` correspond to the folder name under `Modules/` (e.g., `Sales`, `Notifications`).
  - The `AssemblySuffix` is the assembly name without the module name prefix (e.g., `Services`, `DbContext`).
  - Assembly are named by convention as `{ModuleName}.{AssemblySuffix}` (e.g., `Sales.Services`, `Notifications.Services`, `Sales.DbContext`).
- When a module has dependent assemblies that are not referenced by the assembly that gives the plugin name, specify their names in the `.AddPlugin()` dependency parameter.
    - Example: `.AddPlugin("Sales.Services", new[] { "Sales.DbContext" })` — each string is a simple module name (not a file path or DLL filename).

## 6) Build for Dev/Debug
- Some plugin assemblies are not referenced by the host or by other projects. These assemblies are loaded dynamically at runtime and must be built for Dev/Debug.
- Ensure those assemblies are included in the Visual Studio build by adding them as build dependencies of the host or plugin root project using the __Project Build Dependencies__ feature in the solution.
    - Steps: right‑click the solution → choose __Project Build Dependencies__ → select the dependent projects (for example, add plugin projects as dependencies of `UI/ConsoleUi` or the plugin root).
    - The selection is saved in the `.sln` file and is not part of individual project files.
- If a plugin has additional assemblies that are not directly referenced, add those dependent projects as build dependencies of the plugin root project as well.
    - Example: `Sales.DbContext` is a dependency of the `Sales.Services` plugin; add `Sales.DbContext` as a build dependency of the `Sales.Services` project so both are built in Debug.

## 7) Data & Persistence
- `Infra/DataAcces` abstractions only, like `IRepository` or `IUnitOfWork`. Do not use directly EF Core. Do not take hard dependencies to EF Core. 
- Use `IRepository` for read only cases; Get the `IRepository` via DI.
- Use `IUnitOfWork` for transactional operations; Get the `IUnitOfWork` via a factory function (`IRepository.CreateUnitOfWork`).

---

## 8) Console UI
- Host project is `UI/ConsoleUi/`.
- Each module has its own subfolder under `Modules/` for console commands (e.g., `Modules/Sales/Console/`).
- The modules do not directly depend `UI/ConsoleUi/`; instead, commands implement interfaces defined in `Modules/Contracts/Console/`.

---


## 9) Files Copilot Must Not Modify
- Any file under `Infra/**`
- Any file under `*/DbContext`
- Any `*.csproj` file

> **Copilot:** If a change is requested in these paths, reply with an alternative that keeps generated/third-party code intact (e.g., partial class, extension method, adapter).

---

## 10) How Copilot Should Respond (Style Guide)
- Prefer **small, reviewable diffs**. If a change spans multiple files, list a plan first.
- Quote path-relative file names in suggestions (e.g., `src/Application/Orders/GetOrderHandler.cs`).
- Include a brief checklist with each plan.
- If context is missing, ask **one clear question** before proceeding.

**Example response skeleton:**
```
Plan
1) Add DTO + validator in Application
2) Add handler with repository usage
3) Register mapping in Infrastructure
4) Expose endpoint in WebApi

Files to change
- src/Application/Orders/GetOrderQuery.cs (new)
- src/Application/Orders/GetOrderValidator.cs (new)
- src/Application/Orders/GetOrderHandler.cs (new)
- src/WebApi/Endpoints/Orders/Get.cs (new)

Notes
- Respects Clean Architecture
- Async end-to-end, validation + logging included
```

---

## 11) Agent Mode: Multi‑Step Tasks (Labs/Exercises)
> Use this when executing a lab or implementing a feature end-to-end.

**Agent prompt template:**
```
Goal: <short goal>
Constraints: respect layering; do not modify Generated/ThirdParty; async only; add tests.
Steps:
1) <step>
2) <step>
3) <step>
Deliverables: list changed files, build passes, tests added and green.
```

**Acceptance checklist (Agent should self-check):**
- [ ] Build succeeds (`dotnet build`)
- [ ] New/changed public APIs have tests
- [ ] Validation + error handling present
- [ ] No boundary violations (Core ↔ Infrastructure)
- [ ] No secrets, no hardcoded connection strings

---
## 12) Performance & Resilience Hints
- Use cancellation tokens for all I/O.
- Add Polly policies at HTTP/DB/messaging boundaries as appropriate.
- Avoid synchronous over async calls.

---

## 13) If You’re Unsure (Copilot)
- Ask a single clarifying question with the minimal diff you can produce safely.
- Prefer adapters, ports, or extension methods over changing existing contracts.

---

## 14) Local Overrides
If this repository provides a `.github/copilot-exclude.yml`, treat those paths as **off-limits for context** and avoid proposing changes there.

---
