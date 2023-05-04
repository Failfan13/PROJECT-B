using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class MoviesLogic : Order<MovieModel>
{

    static private CategoryLogic CategoryLogic = new CategoryLogic();

    private List<MovieModel> _movies;
    private List<ReviewModel> _reviews;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public MoviesLogic()
    {
        _movies = MoviesAccess.LoadAll();
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

    public List<MovieModel> GetByTitle(string name)
    {
        return _movies.Where(i => i.Title.ToLower().Contains(name.ToLower())).ToList();
    }
    public List<MovieModel> GetByPrice(double price, List<MovieModel> movies = null)
    {
        TimeSlotsLogic tsl = new TimeSlotsLogic();
        if (movies == null)
        {
            movies = _movies;
        }

        return movies.Where(i => tsl.GetByMovieId(i.Id).Any(t => t.Theatre.Seats.Min(s => s.Price) + i.Price <= price)).ToList();
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
        int NewID = GetNewestId();
        MovieModel movie = new MovieModel(NewID, title, releaseDate, director, desript, duration, price, categories, formats);
        UpdateList(movie);
        return movie;
    }

    public List<MovieModel> AllMovies()
    {
        return _movies;
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

    public void AddNewReview(int MovieId, ReservationModel pastReservation)
    {
        MovieModel Movie = GetById(MovieId)!;

        if (Movie == null) return;

        Console.WriteLine("Add new review by entering a rating between 1 and 5 (can be specific bv 4.75)");
        string input = Console.ReadLine()!;

        Console.WriteLine("Would you like to add a message to the review (y/n)");
        ConsoleKeyInfo messageInput = Console.ReadKey();

        if (messageInput.Key == ConsoleKey.Y)
        {
            Console.WriteLine("Enter your message");
            string message = Console.ReadLine()!;
            SaveReview(message, pastReservation); // saves message to CSV
        }

        if (double.TryParse(input, out double rating))
        {
            Movie.Reviews.Amount++; // add one rating
            Movie.Reviews.Stars = ((Movie.Reviews.Stars * (Movie.Reviews.Amount - 1)) + rating) / Movie.Reviews.Amount; // average stars
        }

        UpdateList(Movie);
    }

    public void SaveReview(string message, ReservationModel pastReservation)
    {
        // update csv reviews
    }
}