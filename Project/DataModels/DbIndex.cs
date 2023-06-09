using Postgrest.Attributes;
using Postgrest.Models;
public class DbIndex : BaseModel
{
    [Column("id")]
    public int Id { get; set; }
}