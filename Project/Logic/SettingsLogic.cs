using System.Collections.Generic;

public static class SettingsLogic
{
    private static SettingModel _settings = new SettingModel();

    static SettingsLogic()
    {
        _settings = SettingsAccess.Load();
    }

    public static SettingModel GetSettings()
    {
        return _settings;
    }

    public static ConsoleColor GetColor()
    {
        return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), _settings.Color);
    }

    public static ConsoleColor GetSelectColor()
    {
        return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), _settings.SelectColor);
    }

    public static void ChangeColor(ConsoleColor color)
    {

        _settings.Color = color.ToString();
        Update();
    }

    public static void Update()
    {
        SettingsAccess.Write(_settings);
    }

    public static void ChangeMenuColor(ConsoleColor color)
    {
        _settings.SelectColor = color.ToString();
        Update();
    }
}