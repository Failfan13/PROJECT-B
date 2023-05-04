public class ReviewLogic
{
    private List<ReviewModel> _reviews;

    public ReviewLogic()
    {
        _reviews = ReviewsAccess.LoadReviews();
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

            movie.Reviews.Amount = reviews.Count;
            movie.Reviews.Stars = reviewStars;
        }
        catch (System.Exception) { }

        return movie;
    }

    public void SaveReview(string message, double reviewScore, ReservationModel pastReservation)
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        ReviewModel review = null!;
        try
        {
            review = new ReviewModel(TL.GetById(pastReservation.TimeSLotId)!.MovieId, AccountsLogic.CurrentAccount!.Id, reviewScore, message);
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

        switch ((specUser, specMovie))
        {
            case (false, false):
                break;
            case (true, false):
                break;
            case (false, true):
                break;
            case (true, true):
                break;
        }
    }

    public void EditReview(List<ReviewModel> reviews)
    {
        AccountsLogic AL = new AccountsLogic();

        string question = "Select the review you would like to edit";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        foreach (ReviewModel review in reviews)
        {
            options.Add(@$"From user {review.AccountId} - {AL.GetById(review.AccountId)!.FullName}, Date: {review.reviewDate}, Review score {review.Rating},
Message: {review.Review}");
            actions.Add(() => EditReview(review));
        }

        options.Add("Return");
        actions.Add(() => Movies.EditReviewsMenu());

        MenuLogic.Question(question, options, actions);

        Movies.EditReviewsMenu();
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

        Console.WriteLine("Enter the new review score");

        if (!double.TryParse(Console.ReadLine()!, out double score))
            review.Rating = score;

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
