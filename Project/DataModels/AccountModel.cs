using Postgrest.Attributes;
using Postgrest.Models;

[Table("accounts")]
public class AccountModel : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }

    [Column("email_address")]
    public string EmailAddress { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [Column("admin_rights")]
    public bool Admin { get; set; }

    [Column("ad_mails")]
    public bool AdMails { get; set; }

    [Column("complaints")]
    public List<string> Complaints { get; set; }

    public AccountModel NewAccountModel(string emailAddress, string password, string fullName, DateTime dateofbirth)
    {
        EmailAddress = emailAddress;
        Password = password;
        string[] name = fullName.Split(' ');
        FirstName = name[0];
        if (name.Length > 1)
            LastName = string.Join(" ", name.Skip(1));
        DateOfBirth = dateofbirth;
        Complaints = new List<string>{};
        return this;
    }
}




