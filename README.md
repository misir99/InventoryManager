# InventoryManager

Konzolna aplikacija za upravljanje zalihama proizvoda, napisana u C#-u.
Podržava dodavanje, pregled, izmjenu i brisanje proizvoda (CRUD), pretraživanje
po kategoriji i prikaz proizvoda s malom količinom na skladištu.

## Tehnologije

- C# / .NET 8
- SQL Server (ADO.NET, Microsoft.Data.SqlClient)
- Repository pattern (sučelje `IProductRepository`)
- LINQ
- Konfiguracija preko `appsettings.json`

## Struktura projekta

- `Models/` – klasa `Product`
- `Repositories/` – sučelje `IProductRepository` i implementacija `SqlProductRepository`
- `Program.cs` – konzolni izbornik

## Pokretanje

1. Kreiraj bazu `InventoryDb` u SQL Serveru i pokreni skriptu:

```sql
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Quantity INT NOT NULL DEFAULT 0
);
```

2. U `appsettings.json` provjeri da connection string odgovara tvom serveru.
3. Pokreni projekt u Visual Studiju (F5) ili s `dotnet run`.

## Mogućnosti

1. Prikaz svih proizvoda
2. Dodavanje proizvoda
3. Izmjena proizvoda
4. Brisanje proizvoda (uz potvrdu)
5. Pretraga po kategoriji
6. Proizvodi s količinom manjom od 5

## Planirano

- Entity Framework Core
- ASP.NET Core Web API
