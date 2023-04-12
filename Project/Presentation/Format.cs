// Format/formats will be the extra options as IMAX, 3D, REX etc.
public static class Format
{
    public static void ChangeFormats<T>(T formatModel, Action Action = null)
    {
        string Question = "Would you like to change viewing methods?";
        List<string> Options = new List<string>() { "Add a viewing method", "Remove a viewing method" };
        List<Action> Actions = new List<Action>();

        Actions.Add(() => Format.AddViewFormat<T>(formatModel));
        Actions.Add(() => Format.RemoveViewFormat<T>(formatModel));

        Options.Add("Return");
        Actions.Add(() => Console.WriteLine(""));

        MenuLogic.Question(Question, Options, Actions);

        Action?.Invoke();
    }

    public static void AddViewFormat<T>(T formatModel) where T : MovieModel
    {
        string Question = "Select the format you want to add";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (MoviesLogic.AllFormats().Any())
        {
            foreach (var format in MoviesLogic.AllFormats())
            {
                Options.Add(format);
                Actions.Add(() => MoviesLogic.AddFormat(formatModel, format));
            }
            MenuLogic.Question(Question, Options, Actions);
        }
        MoviesLogic.UpdateList(formatModel);

        ChangeFormats<T>(formatModel);
    }
    public static void AddViewFormat<T>(TimeSlotModel formatModel)
    {
        string Question = "Select the format you want to add";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        MoviesLogic MoviesLogic = new MoviesLogic();
        MovieModel movie = MoviesLogic.GetById(formatModel.MovieId);

        if (movie.Formats.Any())
        {
            foreach (var format in movie.Formats)
            {
                Options.Add(format);
                Actions.Add(() => TimeSlotsLogic.AddFormat(formatModel, format));
            }
            MenuLogic.Question(Question, Options, Actions);
        }
        TimeSlotsLogic.UpdateList(formatModel);

        ChangeFormats<T>(formatModel);
    }

    public static void RemoveViewFormat<T>(T formatModel) where T : MovieModel
    {
        string Question = "Select the format you want to remove";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (formatModel.Formats.Any())
        {
            foreach (var format in formatModel.Formats)
            {
                Options.Add(format);
                //Actions.Add(() => MoviesLogic.RemoveFormat(formatModel, format));
            }
            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Console.WriteLine("There are no formats to remove");
        }

        MoviesLogic.UpdateList(formatModel);

        ChangeFormats<T>(formatModel);
    }
    public static void RemoveViewFormat<T>(TimeSlotModel formatModel)
    {
        string Question = "Select the format you want to remove";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (formatModel.Format.Any())
        {
            foreach (string format in formatModel.Format)
            {
                Options.Add(format);
                //Actions.Add(() => TimeSlotsLogic.RemoveFormat(formatModel, format));
            }
            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Console.WriteLine("There are no formats to remove");
        }

        TimeSlotsLogic.UpdateList(formatModel);
        ChangeFormats<T>(formatModel);
    }
}