using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class SnacksLogic : Order<SnackModel>//IReservational<SnackModel>
{
    private List<SnackModel> _snacks;
    static public bool _addRemove = true;

    public static Dictionary<int, int> CurrentResSnacks = new Dictionary<int, int>();

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
            Logger.LogDataChange<SnackModel>(snack.Id, "Updated");
        }
        else
        {
            //add new model
            _snacks.Add(snack);
            Logger.LogDataChange<SnackModel>(snack.Id, "Added");
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
    public SnackModel NewSnack(string snackName, List<string> size, double price)
    {
        int NewID = GetNewestId();
        SnackModel snack = new SnackModel(NewID, snackName, size, price);
        UpdateList(snack);
        return snack;
    }
    //Get all snacks
    public List<SnackModel> AllSnacks()
    {
        return _snacks;
    }

    public void AddSnack(SnackModel snack)
    {
        if (CurrentResSnacks.ContainsKey(snack.Id))
        {
            CurrentResSnacks[snack.Id]++;
        }
        else
        {
            CurrentResSnacks.Add(snack.Id, 1);
        }
    }

    public void RemoveSnack(SnackModel snack)
    {
        if (CurrentResSnacks.ContainsKey(snack.Id))
        {
            CurrentResSnacks[snack.Id]--;
            if (CurrentResSnacks[snack.Id] <= 0)
            {
                CurrentResSnacks.Remove(snack.Id);
            }
        }
    }

    public void AddToRess(bool IsEdited = false)
    {
        _addRemove = true;

    }

    public void SwapMode()
    {
        _addRemove = !_addRemove;
    }

    // Get and clears currentResSnacks
    public static Dictionary<int, int> GetSelectedSnacks()
    {
        Dictionary<int, int> tempSnacks = new Dictionary<int, int>(CurrentResSnacks);
        CurrentResSnacks.Clear();
        return tempSnacks;
    }


    public double GetTotalPrice(List<SnackModel> snacks)
    {
        double SnackPrice = 0.00;

        foreach (SnackModel snack in snacks)
        {
            SnackPrice += snack.Price;
        }
        return SnackPrice;
    }
}