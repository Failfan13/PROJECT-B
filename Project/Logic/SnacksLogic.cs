using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class SnacksLogic
{
    static public bool _addRemove = true;
    public static Dictionary<int, int> CurrentResSnacks = new Dictionary<int, int>();


    public async Task<List<SnackModel>> GetAllSnacks()
    {
        return await DbLogic.GetAll<SnackModel>();
    }
    // pass model to update
    public async Task UpdateList(SnackModel snack)
    {
        await DbLogic.UpdateItem(snack);
    }

    public async Task UpsertList(SnackModel snack)
    {
        await DbLogic.UpsertItem(snack);
    }

    public async Task<SnackModel>? GetById(int id)
    {
        return await DbLogic.GetById<SnackModel>(id);
    }

    public async Task<SnackModel> NewSnack(SnackModel snack)
    {
        await DbLogic.InsertItem<SnackModel>(snack);
        return snack;
    }

    public async Task<SnackModel> NewSnack(string snackName, List<string> size, double price)
    {
        SnackModel snackModel = new SnackModel();
        snackModel = snackModel.NewSnackModel(snackName, size, price);
        await DbLogic.UpsertItem<SnackModel>(snackModel);
        return snackModel;
    }

    public async Task UpdateSnack(SnackModel snack) //Adds or changes category to list of categories
    {
        await UpdateList(snack);
    }

    public void DeleteSnack(int snackInt) // Deletes category from list of categories
    {
        // account exists and is admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            DbLogic.RemoveItemById<SnackModel>(snackInt);
        }
    }

    public void SwapMode()
    {
        _addRemove = !_addRemove;
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