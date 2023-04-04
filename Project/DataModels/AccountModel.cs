using System.Text.Json.Serialization;


public class AccountModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    [JsonPropertyName("admin")]
    public bool Admin { get; set; }

    [JsonPropertyName("Adult")]
    public bool Adult { get; set; }

    public AccountModel(int id, string emailAddress, string password, string fullName, bool adult)
    {
        Id = id;
        EmailAddress = emailAddress;
        Password = password;
        FullName = fullName;
        Admin = false;
        Adult = adult;
    }

}




