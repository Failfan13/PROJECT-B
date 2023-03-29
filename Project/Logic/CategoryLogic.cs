using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
public class CategoryLogic
{
    private List<CategoryModel> _categories;

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
        }
        else
        {
            //add new model
            _categories.Add(category);
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
}