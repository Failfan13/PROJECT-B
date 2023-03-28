using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class MoviesLogic
{
    
    static private CategoryLogic CategoryLogic = new CategoryLogic();
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

    public MovieModel NewMovie(string title, DateTime releaseDate, string director, string desript, int duration, List<CategoryModel> categories)
    {
        int NewID = GetNewestId();
        MovieModel movie = new MovieModel(NewID, title, releaseDate, director, desript, duration, categories);
        UpdateList(movie);
        return movie;
    }

    public List<MovieModel> AllMovies()
    {
        return _movies;
    }
    public void AddCategory(MovieModel movie)
    {
        Console.Clear();
        List<CategoryModel> AllCategories = CategoryLogic.AllCategories();
        List<int> catids = new{};

        foreach(CategoryModel cm in AllCategories)
        {
            Console.WriteLine($"{cm.Id} {cm.Name}");
        }
        int catid = QuestionLogic.AskNumber("What Category do you want to add?\nPlease enter the number");
        CategoryModel category = CategoryLogic.GetById(catid);
        if (!movie.Categories.Contains(category))
        {
            movie.Categories.Add(category);
        }
        UpdateList(movie);
        Console.Clear();
        Console.WriteLine("Current movie info:");
        movie.Info();
        Console.WriteLine("Press enter to continue");
        Console.ReadLine();
        Menu.Start();
    }
    public void RemoveCategory(MovieModel movie)
    {
        Console.Clear();
        foreach(CategoryModel cm in movie.Categories)
        {
            Console.WriteLine($"{cm.Id} {cm.Name}");
        }
        int catid = QuestionLogic.AskNumber("What Category do you want to remove?");
        movie.Categories.Remove(CategoryLogic.GetById(catid));
        UpdateList(movie);
        Console.Clear();
        Console.WriteLine("Current movie info:");
        movie.Info();
        Console.WriteLine("Press enter to continue");
        Console.ReadLine();
        Menu.Start();
    }
}




