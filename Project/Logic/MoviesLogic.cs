using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class MoviesLogic : Order<MovieModel>
{

    static private CategoryLogic CategoryLogic = new CategoryLogic();

    private List<MovieModel> _movies;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public MoviesLogic()
    {
        _movies = LoaderOpt().Result;
    }

    private async Task<List<MovieModel>> LoaderOpt()
    {
        return await DbLogic.GetAll<MovieModel>();
    }

    public override void UpdateList(MovieModel movie)
    {
        //Find if there is already an model with the same id
        int index = _movies.FindIndex(s => s.Id == movie.Id);

        if (index != -1)
        {
            //update existing model
            _movies[index] = movie;
            Logger.LogDataChange<MovieModel>(movie.Id, "Updated");
        }
        else
        {
            //add new model
            _movies.Add(movie);
            Logger.LogDataChange<MovieModel>(movie.Id, "Added");
        }

        MoviesAccess.WriteAll(_movies);
    }

    public override MovieModel? GetById(int id)
    {
        return _movies.Find(i => i.Id == id);
    }

    public MovieModel? FindTitle(string movieName)
    {
        return _movies.Find(i => i.Title.ToLower() == movieName.ToLower());
    }

    public List<MovieModel> GetByTitle(string name)
    {
        return _movies.Where(i => i.Title.ToLower().Contains(name.ToLower())).ToList();
    }

    public List<MovieModel> GetByPrice(double price, List<MovieModel> movies = null)
    {
        TimeSlotsLogic tsl = new TimeSlotsLogic();
        TheatreLogic TL = new TheatreLogic();

        if (movies == null)
        {
            movies = _movies;
        }

        return movies.Where(i => tsl.GetByMovieId(i.Id).Any(t => t.Theatre.Seats.Min(s => TL.PriceOfSeatType(s.Type, t.Theatre.TheatreId)) + i.Price <= price)).ToList();
    }

    public List<MovieModel> GetByTimeSlots(DateTime date, List<MovieModel> movies = null)
    {
        TimeSlotsLogic tsl = new TimeSlotsLogic();
        if (movies == null)
        {
            movies = _movies;
        }

        return movies.Where(i => tsl.GetByDate(date).Any(x => x.MovieId == i.Id)).ToList();
    }

    public List<MovieModel> GetByCategories(List<CategoryModel> categories, List<MovieModel> movies = null)
    {
        if (movies == null)
        {
            movies = _movies;
        }

        return movies.Where(i => categories.All(x => i.Categories.Any(y => y.Id == x.Id))).ToList();
    }

    public override int GetNewestId()
    {
        return (_movies.OrderByDescending(item => item.Id).First().Id) + 1;
    }

    public MovieModel NewMovie(string title, DateTime releaseDate, string director, string desript,
        int duration, double price, List<CategoryModel> categories, List<string> formats)
    {
        MovieModel movie = new();
        movie.NewMovieModel(title, releaseDate, director, desript, duration, price);
        movie.Categories = categories;
        movie.Formats = formats;
        //movie = movie.NewMovieModel(NewID, title, releaseDate, director, desript, duration, price, categories, formats);
        UpdateList(movie);
        return movie;
    }

    public List<MovieModel> AllMovies(bool includeUnreleased = false)
    {
        if (includeUnreleased)
        {
            return _movies;
        }
        return _movies.FindAll(i => i.ReleaseDate < DateTime.Now);
    }

    public List<MovieModel> UnreleasedMovies()
    {
        return _movies.FindAll(i => i.ReleaseDate > DateTime.Now);
    }

    public static void AddFormat(MovieModel movie, string format)
    {
        if (!movie.Formats.Contains(format))
            movie.Formats.Add(format);
    }

    public static void RemoveFormat(MovieModel movie, string format)
    {
        if (movie.Formats.Contains(format))
            movie.Formats.Remove(format);
    }

    public void RemoveMovie(int MovieInt)
    {
        try
        {
            if (AccountsLogic.CurrentAccount.Admin == true)
            {
                _movies.Remove(GetById(MovieInt));
                MoviesAccess.WriteAll(_movies);
            }
        }
        catch (System.Exception)
        {
            Console.WriteLine("Not Admin");
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
            return RL.Reservations.FindAll(r => r.AccountId == AccountsLogic.CurrentAccount!.Id && r.DateTime < DateTime.Now);
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
}