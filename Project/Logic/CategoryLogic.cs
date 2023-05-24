using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
public class CategoryLogic
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();
    public bool _swapCategory = false;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public async Task<List<CategoryModel>> GetAllCategories()
    {
        return await DbLogic.GetAll<CategoryModel>();
    }
    // pass model to update
    public async Task UpdateList(CategoryModel account)
    {
        await DbLogic.UpsertItem(account);
    }

    // get currect account userId
    public static int? UserId()
    {
        return AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.Id : null;
    }

    // gets the account by associated id
    public async Task<CategoryModel>? GetById(int id)
    {
        return await DbLogic.GetById<CategoryModel>(id);
    }
    public async Task<CategoryModel> NewCategory(string name)
    {
        CategoryModel category = new CategoryModel();
        category = category.NewCategoryModel(name);
        await DbLogic.UpsertItem<CategoryModel>(category);
        return category;
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

    public async Task AddCategoryToMovie(MovieModel movie) // Adds category to movie
    {
        Console.Clear();
        List<CategoryModel> AllCategories = await GetAllCategories();
        List<int> catids = new List<int> { };

        foreach (CategoryModel cm in AllCategories)
        {
            Console.WriteLine($"{cm.Id} {cm.Name}");
        }
        int catid = (int)QuestionLogic.AskNumber("What Category do you want to add?\nPlease enter the number");
        CategoryModel category = await GetById(catid)!;
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
    public async Task RemoveCategory(MovieModel movie) // Removes category to movie
    {
        Console.Clear();
        foreach (CategoryModel cm in movie.Categories)
        {
            Console.WriteLine($"{cm.Id} {cm.Name}");
        }
        int catid = (int)QuestionLogic.AskNumber("What Category do you want to remove?");
        movie.Categories.Remove(await GetById(catid)!);
        MoviesLogic.UpdateList(movie);
        Console.Clear();
        Console.WriteLine("Current movie info:");
        movie.Info();
        QuestionLogic.AskEnter();
        Movies.ChangeMovieMenu(movie);
    }

    public async Task CreateNewCategory(CategoryModel category) //Adds or changes category to list of categories
    {
        await UpdateList(category);
    }

    public async Task DeleteCategory(int CategoryInt) // Deletes category from list of categories
    {
        try
        {
            if (AccountsLogic.CurrentAccount.Admin == true)
            {
                await DbLogic.RemoveItemById<CategoryModel>(CategoryInt);
            }
        }
        catch (System.Exception)
        {
            Console.WriteLine("Not Admin");
        }
    }

    public void SwapMode() => _swapCategory = !_swapCategory;
}