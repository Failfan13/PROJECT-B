using System.Collections.Generic;

public class SettingsLogic
{
    private SettingModel _settings = new SettingModel();

    public SettingsLogic()
    {
        _settings = SettingsAccess.Load();
    }

    public ConsoleColor GetColor()
    {
        return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), _settings.Color);
    }

    public void ChangeColor(ConsoleColor color)
    {
        _settings.Color = color.ToString();
        Update();
    }

    public void Update()
    {
        SettingsAccess.Write(_settings);
    }
}