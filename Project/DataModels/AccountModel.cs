using Postgrest.Attributes;
using Postgrest.Models;

[Table("accounts")]
public class AccountModel : BaseModel2
{
    [PrimaryKey("id, false")]
    public int Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("date_of_birth")]
    public string DateOfBirth { get; set; }

    [Column("email_address")]
    public string EmailAddress { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [Column("admin")]
    public bool Admin { get; set; }

    [Column("ad_mails")]
    public bool AdMails { get; set; }

    [Column("adult")]
    public bool Adult { get; set; }

    [Column("complaints")]
    public List<string> Complaints { get; set; }

    public AccountModel NewAccountModel(string emailAddress, string password, string fullName, string dateofbirth)
    {
        EmailAddress = emailAddress;
        Password = password;
        string name = fullName;
        FirstName = name.Split(' ')[0];
        LastName = name.Split(' ')[1];
        DateOfBirth = dateofbirth;
        Admin = false;
        AdMails = false;
        Adult = false;
        Complaints = new List<string>();
        return this;
    }
}




