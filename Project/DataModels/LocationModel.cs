using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

public class LocationModel
{
    [Name("name")]
    public string Name { get; set; }

    [Name("description")]
    public string Description { get; set; }

    [Name("gmapsUrl")]
    public string GmapsUrl { get; set; }

    public LocationModel(string name, string description, string gmapsUrl)
    {
        Name = name;
        Description = description;
        GmapsUrl = gmapsUrl;
    }
}