using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class MoviesLogic
{
    private List<MovieModel> _movies;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public MoviesLogic()
    {
        _movies = MoviesAccess.LoadAll();
    }


    public void UpdateList(MovieModel movie)
    {
        //Find if there is already an model with the same id
        int index = _movies.FindIndex(s => s.Id == movie.Id);

        if (index != -1)
        {
            //update existing model
            _movies[index] = movie;
        }
        else
        {
            //add new model
            _movies.Add(movie);
        }
        MoviesAccess.WriteAll(_movies);

    }

    public MovieModel? GetById(int id)
    {
        return _movies.Find(i => i.Id == id);
    }
    public int GetNewestId()
    {
        return (_movies.OrderByDescending(item => item.Id).First().Id) + 1;
    }

    public MovieModel NewMovie(string title, DateTime releaseDate, string director, string desript, int duration)
    {
        int NewID = GetNewestId();
        MovieModel movie = new MovieModel(NewID, title, releaseDate, director, desript, duration);
        UpdateList(movie);
        return movie;
    }

    public List<MovieModel> AllMovies()
    {
        return _movies;
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
}




