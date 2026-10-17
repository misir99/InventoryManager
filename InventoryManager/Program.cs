using InventoryManager.Models;
using InventoryManager.Repositories;

string connectionString = "Server=localhost;Database=InventoryDb;Trusted_Connection=True;TrustServerCertificate=True;";

IProductRepository repo = new SqlProductRepository(connectionString);

//repo.Add(new Product { Name = "Laptop", Price = 999.99m, Category = "Elektronika", Quantity = 5 });



bool radi = true;

while (radi)
{
    Console.WriteLine("\n1. Prikaži sve proizvode");
    Console.WriteLine("2. Dodaj proizvod");
    Console.WriteLine("0. Izlaz");
    Console.Write("Odabir: ");

    string? unos = Console.ReadLine();
    switch(unos){
        case "1":
            foreach(var p in repo.GetAll())
            {
                Console.WriteLine(p.Id + ": " + p.Name + " - " + p.Price + " (" + p.Category + "), kom: " + p.Quantity);
            }break;
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
                if(!int.TryParse(Console.ReadLine(), out int quantity))
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
            case "0":
                radi = false;
                break;
            default:
            Console.WriteLine("nepoznata opcija");
            break;
    }
}