using Postgrest.Attributes;
using Postgrest.Models;

[Table("snacks")]
public class SnackModel : BaseModel, IIdentity
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("price")]
    public double Price { get; set; }


    public SnackModel NewSnackModel(string name, List<string> sizeList, double price)
    {
        Name = name;
        Price = price;
        return this;
    }

    public void Info()
    {
        Console.WriteLine($"Snack:      \t{Name}");
        Console.WriteLine($"Price:      \t{Price}");
        Console.Write("\n");
    }

}




