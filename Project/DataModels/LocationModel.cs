using Postgrest.Attributes;
using Postgrest.Models;

[Table("locations")]
public class LocationModel : BaseModel, IIdentity
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("gmaps_url")]
    public string GmapsUrl { get; set; }

    public LocationModel NewLocationModel(string name, string description, string gmapsUrl, string address)
    {
        Name = name;
        Address = address;
        Description = description;
        GmapsUrl = gmapsUrl;
        return this;
    }
}