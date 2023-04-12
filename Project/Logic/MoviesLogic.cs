using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class MoviesLogic : Order<MovieModel>
{

    static private CategoryLogic CategoryLogic = new CategoryLogic();

    private static List<string> _formats = new List<string>() { "imax", "imax 3d", "3d", "rpx" };
    private List<MovieModel> _movies;

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

    public static List<string> AllFormats()
    {
        return _formats;
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
}




