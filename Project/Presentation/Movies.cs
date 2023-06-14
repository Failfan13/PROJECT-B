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
        DateTime releaseDate = new DateTime();
        CategoryLogic categoryLogic = new CategoryLogic();

        List<string> formats = new List<string>();
        List<CategoryModel> categories = new List<CategoryModel>();

        string title = "";
        bool correctDate = false;
        int duration = 0;
        string director = "";
        string description = "";
        double price = 0;
        bool ads = false;

        Console.Clear();

        // sets title
        title = QuestionLogic.AskString("What is the title of the movie?");

        // sets release date
        Console.Clear();
        while (!correctDate)
        {
            Console.WriteLine("What is the release date of the movie? (dd/mm/yyyy): ");
            try
            {
                releaseDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                correctDate = true;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Wrong date format!");
            }
        }

        description = QuestionLogic.AskString("What is the description of the movie? ");
        director = QuestionLogic.AskString("Who is the director of the movie?: ");
        duration = (int)QuestionLogic.AskNumber("What is the duration? (minutes)");
        bool ispositive = false;

        while(!ispositive)
        {
            price = (int)QuestionLogic.AskNumber("How expensive is the movie?: ");
            if (price < 0)
            Console.WriteLine("please enter a positive number");
            else
            ispositive = true;
        }

        Console.Clear();
        Console.WriteLine("Would you like to turn on ads? (y/n)");
        ads = (Console.ReadKey().KeyChar == 'y') switch
        {
            true => true,
            false => false
        };

        MovieModel movie = MoviesLogic.NewMovie(title, releaseDate, director, description, duration, price, categories, formats);

        movie = MoviesLogic.GetAllMovies().Result.Last();

        if (movie == null) return;

        Category.CategoryMenu(movie);

        Format.ViewFormatMovieMenu(movie);

        Console.Clear();
        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date.ToShortDateString()}");
        Console.WriteLine($"Director: {movie.Director}");
        Console.WriteLine($"Price: {movie.Price}");
        Console.WriteLine($"Categories: {string.Join(", ", movie.Categories.Select(c => c.Name))}");
        Console.WriteLine($"Formats: {string.Join(", ", movie.Formats)}");
        Console.WriteLine($"Ads: {movie.Ads}");

        QuestionLogic.AskEnter();

        Admin.Start();
    }

    public static void ChangeMoviesMenu()
    {
        Console.Clear();
        List<MovieModel> movies = MoviesLogic.AllMovies(true);
        string Question = "What movie would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        foreach (var movie in movies)
        {
            Options.Add(movie.Title);
            Actions.Add(() => ChangeMovieMenu(movie));
        }

        Options.Add("Return");
        Actions.Add(() => Admin.ChangeData());

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
            "Change Ads", "Change Categories",
            "Change Formats"
        };

        List<Action> Actions = new List<Action>() { };
        Actions.Add(() => ChangeTitle(movie));
        Actions.Add(() => ChangeDirector(movie));
        Actions.Add(() => ChangeReleaseDate(movie));
        Actions.Add(() => ChangeDescription(movie));
        Actions.Add(() => ChangeDuration(movie));
        Actions.Add(() => ChangePrice(movie));
        Actions.Add(() => ChangeAds(movie));
        Actions.Add(() => ChangeCategory(movie));
        Actions.Add(() => Format.ChangeFormats(movie, () => ChangeMovieMenu(movie)));

        Options.Add("Return");
        Actions.Add(() => ChangeMoviesMenu());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ChangeCategory(MovieModel movie)
    {
        CategoryLogic CL = new CategoryLogic();
        CL.AddRemoveCategoryMovie(movie);
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
        Console.Clear();
        int NewDuration = (int)QuestionLogic.AskNumber("What do you want to change the duration of this movie to? (please enter the ammount of minutes)");
        MoviesLogic.ChangeDuration(movie, NewDuration);
        Console.WriteLine($"Duration is now: {NewDuration} minutes");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangePrice(MovieModel movie)
    { 
        double NewPrice = 0;
        Console.Clear();
        bool positive = false;
        while (!positive)
        {
            NewPrice = QuestionLogic.AskNumber("What do you want to change the price of this movie to?");
            if (NewPrice < 0)
            Console.WriteLine("Please enter a positive number");
            else
            positive = true;
        }

        MoviesLogic.ChangePrice(movie, NewPrice);
        Console.WriteLine($"Price is now: {NewPrice}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }

    private static void ChangeAds(MovieModel movie)
    {
        Console.Clear();
        if (movie.Ads) Console.WriteLine("Would you like to turn on ads? (y/n)");
        else Console.WriteLine("Would you like to turn off ads? (y/n)");

        if (Console.ReadKey().KeyChar == 'y')
        {
            MoviesLogic.ChangeAds(movie);
        }
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
                NewReleaseDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            m => !pastReviews.Any(r => r.MovieId == TL.GetById(m.TimeSlotId)!.Result.MovieId));

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
                options.Add($"Movie: {MoviesLogic.GetById(TL.GetById(pastReservation.TimeSlotId)!.Result.MovieId)!.Result.Title} Watched on: {pastReservation.DateTime}");
                actions.Add(() => AddNewReview(TL.GetById(pastReservation.TimeSlotId)!.Result.MovieId, pastReservation));
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
    public async static void AddNewReview(int MovieId, ReservationModel pastReservation)
    {
        MoviesLogic ML = new MoviesLogic();
        ReviewLogic RL = new ReviewLogic();

        MovieModel Movie = ML.GetById(MovieId)!.Result;

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
            Console.WriteLine("\nEnter your message (max 155 characters): ");
            message = Console.ReadLine()!;
            message = ReviewLogic.CutReviewMessage(message); // cuts message to size
        }

        await RL.SaveNewReview(message, rating, pastReservation); // saves message to CSV
    }

    public static void UpAndComingReleases()
    {
        MoviesLogic ML = new MoviesLogic();

        Console.Clear();

        string Question = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        List<MovieModel> UpcomingMovies = ML.UnreleasedMovies().FindAll(m => m.Ads == true);

        if (UpcomingMovies.Count == 0)
        {
            Console.WriteLine("You have no upcoming movies");
            QuestionLogic.AskEnter();
            return;
        }

        foreach (MovieModel movie in ML.UnreleasedMovies())
        {
            // Movie title
            Options.Add($"Title: {movie.Title}");
            Actions.Add(() => ML.GetMovieDetails(movie, () => UpAndComingReleases()));

            // Follow or unfollow
            if (AccountsLogic.CurrentAccount != null && !movie.Followers.Any(f => f == AccountsLogic.CurrentAccount.Id))
            {
                Options.Add($"~Follow");
                Actions.Add(() => ML.FollowMovie(movie));
            }
            else if (movie.Followers.Any(f => f == AccountsLogic.CurrentAccount!.Id))
            {
                Options.Add($"~Unfollow");
                Actions.Add(() => ML.UnfollowMovie(movie));
            }
        }

        Options.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);

        UpAndComingReleases();
    }
}

