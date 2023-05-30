using System.Text.Json.Serialization;

public class SettingModel
{
    [JsonPropertyName("colour")]
    public ConsoleColor Colour { get; set; }

    public SettingModel(ConsoleColor colour)
    {
        Colour = colour;
    }
}