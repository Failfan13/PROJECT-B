using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class SnacksLogic
{
    private List<SnackModel> _snacks;
    public List<SnackModel> SelectedSnacks;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public SnacksLogic()
    {
        _snacks = SnackAccess.LoadAll();
    }

    public void UpdateList(SnackModel snack)
    {
        //Find if there is already an model with the same id
        int index = _snacks.FindIndex(s => s.Id == snack.Id);

        if (index != -1)
        {
            //update existing model
            _snacks[index] = snack;
        }
        else
        {
            //add new model
            _snacks.Add(snack);
        }
        SnackAccess.WriteAll(_snacks);

    }

    public SnackModel? GetById(int id)
    {
        return _snacks.Find(i => i.Id == id);
    }
    public int GetNewestId()
    {
        return (_snacks.OrderByDescending(item => item.Id).First().Id) + 1;
    }

    public SnackModel NewMovie(int id, string snackName, List<string> size, double price)
    {
        int NewID = GetNewestId();
        SnackModel snack = new SnackModel(id, snackName, size, price);
        UpdateList(snack);
        return snack;
    }

    public List<SnackModel> AllMovies()
    {
        return _snacks;
    }
}