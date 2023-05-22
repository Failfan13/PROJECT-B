public class ReviewLogic
{
    private List<ReviewModel> _reviews;

    public ReviewLogic()
    {
        _reviews = ReviewsAccess.LoadReviews();
    }

    public static string CutReviewMessage(string reviewMsg)
    {
        if (reviewMsg.Length > 255)
        {
            return reviewMsg.Substring(0, 255);
        }
        return reviewMsg;
    }


    public List<ReviewModel> AllReviews()
    {
        return _reviews;
    }

    private List<ReviewModel> SortByMovieId(List<ReviewModel> reviews)
    {
        return reviews.OrderBy(r => r.MovieId).ToList();
    }

    public void UpdateReviews()
    {
        _reviews = SortByMovieId(_reviews);

        MoviesLogic ML = new MoviesLogic();
        UpdateMovieReviews(ML.AllMovies());

        ReviewsAccess.WriteReviews(_reviews);
    }

    // uses one movie to update all
    public void UpdateMovieReviews(List<MovieModel> movies)
    {
        MoviesLogic ML = new MoviesLogic();

        for (int i = 0; i < movies.Count; i++)
        {
            movies[i] = UpdateMovieReview(movies[i]);
            ML.UpdateList(movies[i]);
        }
    }

    // update one movie
    private MovieModel UpdateMovieReview(MovieModel movie)
    {
        List<ReviewModel> reviews = AllReviews().FindAll(r => r.MovieId == movie.Id);

        try
        {
            double reviewStars = Math.Round(reviews.Average(r => r.Rating), 2);

            movie.Reviews.ReviewAmount = reviews.Count;
            movie.Reviews.ReviewStars = reviewStars;
        }
        catch (System.Exception) { }

        return movie;
    }

    public void SaveNewReview(string message, double reviewScore, ReservationModel pastReservation)
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        ReviewModel review = null!;
        try
        {
            review = new ReviewModel(TL.GetById(pastReservation.TimeSlotId)!.MovieId, AccountsLogic.CurrentAccount!.Id, reviewScore, message);
        }
        catch (System.Exception)
        {
            return;
        }

        _reviews.Add(review);
        UpdateReviews();
    }

    public void ViewReviews(bool specUser = false, bool specMovie = false)
    {
        List<ReviewModel> reviews = AllReviews();
        AccountsLogic AL = new AccountsLogic();
        MoviesLogic ML = new MoviesLogic();

        Console.Clear();

        string question = "";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        switch ((specUser, specMovie))
        {
            case (false, false):
                ShowAvailableReviews(reviews);
                break;
            case (true, false):
                question = "Select the user you would like to edit reviews for";
                foreach (AccountModel user in AL.GetAllAccounts().Result)
                {
                    options.Add(user.FirstName + " " + user.LastName);
                    actions.Add(() => ShowAvailableReviews(reviews.FindAll(r => r.AccountId == user.Id)));
                }
                break;
            case (false, true):
                ShowAvailableMovies(reviews);
                break;
            case (true, true):
                question = "Select the user and movie you would like to edit reviews for";
                foreach (AccountModel user in AL.GetAllAccounts().Result)
                {
                    options.Add(user.FirstName + " " + user.LastName);
                    actions.Add(() => ShowAvailableMovies(reviews.FindAll(r => r.AccountId == user.Id)));
                }
                break;
        }

        MenuLogic.Question(question, options, actions);

        Movies.EditReviewsMenu();
    }

    public List<ReviewModel> UserPastReviews(int accountId)
    {
        return AllReviews().FindAll(r => r.AccountId == accountId);
    }

    public async void ShowAvailableReviews(List<ReviewModel> reviews)
    {
        AccountsLogic AL = new AccountsLogic();

        AccountModel reviewAccount = null!;

        string question = "Select the review you would like to edit";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        foreach (ReviewModel review in reviews)
        {
            reviewAccount = await AL.GetById(review.AccountId)!;
            options.Add(@$"From user: {review.AccountId} - {reviewAccount.FirstName + " " + reviewAccount.LastName}, Date: {review.ReviewDate}, Review score: {review.Rating},
Message: {review.Review}
");
            actions.Add(() => EditReview(review));
        }

        options.Add("Return");
        actions.Add(() => Movies.EditReviewsMenu());

        MenuLogic.Question(question, options, actions);

        Movies.EditReviewsMenu();
    }

    public void ShowAvailableMovies(List<ReviewModel> reviews)
    {
        MoviesLogic ML = new MoviesLogic();

        string question = "Select the movie you would like to edit reviews for";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        foreach (MovieModel movie in ML.AllMovies(true))
        {
            options.Add(movie.Title);
            actions.Add(() => ShowAvailableReviews(reviews.FindAll(r => r.MovieId == movie.Id)));
        }

        MenuLogic.Question(question, options, actions);

        ShowAvailableReviews(reviews);
    }

    public void EditReview(ReviewModel review)
    {
        UpdateReviews();

        string question = "What would you like to edit?";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        int oldReviewIndex = AllReviews().IndexOf(review);

        options.Add("Edit message");
        actions.Add(() => EditReviewMessage(review, oldReviewIndex));
        options.Add("Edit score");
        actions.Add(() => EditReviewScore(review, oldReviewIndex));
        options.Add("Edit movieId");
        actions.Add(() => EditReviewMovieId(review, oldReviewIndex));
        options.Add("Remove review");
        actions.Add(() => RemoveReview(review, oldReviewIndex));

        options.Add("Return");
        actions.Add(() => Movies.EditReviewsMenu());

        MenuLogic.Question(question, options, actions);

        Movies.EditReviewsMenu();
    }

    public void EditReviewMessage(ReviewModel review, int oldReviewIndex)
    {
        Console.Clear();

        Console.WriteLine("Enter the new review message (max 255 characters)");

        review.Review = Console.ReadLine()!;

        _reviews[oldReviewIndex] = review;
        UpdateReviews();
    }

    public void EditReviewScore(ReviewModel review, int oldReviewIndex)
    {
        Console.Clear();

        Console.WriteLine("Enter the new review score (1-5)");

        if (!double.TryParse(Console.ReadLine()!, out double score))
        {
            if (score >= 1 || score <= 5) review.Rating = score; // score more then 1 less then 5
        }

        _reviews[oldReviewIndex] = review;
        UpdateReviews();
    }

    public void EditReviewMovieId(ReviewModel review, int oldReviewIndex)
    {
        MoviesLogic ML = new MoviesLogic();
        int oldMovieId = review.MovieId;

        Console.Clear();

        string question = "Select the movie for the review";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        foreach (MovieModel movie in ML.AllMovies())
        {
            options.Add($"{movie.Id} - {movie.Title}");
            actions.Add(() => oldMovieId = movie.Id);
        }

        MenuLogic.Question(question, options, actions);

        review.MovieId = oldMovieId;

        _reviews[oldReviewIndex] = review;
        UpdateReviews();
    }

    public void RemoveReview(ReviewModel review, int oldReviewIndex)
    {
        _reviews.RemoveAt(oldReviewIndex);
        UpdateReviews();
    }
}
