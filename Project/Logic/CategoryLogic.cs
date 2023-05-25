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

    public async Task<CategoryModel> NewCategory(CategoryModel category)
    {
        await DbLogic.UpsertItem<CategoryModel>(category);
        return category;
    }

    public async Task<CategoryModel> NewCategory(string name)
    {
        CategoryModel category = new CategoryModel();
        category = category.NewCategoryModel(name);
        await DbLogic.UpsertItem<CategoryModel>(category);
        return category;
    }

    public async Task UpdateCategory(CategoryModel category) //Adds or changes category to list of categories
    {
        await UpdateList(category);
    }

    public void DeleteCategory(int CategoryInt) // Deletes category from list of categories
    {
        // account exists and is admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            DbLogic.RemoveItemById<CategoryModel>(CategoryInt);
        }
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

    public void SwapMode() => _swapCategory = !_swapCategory;

    public void AddRemoveCategoryMovie(MovieModel movie)
    {
        bool finishCategory = false;

        Console.Clear();

        string Qeustion = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (!_swapCategory)
        {
            Options.Add("Swap Mode, Currently: Adding category");
        }
        else
        {
            Options.Add("Swap Mode, Currently: Removing category");
        }
        Actions.Add(() => SwapMode());

        if (!_swapCategory)
        {
            foreach (CategoryModel cat in GetAllCategories().Result.Where(cg => !movie.Categories.Any(c => c.Name == cg.Name)))
            {
                Options.Add(cat.Name);
                Actions.Add(() => AddCategoryToMovie(movie, cat));
            }
        }
        else
        {
            foreach (CategoryModel cat in movie.Categories)
            {
                Options.Add(cat.Name);
                Actions.Add(() => RemoveCategoryFromMovie(movie, cat));
            }
        }

        Options.Add($"\nFinish {(_swapCategory ? "Removing" : "Adding")} categories");
        Actions.Add(() => finishCategory = true);

        MenuLogic.Question(Qeustion, Options, Actions);

        if (!finishCategory) AddRemoveCategoryMovie(movie);
    }
}