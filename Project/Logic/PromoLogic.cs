using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
public class PromoLogic
{
    private List<PromoModel> _promos;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public PromoLogic()
    {
        _promos = PromoAccess.LoadAll();
    }

    public void UpdateList(PromoModel Promo)
    {
        //Find if there is already an model with the same id
        int index = _promos.FindIndex(s => s.Id == Promo.Id);

        if (index != -1)
        {
            //update existing model
            _promos[index] = Promo;
            Logger.LogDataChange<PromoModel>(Promo.Id, "Updated");
        }
        else
        {
            //add new model
            _promos.Add(Promo);
            Logger.LogDataChange<PromoModel>(Promo.Id, "Added");
        }
        PromoAccess.WriteAll(_promos);
    }

    public PromoModel? GetById(int id)
    {
        return _promos.Find(i => i.Id == id);
    }
    public int GetNewestId()
    {
        try
        {
            return (_promos.OrderByDescending(item => item.Id).First().Id) + 1;
        }
        catch (System.Exception)
        {
            return 1;
        }
    }

    public List<PromoModel> AllPromos()
    {
        return _promos;
    }

    public void NewPromo(string code)
    {

        PromoModel newPromo = new PromoModel(GetNewestId(),code);
        UpdateList(newPromo);
    }
}