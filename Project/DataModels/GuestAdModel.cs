using Postgrest.Attributes;
using Postgrest.Models;

[Table("guest_ads")]
public class GuestAdModel : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("date_time")]
    public DateTime dateTime { get; set; }

    [Column("email")]
    public string Email { get; set; }

    public GuestAdModel NewGuestAdModel(string email)
    {
        dateTime = DateTime.Now;
        Email = email;
        return this;
    }
}