public static class Settings
{
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
        Console.Clear();
        Console.WriteLine("View the application settings below");

        SettingsLogic.GetSettings().Info();

        Console.WriteLine("");
        QuestionLogic.AskEnter();
    }

    public static void ChangeSettings()
    {
        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("App color");
        Actions.Add(() => Settings.ChangeColorMenu(false));

        Options.Add("Selection color");
        Actions.Add(() => Settings.ChangeColorMenu(true));

        Options.Add("\nReturn");
        Actions.Add(() => Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ChangeColorMenu(bool isSelection)
    {
        string Question = "What color should the system be?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (ConsoleColor colour in Enum.GetValues(typeof(ConsoleColor)))
        {
            Options.Add(colour.ToString()!);
            if (!isSelection) Actions.Add(() => Settings.ChangeColor(colour));
            else Actions.Add(() => SettingsLogic.ChangeMenuColor(colour));
        }

        Options.Add("\nReturn");
        Actions.Add(() => ChangeSettings());

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void ChangeColor(ConsoleColor color)
    {
        Console.Clear();
        Console.WriteLine($"Change selections too {color.ToString()}? (y/n)");
        ConsoleKeyInfo key = Console.ReadKey();
        if (key.Key == ConsoleKey.Y) SettingsLogic.ChangeMenuColor(color);
        SettingsLogic.ChangeColor(color);
    }
}