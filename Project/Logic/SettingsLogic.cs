using System.Collections.Generic;

public class SettingsLogic
{
    private SettingModel _settings;

    public SettingsLogic()
    {
        _settings = SettingsAccess.Load();
    }

    public void ChangeColour(ConsoleColor colour)
    {
        _settings.Colour = colour;
    }

    public void Update()
    {
        SettingsAccess.Write(_settings);
    }
}