using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class SnacksLogic : Order<SnackModel>//IReservational<SnackModel>
{
    private List<SnackModel> _snacks;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    //Load all snacks from database
    public SnacksLogic()
    {
        _snacks = SnackAccess.LoadAll();
    }

    //Write 
    public override void UpdateList(SnackModel snack)
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

    //Find if for id in loaded json
    public override SnackModel? GetById(int id)
    {
        return _snacks.Find(i => i.Id == id);
    }
    //Receive most recent id
    public override int GetNewestId()
    {
        return (_snacks.OrderByDescending(item => item.Id).First().Id) + 1;
    }
    //Add new to database
    public SnackModel NewSnack(int id, string snackName, List<string> size, double price)
    {
        int NewID = GetNewestId();
        SnackModel snack = new SnackModel(id, snackName, size, price);
        UpdateList(snack);
        return snack;
    }
    //Get all snacks
    public List<SnackModel> AllSnacks()
    {
        return _snacks;
    }
}