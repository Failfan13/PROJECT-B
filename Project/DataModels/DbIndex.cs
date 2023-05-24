using Postgrest.Attributes;
using Postgrest.Models;
public class DbIndex
{
    [Column("id")]
    public int Id { get; set; }
}