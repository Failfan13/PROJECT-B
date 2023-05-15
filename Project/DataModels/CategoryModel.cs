using System.Text.Json.Serialization;
public class CategoryModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public CategoryModel(int id, string name)
    {
        Id = id;
        Name = name;
    }
}