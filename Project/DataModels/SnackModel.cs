using System.Text.Json.Serialization;


public class SnackModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("snack")]
    public string Name { get; set; }

    [JsonPropertyName("sizes")]
    public List<string> SizeList { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }


    public SnackModel(int id, string name, List<string> sizeList, double price)
    {
        Id = id;
        Name = name;
        SizeList = sizeList;
        Price = price;
    }

    public void Info()
    {
        Console.WriteLine($"Snack:      \t{Name}");
        Console.WriteLine($"Sizes:");
        foreach (var size in SizeList)
        {
            Console.Write($"\t{size}\n");
        }
        Console.WriteLine($"Price:      \t{Price}");
        Console.Write("\n");
    }

}




