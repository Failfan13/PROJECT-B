using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
public class CategoryLogic
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();
    private List<CategoryModel> _categories;
    public bool _swapCategory = false;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public CategoryLogic()
    {
        _categories = CategoryAccess.LoadAll();
    }

    public void UpdateList(CategoryModel category)
    {
        //Find if there is already an model with the same id
        int index = _categories.FindIndex(s => s.Id == category.Id);

        if (index != -1)
        {
            //update existing model
            _categories[index] = category;
            Logger.LogDataChange<CategoryModel>(category.Id, "Updated");
        }
        else
        {
            //add new model
            _categories.Add(category);
            Logger.LogDataChange<CategoryModel>(category.Id, "Added");
        }
        CategoryAccess.WriteAll(_categories);
    }

    public CategoryModel? GetById(int id)
    {
        return _categories.Find(i => i.Id == id);
    }
    public int GetNewestId()
    {
        return (_categories.OrderByDescending(item => item.Id).First().Id) + 1;
    }
    public CategoryModel NewCategory(string name)
    {
        int NewID = GetNewestId();
        CategoryModel category = new CategoryModel(NewID, name);
        UpdateList(category);
        return category;
    }
    public List<CategoryModel> AllCategories()
    {
        return _categories;
    }

    public void AddCategoryToMovie(MovieModel movie, CategoryModel category)
    {
        movie.Categories.Add(category);
        MoviesLogic.UpdateList(movie);
    }

    public void RemoveCategoryFromMovie(MovieModel movie, CategoryModel category)
    {
        movie.Categories.Remove(category);
        MoviesLogic.UpdateList(movie);
    }

    public void AddCategory(MovieModel movie) // Adds category to movie
    {
        Console.Clear();
        List<CategoryModel> AllCategories = this.AllCategories();
        List<int> catids = new List<int> { };

        foreach (CategoryModel cm in AllCategories)
        {
            Console.WriteLine($"{cm.Id} {cm.Name}");
        }
        int catid = (int)QuestionLogic.AskNumber("What Category do you want to add?\nPlease enter the number");
        CategoryModel category = this.GetById(catid);
        if (!movie.Categories.Contains(category))
        {
            movie.Categories.Add(category);
        }
        MoviesLogic.UpdateList(movie);
        Console.Clear();
        Console.WriteLine("Current movie info:");
        movie.Info();
        QuestionLogic.AskEnter();
        Movies.ChangeMovieMenu(movie);
    }
    public void RemoveCategory(MovieModel movie) // Removes category to movie
    {
        Console.Clear();
        foreach (CategoryModel cm in movie.Categories)
        {
            Console.WriteLine($"{cm.Id} {cm.Name}");
        }
        int catid = (int)QuestionLogic.AskNumber("What Category do you want to remove?");
        movie.Categories.Remove(this.GetById(catid));
        MoviesLogic.UpdateList(movie);
        Console.Clear();
        Console.WriteLine("Current movie info:");
        movie.Info();
        QuestionLogic.AskEnter();
        Movies.ChangeMovieMenu(movie);
    }

    public void CreateNewCategory(CategoryModel category) //Adds or changes category to list of categories
    {
        UpdateList(category);
    }

    public void DeleteCategory(int CategoryInt) // Deletes category from list of categories
    {
        try
        {
            if (AccountsLogic.CurrentAccount.Admin == true)
            {
                _categories.Remove(GetById(CategoryInt));
                CategoryAccess.WriteAll(_categories);
            }
        }
        catch (System.Exception)
        {
            Console.WriteLine("Not Admin");
        }
    }

    public void SwapMode() => _swapCategory = !_swapCategory;
}