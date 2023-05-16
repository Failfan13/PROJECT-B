using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

public class LocationModel
{
    [Name("name")]
    public string Name { get; set; }

    [Name("address")]
    public string Address { get; set; }

    [Name("description")]
    public string Description { get; set; }

    [Name("gmapsUrl")]
    public string GmapsUrl { get; set; }

    public LocationModel(string name, string description, string gmapsUrl, string address)
    {
        Name = name;
        Address = address;
        Description = description;
        GmapsUrl = gmapsUrl;
    }
}