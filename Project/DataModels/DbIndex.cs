using Postgrest.Attributes;
using Postgrest.Models;
public class DbIndex : BaseModel, IIdentity
{
    [Column("id")]
    public int Id { get; set; }
}