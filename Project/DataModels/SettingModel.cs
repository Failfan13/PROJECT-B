using System.Text.Json.Serialization;

public class SettingModel
{
    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("selectColor")]
    public string SelectColor { get; set; }

    [JsonConstructor]
    public SettingModel()
    {
        Color = "White";
        SelectColor = "White";
    }
    public SettingModel(ConsoleColor color)
    {
        Color = color.ToString();
        SelectColor = color.ToString();
    }
    public SettingModel(ConsoleColor color, ConsoleColor selectColor)
    {
        Color = color.ToString();
        SelectColor = selectColor.ToString();
    }


    public void Info()
    {
        Console.WriteLine($"Color: {Color}");
        Console.WriteLine($"Selection Color: {SelectColor}");
    }
}