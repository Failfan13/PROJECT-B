﻿using System.Text.Json.Serialization;


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
  
    [JsonPropertyName("dateofbirth")]
    public string DateOfBirth { get; set; }
  
    [JsonPropertyName("admin")]
    public bool Admin { get; set; }
  
    [JsonPropertyName("adMails")]
    public bool AdMails { get; set; }
  
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }
  
    [JsonPropertyName("complaints")]
    public List<string> Complaints { get; set; }

    public AccountModel(int id, string emailAddress, string password, string fullName, string dateofbirth)
    {
        Id = id;
        EmailAddress = emailAddress;
        Password = password;
        FullName = fullName;
        DateOfBirth = dateofbirth;
        Admin = false;
        AdMails = false;
        Adult = false;
        Complaints = new List<string>();
    }
}




