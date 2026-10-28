using InventoryManager.Models;
using InventoryManager.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using InventoryManager.Exceptions;
using InventoryManager.Data;
using Microsoft.EntityFrameworkCore;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

string connectionString = config.GetConnectionString("InventoryDb")
    ?? throw new InvalidOperationException("Connection string nije pronađen.");
//IProductRepository repo = new SqlProductRepository(connectionString);
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var context = new AppDbContext(options);
IProductRepository repo = new EfProductRepository(context);
//repo.Add(new Product { Name = "Laptop", Price = 999.99m, Category = "Elektronika", Quantity = 5 });



bool radi = true;

while (radi)
{
    Console.WriteLine("\n1. Prikaži sve proizvode");
    Console.WriteLine("2. Dodaj proizvod");
    Console.WriteLine("3. Izmjena proizvoda");
    Console.WriteLine("4. Izbriši proizvod");
    Console.WriteLine("5. Pretraži po kategoriji");
    Console.WriteLine("6. Proizvodi s količinom manjom od 5.");
    Console.WriteLine("0. Izlaz");
    Console.Write("Odabir: ");

    string? unos = Console.ReadLine();
    try
    {
        switch (unos)
        {
            case "1":
                foreach (var p in repo.GetAll())
                {
                    Console.WriteLine($"{p.Id}: {p.Name} - {p.Price:F2} KM ({p.Category}), kom: {p.Quantity}");
                }
                break;
            case "2":
                {
                    Console.Write("Unesite naziv proizvoda: ");
                    string? name = Console.ReadLine();
                    Console.Write("Unesite cijenu proizvoda: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        Console.WriteLine("Neispravna cijena.");
                        break;
                    }
                    Console.Write("Unesite kategoriju proizvoda: ");
                    string? category = Console.ReadLine();
                    Console.Write("Unesite količinu proizvoda: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        Console.WriteLine("Neispravna količina.");
                        break;
                    }

                    var product = new Product
                    {
                        Name = name ?? "",
                        Price = price,
                        Category = category ?? "",
                        Quantity = quantity
                    };
                    repo.Add(product);
                    Console.WriteLine("Proizvod je dodan.");
                }
                break;
            case "3":
                {
                    Console.WriteLine("Unesite ID proizvoda koji želite izmijeniti: ");
                    if (!int.TryParse(Console.ReadLine(), out int editId))
                    {
                        Console.WriteLine("Neispravan Id.");
                        break;
                    }

                    var existing = repo.GetById(editId);
                    if (existing == null)
                    {
                        Console.WriteLine("Proizvod ne postoji.");
                        break;
                    }

                    Console.Write($"Novi naziv ({existing.Name}): ");
                    string? newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                        existing.Name = newName;
                    Console.Write($"Nova cijena ({existing.Price:F2}): ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                        existing.Price = newPrice;

                    Console.Write($"Nova kategorija ({existing.Category}): ");
                    string? newCategory = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newCategory))
                        existing.Category = newCategory;

                    Console.Write($"Nova količina ({existing.Quantity}): ");
                    if (int.TryParse(Console.ReadLine(), out int newQuantity))
                        existing.Quantity = newQuantity;

                    repo.Update(existing);
                    Console.WriteLine("Proizvod izmijenjen.");
                    break;

                }
            case "4":
                {

                    Console.WriteLine("Koji Id želite obrisati?");
                    if (!int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        Console.WriteLine("Neispravan Id.");
                        break;
                    }

                    var toDelete = repo.GetById(deleteId);
                    if (toDelete == null)
                    {
                        Console.WriteLine("Proizvod ne postoji.");
                        break;
                    }
                    Console.Write($"Obrisati '{toDelete.Name}'? (d/n): ");
                    string? potvrda = Console.ReadLine();
                    if (potvrda?.ToLower() == "d")
                    {
                        repo.Delete(deleteId);
                        Console.WriteLine("Proizvod obrisan.");
                    }
                    else
                    {
                        Console.WriteLine("Brisanje otkazano");
                    }
                    break;
                }
            case "5":
                {
                    Console.WriteLine("Unesite kategoriju: ");
                    string? searchCategory = Console.ReadLine();
                    var found = repo.GetAll().
                        Where(p => p.Category.Equals(searchCategory, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (found.Count == 0)
                    {
                        Console.WriteLine("Nema proizvoda u toj kategoriji.");
                        break;
                    }

                    foreach (var p in found)
                    {
                        Console.WriteLine($"{p.Id}: {p.Name} - {p.Price:F2} KM ({p.Category}), kom: {p.Quantity}");
                    }
                    break;
                }
            case "6":
                {
                    var lowStock = repo.GetAll()
                        .Where(p => p.Quantity < 5)
                        .OrderBy(p => p.Quantity)
                        .ToList();

                    if (lowStock.Count == 0)
                    {
                        Console.WriteLine("Nema proizvoda s malom količinom.");
                        break;
                    }

                    foreach (var item in lowStock)
                    {
                        Console.WriteLine($"{item.Id}: {item.Name} - {item.Price:F2} KM ({item.Category}), kom: {item.Quantity}");
                    }
                    break;
                }
            case "0":
                radi = false;
                break;

            default:
                Console.WriteLine("nepoznata opcija");
                break;
        }
    }
    catch (ProductNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (SqlException ex) {
        Console.WriteLine($"Greška s bazom: {ex.Message}");
    }
    catch (DbUpdateException ex)
    {
        Console.WriteLine($"Greška pri spremanju u bazu: {ex.InnerException?.Message ?? ex.Message}");
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Neočekivana greška: {ex.Message}");
    }
}