using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class MoviesLogic
{
    public async Task<List<MovieModel>> GetAllMovies()
    {
        return await DbLogic.GetAll<MovieModel>();
    }
    // pass model to update
    public async Task UpdateList(MovieModel movie)
    {
        await DbLogic.UpdateItem(movie);
    }

    public async Task UpsertList(MovieModel movie)
    {
        await DbLogic.UpsertItem(movie);
    }

    public async Task<MovieModel>? GetById(int id)
    {
        return await DbLogic.GetById<MovieModel>(id);
    }

    public async Task<MovieModel> NewMovie(MovieModel movie)
    {
        await DbLogic.InsertItem<MovieModel>(movie);
        return movie;
    }

    public async Task<MovieModel> NewMovie(string title, DateTime releaseDate, string director, string description, int duration, double price)
    {
        MovieModel movie = new MovieModel();
        movie = movie.NewMovieModel(title, releaseDate, director, description, duration, price);
        await DbLogic.UpsertItem<MovieModel>(movie);
        return movie;
    }

    public async Task UpdateMovie(MovieModel movie) //Adds or changes category to list of categories
    {
        await UpdateList(movie);
    }

    public void DeleteMovie(int MovieInt) // Deletes category from list of categories
    {
        // account exists and is admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            DbLogic.RemoveItemById<MovieModel>(MovieInt);
        }
    }

    public MovieModel? FindTitle(string movieName)
    {
        return GetAllMovies().Result.Find(i => i.Title.ToLower() == movieName.ToLower());
    }

    public List<MovieModel> GetByTitle(string name)
    {
        return GetAllMovies().Result.Where(i => i.Title.ToLower().Contains(name.ToLower())).ToList();
    }

    public List<MovieModel> GetByPrice(double price, List<MovieModel> movies = null)
    {
        TimeSlotsLogic tsl = new TimeSlotsLogic();
        TheatreLogic TL = new TheatreLogic();

        if (movies == null)
        {
            movies = GetAllMovies().Result;
        }
        List<MovieModel> corrmovies = new();
        foreach(MovieModel M in movies)
        {
            List<TimeSlotModel> tsms =  tsl.GetTimeslotByMovieId(M.Id);
            foreach (TimeSlotModel t in tsms)
            {
                TheatreModel TH = TL.GetById(t.Theatre.TheatreId).Result;
                if (!corrmovies.Contains(M) && TH.SeatPrices.Basic <= price)
                corrmovies.Add(M);
            }
        }
        return corrmovies;
        // return movies.Where(i => tsl.GetTimeslotByMovieId(i.Id)!.Any(t => t.Theatre.Seats.Min(s => TL.PriceOfSeatType(s.Type, t.Theatre.TheatreId)) + i.Price <= price)).ToList();
    }

    public List<MovieModel> GetByTimeSlots(DateTime date, List<MovieModel> movies = null)
    {
        TimeSlotsLogic tsl = new TimeSlotsLogic();
        if (movies == null)
        {
            movies = GetAllMovies().Result;
        }

        return movies.Where(i => tsl.GetTimeslotByDate(date)!.Any(x => x.MovieId == i.Id)).ToList();
    }

    public List<MovieModel> GetByCategories(List<CategoryModel> categories, List<MovieModel> movies = null)
    {
        if (movies == null)
        {
            movies = GetAllMovies().Result;
        }

        return movies.Where(i => categories.All(x => i.Categories.Any(y => y.Name == x.Name))).ToList();
    }

    public MovieModel NewMovie(string title, DateTime releaseDate, string director, string desript,
        int duration, double price, List<CategoryModel> categories, List<string> formats)
    {
        MovieModel movie = new();
        movie.NewMovieModel(title, releaseDate, director, desript, duration, price);
        movie.Categories = categories;
        movie.Formats = formats;

        NewMovie(movie);
        return movie;
    }

    public List<MovieModel> AllMovies(bool includeUnreleased = false)
    {
        if (includeUnreleased)
        {
            return GetAllMovies().Result;
        }
        return GetAllMovies().Result.FindAll(i => i.ReleaseDate < DateTime.Now);
    }

    public List<MovieModel> UnreleasedMovies()
    {
        return GetAllMovies().Result.FindAll(i => i.ReleaseDate > DateTime.Now);
    }

    public void AddFormat(MovieModel movie, string format)
    {
        if (!movie.Formats.Contains(format))
        {
            movie.Formats.Add(format);
            UpdateList(movie);
        }

    }

    public void RemoveFormat(MovieModel movie, string format)
    {
        if (movie.Formats.Contains(format))
        {
            movie.Formats.Remove(format);
            UpdateList(movie);
        }
    }

    public List<MovieModel> FilterOnCategories(List<int> CatIds)
    {
        List<MovieModel> FilteredList = new List<MovieModel>();
        foreach (MovieModel movie in AllMovies())
        {
            bool add = false;
            foreach (CategoryModel Cat in movie.Categories)
            {
                foreach (int CatId in CatIds)
                {
                    if (Cat.Id == CatId)
                    {
                        add = true;
                    }
                }
            }
            if (add)
            {
                FilteredList.Add(movie);
            }
        }

        return FilteredList;
    }

    public static void FilterOnMovieName(string input)
    {
        Console.ReadKey();
    }

    public void ChangeTitle(MovieModel movie, string NewTitle)
    {
        movie.Title = NewTitle;
        UpdateList(movie);
    }

    public void ChangeDirector(MovieModel movie, string NewDirector)
    {
        movie.Director = NewDirector;
        UpdateList(movie);
    }

    public void ChangeDescription(MovieModel movie, string NewDescription)
    {
        movie.Description = NewDescription;
        UpdateList(movie);
    }

    public void ChangeDuration(MovieModel movie, int NewDuration)
    {
        movie.Duration = NewDuration;
        UpdateList(movie);
    }

    public void ChangePrice(MovieModel movie, double NewPrice)
    {
        movie.Price = NewPrice;
        UpdateList(movie);
    }

    public void ChangeReleaseDate(MovieModel movie, DateTime NewDate)
    {
        movie.ReleaseDate = NewDate;
        UpdateList(movie);
    }

    // this account's reservations after the current date
    public List<ReservationModel> PastMovies()
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        ReservationLogic RL = new ReservationLogic();

        try
        {
            return RL.GetAllReservations().Result.FindAll(r => r.AccountId == AccountsLogic.CurrentAccount!.Id && r.DateTime < DateTime.Now);
        }
        catch (System.Exception)
        {
            return new List<ReservationModel>();
        }
    }

    public void GetMovieDetails(MovieModel movie, Action returnTo)
    {
        Console.Clear();

        movie.Info();

        QuestionLogic.AskEnter();
        returnTo();
    }

    public void FollowMovie(MovieModel movie)
    {
        movie.Followers.Add(AccountsLogic.CurrentAccount!.Id);
        UpdateList(movie);
    }

    public void UnfollowMovie(MovieModel movie)
    {
        movie.Followers.Remove(AccountsLogic.CurrentAccount!.Id);
        UpdateList(movie);
    }

    public void RemoveFollower(MovieModel movie, int AccountId)
    {
        movie.Followers.Remove(AccountId);
        UpdateList(movie);
    }

    public void ChangeAds(MovieModel movie)
    {
        movie.Ads = !movie.Ads;
    }

    public void ShowMovieDetails(MovieModel movie)
    {
        Console.Clear();

        movie.Info();

        Console.Write($"Stars: \t\t{ShowMovieStars(movie)}\n\n");

        QuestionLogic.AskEnter();
    }

    private string ShowMovieStars(MovieModel movie)
    {
        string stars = "";

        // round to nearest whole number
        int currStars = (int)Math.Round(movie.Reviews.ReviewStars);

        for (int i = 0; i < 5; i++)
        {
            if (i < currStars)
            {
                stars += MenuLogic.ColorAndReturnString("★ ");
            }
            else
            {
                stars += MenuLogic.ColorAndReturnString("☆ ");
            }
        }

        return stars;
    }
}