public class ReviewLogic
{
    public async Task<List<ReviewModel>> GetAllReviews()
    {
        return await DbLogic.GetAll<ReviewModel>();
    }
    // pass model to update
    public async Task UpdateList(ReviewModel review)
    {
        await DbLogic.UpdateItem(review);
    }

    public async Task UpsertList(ReviewModel review)
    {
        await DbLogic.UpsertItem(review);
    }

    public async Task<ReviewModel>? GetById(int id)
    {
        return await DbLogic.GetById<ReviewModel>(id);
    }

    public async Task<ReviewModel> NewReview(ReviewModel review)
    {
        await DbLogic.InsertItem<ReviewModel>(review);
        return review;
    }

    public async Task<ReviewModel> NewReview(int movieId, int accountId, double rating, string review, DateTime date = default)
    {
        ReviewModel reviewModel = new ReviewModel();
        reviewModel = reviewModel.NewReviewModel(movieId, accountId, rating, review, date);
        await DbLogic.UpsertItem<ReviewModel>(reviewModel);
        return reviewModel;
    }

    public async Task UpdateReview(ReviewModel review) //Adds or changes category to list of categories
    {
        await UpdateList(review);
    }

    public void DeleteReview(int reviewInt) // Deletes category from list of categories
    {
        // account exists and is admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            DbLogic.RemoveItemById<ReviewModel>(reviewInt);
        }
    }

    // update movie reviews
    private async Task UpdateMovieReview(MovieModel movie)
    {
        MoviesLogic ML = new MoviesLogic();
        List<ReviewModel> reviews = GetAllReviews().Result.FindAll(r => r.MovieId == movie.Id);

        try
        {
            double reviewStars = Math.Round(reviews.Average(r => r.Rating), 2);

            movie.Reviews.ReviewAmount = reviews.Count;
            movie.Reviews.ReviewStars = reviewStars;
        }
        catch (System.Exception)
        {
            movie.Reviews.ReviewAmount = reviews.Count;
            movie.Reviews.ReviewStars = 0;
        }

        await ML.UpdateList(movie);
    }

    public static string CutReviewMessage(string reviewMsg)
    {
        if (reviewMsg.Length > 155)
        {
            return reviewMsg.Substring(0, 155);
        }
        return reviewMsg;
    }
    public async Task SaveNewReview(string message, double reviewScore, ReservationModel pastReservation)
    {
        MoviesLogic ML = new MoviesLogic();
        TimeSlotsLogic TL = new TimeSlotsLogic();
        ReviewModel review = null!;

        try
        {
            // Get movie from time slot
            var movie = ML.GetById(TL.GetById(pastReservation.TimeSlotId)!.MovieId)!.Result;
            if (movie == null) throw new Exception("Movie not found");
            // Make new review
            review = new ReviewModel();
            await NewReview(movie.Id, AccountsLogic.CurrentAccount!.Id, reviewScore, message);
            // Update movie review
            await UpdateMovieReview(movie);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void ViewReviews(bool specUser = false, bool specMovie = false)
    {
        List<ReviewModel> reviews = GetAllReviews().Result;
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
        return DbLogic.GetAllById<ReviewModel>(accountId).Result;
    }

    public void ShowAvailableReviews(List<ReviewModel> reviews)
    {
        AccountsLogic AL = new AccountsLogic();

        AccountModel reviewAccount = null!;

        string question = "Select the review you would like to edit";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        foreach (ReviewModel review in reviews)
        {
            reviewAccount = AL.GetById(review.AccountId)!.Result;
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
        string question = "What would you like to edit?";
        List<string> options = new List<string>();
        List<Action> actions = new List<Action>();

        options.Add("Edit message");
        actions.Add(async () => await EditReviewMessage(review));
        options.Add("Edit score");
        actions.Add(async () => await EditReviewScore(review));
        options.Add("Edit movieId");
        actions.Add(async () => await EditReviewMovieId(review));
        options.Add("Remove review");
        actions.Add(async () => await RemoveReview(review));

        options.Add("Return");
        actions.Add(() => Movies.EditReviewsMenu());

        MenuLogic.Question(question, options, actions);

        Movies.EditReviewsMenu();
    }

    public async Task EditReviewMessage(ReviewModel review)
    {
        Console.Clear();

        Console.WriteLine("Enter the new review message (max 155 characters)");

        review.Review = Console.ReadLine()!;

        await UpdateList(review);
    }

    public async Task EditReviewScore(ReviewModel review)
    {
        MoviesLogic ML = new MoviesLogic();
        Console.Clear();

        Console.WriteLine("Enter the new review score (1-5)");
        string newScore = Console.ReadLine()!;

        if (double.TryParse(newScore, out double score) && score >= 1 && score <= 5)
        {
            review.Rating = score; // score more then 1 less then 5
        }

        await UpdateList(review);
        await UpdateMovieReview(ML.GetById(review.MovieId)!.Result);
    }

    public async Task EditReviewMovieId(ReviewModel review)
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

        await UpdateList(review);
        await UpdateMovieReview(ML.GetById(review.MovieId)!.Result);
    }

    public async Task RemoveReview(ReviewModel review)
    {
        MoviesLogic ML = new MoviesLogic();
        try
        {
            await DbLogic.RemoveItemById<ReviewModel>(review.Id);
            await UpdateMovieReview(ML.GetById(review.MovieId)!.Result);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
