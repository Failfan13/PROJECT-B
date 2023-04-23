// Format/formats will be the extra options as IMAX, 3D, REX etc.
public static class Format
{
    public static void Start(TimeSlotModel tsm, List<SeatModel> Seats) => Start(tsm, Seats, null!, false);
    public static void Start(TimeSlotModel tsm, List<SeatModel> Seats, Dictionary<int, int> snacks = null!, bool IsEdited = false)
    {
        ReservationLogic ReservationLogic = new ReservationLogic();
        Console.Clear();
        FormatDetails? formatDt = FormatsLogic.GetByFormat(tsm.Format);

        Console.WriteLine(@$"For this movie there are certain viewing formats available, For the timeslot you choose
The following format is applied: {tsm.Format}

{(formatDt?.Item != "" ? @$"The movie format will require: {formatDt?.Item} for the optimal viewing experience
The total price for the extra requirements will be: {formatDt?.Item} x {Seats.Count()} - Price: € {formatDt?.Price * Seats.Count()}" : "")}");

        QuestionLogic.AskEnter();

        new ReservationLogic().MakeReservation(tsm, Seats, snacks, tsm.Format, IsEdited);
    }

    public static void ChangeFormats(object formatModel, Action returnTo = null!)
    {
        string Question = "Would you like to change viewing methods?";
        List<string> Options = new List<string>() { "Add a viewing method", "Remove a viewing method" };
        List<Action> Actions = new List<Action>();

        Actions.Add(() => Format.AddViewFormat(formatModel));
        Actions.Add(() => Format.RemoveViewFormat(formatModel));

        Options.Add("Return");
        if (returnTo != null)
        {
            Actions.Add(() => returnTo());
        }
        else Actions.Add(() => Console.WriteLine());


        MenuLogic.Question(Question, Options, Actions);
    }

    public static void AddViewFormat(object formatModel)
    {
        MoviesLogic MoviesLogic = new MoviesLogic();
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();

        string Question = "Select the format you want to add";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (formatModel is TimeSlotModel)
        {
            TimeSlotModel? model = formatModel as TimeSlotModel;
            MovieModel movieFormats = MoviesLogic.GetById(model!.MovieId)!;
            if (movieFormats.Formats.Any())
            {
                foreach (var format in movieFormats.Formats)
                {
                    Options.Add(format);
                    Actions.Add(() => TimeSlotsLogic.AddFormat(model, format));
                }

                MenuLogic.Question(Question, Options, Actions);
            }
        }
        else if (formatModel is MovieModel)
        {
            MovieModel? model = formatModel as MovieModel;
            if (FormatsLogic.AllFormats().Any())
            {
                foreach (var format in FormatsLogic.AllFormats())
                {
                    Options.Add(format);
                    Actions.Add(() => MoviesLogic.AddFormat(model!, format));
                }

                Options.Add("Return");
                Actions.Add(() => ChangeFormats(formatModel));

                MenuLogic.Question(Question, Options, Actions);
            }
            MoviesLogic.UpdateList(model!);
        }

        ChangeFormats(formatModel);
    }

    public static void RemoveViewFormat(object formatModel)
    {
        MoviesLogic MoviesLogic = new MoviesLogic();
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();

        string Question = "Select the format you want to remove";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (formatModel is TimeSlotModel)
        {
            TimeSlotModel? model = formatModel as TimeSlotModel;
            if (model!.Format != "")
            {
                Options.Add(model.Format);
                Actions.Add(() => TimeSlotsLogic.RemoveFormat(model));
                MenuLogic.Question(Question, Options, Actions);
            }
        }
        else
        {
            MovieModel? model = formatModel as MovieModel;
            if (model!.Formats.Any())
            {
                foreach (var format in model.Formats)
                {
                    Options.Add(format);
                    Actions.Add(() => MoviesLogic.RemoveFormat(model, format));
                }
                MenuLogic.Question(Question, Options, Actions);
            }
            else
            {
                Console.WriteLine("There are no formats to remove");
            }

            MoviesLogic.UpdateList(model);
        }

        ChangeFormats(formatModel);
    }
}