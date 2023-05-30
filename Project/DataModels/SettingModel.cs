using System.Text.Json.Serialization;

public class SettingModel
{
    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonConstructor]
    public SettingModel()
    {
        Color = "White";
    }
    public SettingModel(ConsoleColor color)
    {
        Color = color.ToString();
    }
}