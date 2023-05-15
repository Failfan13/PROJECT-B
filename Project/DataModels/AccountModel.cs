using System.Text.Json.Serialization;


public class AccountModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public string DateOfBirth { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("admin")]
    public bool Admin { get; set; }

    [JsonPropertyName("adMails")]
    public bool AdMails { get; set; }

    public AccountModel(int id, string emailAddress, string password, string fullName, string dateofbirth)
    {
        Id = id;
        EmailAddress = emailAddress;
        Password = password;
        FullName = fullName;
        DateOfBirth = dateofbirth;
        Admin = false;
        AdMails = false;
    }

}




