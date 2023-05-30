public static class Settings
{
    private static SettingsLogic _logic = new SettingsLogic();

    public static void Start()
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("View Settings");
        Actions.Add(() => Settings.ViewSettings());

        Options.Add("Change settings");
        Actions.Add(() => Settings.ChangeSettings());

        Options.Add("\nReturn");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ViewSettings()
    {

    }

    public static void ChangeSettings()
    {
        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Colour");
        Actions.Add(() => Settings.ChangeColour());

        Options.Add("\nReturn");
        Actions.Add(() => Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ChangeColour()
    {
        string Question = "What colour should the system be?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (ConsoleColor colour in Enum.GetValues(typeof(ConsoleColor)))
        {
            Options.Add(colour.ToString()!);
            Actions.Add(() => _logic.ChangeColor(colour));
        }

        Options.Add("\nReturn");
        Actions.Add(() => ChangeSettings());

        MenuLogic.Question(Question, Options, Actions);
    }

}