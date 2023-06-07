using Postgrest.Attributes;
using Postgrest.Models;

[Table("promotions")]
public class PromoModel : BaseModel, IIdentity
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("code")]
    public string Code { get; set; }

    [Column("active")]
    public bool Active { get; set; }

    [Column("condition")]
    public Dictionary<string, IEnumerable<object>>? Condition { get; set; }

    public PromoModel NewPromoModel(string code)
    {
        Code = code;
        Active = true;
        Condition = new Dictionary<string, IEnumerable<object>>();
        return this;
    }
}