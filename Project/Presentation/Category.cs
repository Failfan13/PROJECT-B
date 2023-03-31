public static class Category
{
    private static CategoryLogic CL = new CategoryLogic();
    public static void Start()
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>() { "Create new category", "Remove a category", "\nReturn" };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => NewCatMenu());
        Actions.Add(() => RemoveCatMenu());
        Actions.Add(() => Admin.Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void NewCatMenu()
    {
        string newCat = QuestionLogic.AskString("What name should the new category have?");
        CL.CreateNewCategory(new CategoryModel(CL.GetNewestId(), newCat));
    }
    private static void RemoveCatMenu()
    {
        string Question = "What category would you like to remove?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (CategoryModel cat in CL.AllCategories())
        {
            Options.Add(cat.Name);
            Actions.Add(() => CL.DeleteCategory(cat.Id));
        }
        Options.Add("\nReturn");
        Actions.Add(() => Start());

        MenuLogic.Question(Question, Options, Actions);
    }
}