using System.Globalization;
static class Movies
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();
    static private CategoryLogic CategoryLogic = new CategoryLogic();

    public static void ShowAllMovies()
    {
        foreach (MovieModel movie in MoviesLogic.AllMovies())
        {
            movie.Info();
        }
    }

    public static void AddNewMovie()
    {
        bool CorrectDate = true;

        int Duration = 0;
        string Title = "";
        string Director = "";
        string Description = "";
        DateTime ReleaseDate = new DateTime();
        double Price = 0;
        List<CategoryModel> Categories = new List<CategoryModel> { };
        List<string> Formats = new List<string> { };

        Console.Clear();
        Title = QuestionLogic.AskString("What is the title of the movie?");

        Console.Clear();
        while (CorrectDate)
        {
            Console.WriteLine("What is the release date of the movie? (dd/mm/yyyy): ");
            try
            {
                ReleaseDate = Convert.ToDateTime(Console.ReadLine());
                CorrectDate = false;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Wrong date format!");
            }
        }

        Description = QuestionLogic.AskString("What is the description of the movie? ");
        Director = QuestionLogic.AskString("Who is the director of the movie?: ");
        Duration = (int)QuestionLogic.AskNumber("What is the duration? (minutes)");
        Price = (int)QuestionLogic.AskNumber("How expensive is the movie?: ");

        bool nomorecategories = false;
        do
        {
            int anothercat = (int)QuestionLogic.AskNumber("Add another category?");
            if (anothercat == 1)
            {
                List<CategoryModel> cats = CategoryLogic.AllCategories();
                Console.Clear();
                foreach (CategoryModel c in cats)
                {
                    Console.WriteLine($"{c.Id} {c.Name}");
                }
                int categorytoadd = (int)QuestionLogic.AskNumber("What category do you want to add");
                CategoryModel category = CategoryLogic.GetById(categorytoadd);
                if (!Categories.Contains(category))
                {
                    Categories.Add(category);
                }
            }
            else if (anothercat == 2)
                nomorecategories = true;
        }
        while (nomorecategories != false);

        MovieModel movie = MoviesLogic.NewMovie(Title, ReleaseDate, Director, Description, Duration, Price, Categories, Formats);

        Format.ChangeFormats(movie);

        TimeSlots.NewTimeSlot(movie.Id);

        Console.Clear();
        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
        Console.WriteLine($"Director: {movie.Director}");
        Console.WriteLine($"Price: {movie.Price}");
        Console.WriteLine($"Categories: {string.Join(", ", movie.Categories)}");
        Console.WriteLine($"Formats: {string.Join(", ", movie.Formats)}");

        //Menu.Start();
    }

    public static void ChangeMoviesMenu()
    {
        Console.Clear();
        List<MovieModel> movies = MoviesLogic.AllMovies();
        string Question = "What movie would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        foreach (var movie in movies)
        {
            Options.Add(movie.Title);
            Actions.Add(() => ChangeMovieMenu(movie));
        }

        Options.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);

    }

    public static void ChangeMovieMenu(MovieModel movie)
    {
        Console.Clear();
        //Console.WriteLine("What would you like to do?");
        string Question = "What would you like to change";

        List<string> Options = new List<string>() {
            "Change title", "Change Director",
            "Change Releasedate", "Change Description",
            "Change Duration", "Change Price",
            "Change Categories", "Change Formats"
        };

        List<Action> Actions = new List<Action>() { };
        Actions.Add(() => ChangeTitle(movie));
        Actions.Add(() => ChangeDirector(movie));
        Actions.Add(() => ChangeReleaseDate(movie));
        Actions.Add(() => ChangeDescription(movie));
        Actions.Add(() => ChangeDuration(movie));
        Actions.Add(() => ChangePrice(movie));
        Actions.Add(() => ChangeCategory(movie));
        Actions.Add(() => Format.ChangeFormats(movie, () => ChangeMovieMenu(movie)));

        Options.Add("Return");
        Actions.Add(() => ChangeMoviesMenu());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ChangeCategory(MovieModel movie)
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>() { "Add a category", "Remove a category" };
        List<Action> Actions = new List<Action>();

        Actions.Add(() => CategoryLogic.AddCategory(movie));
        Actions.Add(() => CategoryLogic.RemoveCategory(movie));

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void ChangeTitle(MovieModel movie)
    {
        Console.Clear();
        string NewTitle = QuestionLogic.AskString("What do you want to change the title of this movie to?");
        MoviesLogic.ChangeTitle(movie, NewTitle);
        Console.WriteLine($"Title is now: {NewTitle}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangeDirector(MovieModel movie)
    {
        Console.Clear();
        string NewDirector = QuestionLogic.AskString("What do you want to change the director of this movie to?");
        MoviesLogic.ChangeDirector(movie, NewDirector);
        Console.WriteLine($"Director is now: {NewDirector}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangeDescription(MovieModel movie)
    {
        Console.Clear();
        string NewDescription = QuestionLogic.AskString("What do you want to change the description of this movie to?");
        MoviesLogic.ChangeDescription(movie, NewDescription);
        Console.WriteLine($"Description is now: {NewDescription}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangeDuration(MovieModel movie)
    {
        int NewDuration = (int)QuestionLogic.AskNumber("What do you want to change the duration of this movie to? (please enter the ammount of minutes)");
        MoviesLogic.ChangeDuration(movie, NewDuration);
        Console.WriteLine($"Duration is now: {NewDuration} minutes");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangePrice(MovieModel movie)
    {
        double NewPrice = QuestionLogic.AskNumber("What do you want to change the price of this movie to?");
        MoviesLogic.ChangePrice(movie, NewPrice);
        Console.WriteLine($"Price is now: {NewPrice}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangeReleaseDate(MovieModel movie)
    {
        Console.Clear();
        DateTime NewReleaseDate = new DateTime();
        bool CorrectDate = true;
        while (CorrectDate)
        {
            Console.WriteLine("What is the release date of the movie? (dd/mm/yyyy): ");
            try
            {
                NewReleaseDate = Convert.ToDateTime(Console.ReadLine());
                CorrectDate = false;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Wrong date format!");
            }
        }
        MoviesLogic.ChangeReleaseDate(movie, NewReleaseDate);
        Console.WriteLine($"Release date  is now: {NewReleaseDate.Date}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }

    public static void AddReviewMenu()
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        ReviewLogic RL = new ReviewLogic();

        List<ReviewModel> pastReviews = RL.UserPastReviews(AccountsLogic.CurrentAccount!.Id);

        List<ReservationModel> pastMoviesNoReview = MoviesLogic.PastMovies().FindAll(
            m => !pastReviews.Any(r => r.MovieId == TL.GetById(m.TimeSLotId)!.MovieId));

        Console.Clear();

        if (pastMoviesNoReview.Count == 0)
        {
            Console.WriteLine("You have no past reservations");
            QuestionLogic.AskEnter();
            return;
        }

        string question = "What movie would you like to add a review for?";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        foreach (ReservationModel pastReservation in pastMoviesNoReview)
        {
            try
            {
                options.Add($"Movie: {MoviesLogic.GetById(TL.GetById(pastReservation.TimeSLotId)!.MovieId)!.Title} Watched on: {pastReservation.DateTime}");
                actions.Add(() => AddNewReview(TL.GetById(pastReservation.TimeSLotId)!.MovieId, pastReservation));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        MenuLogic.Question(question, options, actions);
        Menu.Start(); // needs to go to add review section
    }

    public static void EditReviewsMenu()
    {
        ReviewLogic RL = new ReviewLogic();

        Console.Clear();

        string question = "What would you like to do?";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        options.Add("All reviews");
        actions.Add(() => RL.ViewReviews(false, false));
        options.Add("All revies for specific movie");
        actions.Add(() => RL.ViewReviews(false, true));
        options.Add("All reviews for specific user");
        actions.Add(() => RL.ViewReviews(true, false));
        options.Add("All reviews for specific user and movie");
        actions.Add(() => RL.ViewReviews(true, true));

        options.Add("Return");
        actions.Add(() => Menu.Start());

        MenuLogic.Question(question, options, actions);
        Admin.Start();
    }

    // Questions user for new review
    public static void AddNewReview(int MovieId, ReservationModel pastReservation)
    {
        MoviesLogic ML = new MoviesLogic();
        ReviewLogic RL = new ReviewLogic();

        MovieModel Movie = ML.GetById(MovieId)!;

        double rating = 0;

        if (Movie == null) return;

        Console.Clear();

        while (true)
        {
            Console.WriteLine("Add new review by entering a rating between 1 and 5 (can be specific bv 4.75)");
            string input = Console.ReadLine()!.Replace(',', '.');


            if (double.TryParse(input, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out double newRating) && newRating >= 1 && newRating <= 5)
            
            {
                rating = newRating;
                break;
            }
            Console.WriteLine("Review must be between 1 and 5");
        }


        Console.WriteLine("Would you like to add a message to the review (y/n)");
        ConsoleKeyInfo messageInput = Console.ReadKey();


        string message = "";
        if (messageInput.Key == ConsoleKey.Y)
        {
            Console.WriteLine("\nEnter your message (max 255 characters): ");
            message = Console.ReadLine()!;
            message = ReviewLogic.CutReviewMessage(message); // cuts message to size
        }

        RL.SaveNewReview(message, rating, pastReservation); // saves message to CSV

        RL.UpdateMovieReviews(ML.AllMovies());
        ML.UpdateList(Movie);
    }
}

