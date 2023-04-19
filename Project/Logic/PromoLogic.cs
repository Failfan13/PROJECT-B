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

    public int GetPromoId(string code)
    {
        return _promos.FindIndex(i => i.Code == code) + 1;
    }

    public List<PromoModel> AllPromos()
    {
        return _promos;
    }

    public PromoModel NewPromo(string code)
    {
        PromoModel newPromo = new PromoModel(GetNewestId(), code);
        UpdateList(newPromo);
        return newPromo;
    }

    public void RemovePromo(string code)
    {
        PromoModel PromoModel = GetById(GetPromoId(code));
        int index = _promos.FindIndex(i => i.Code == code);
        _promos.RemoveAt(index);
        PromoAccess.WriteAll(_promos);
    }

    public void TurnPromo(string code)
    {
        int index = _promos.FindIndex(i => i.Code == code);
        _promos[index].Active = !_promos[index].Active;
    }

    public bool FindPromo(string code)
    {
        return _promos.Exists(i => i.Code == code);
    }

    public bool VerifyCode(string code)
    {
        if (!FindPromo(code))
        {
            if (code.Length <= 4 || code.Length > 10)
            {
                return false;
            }

            return true;
        }

        return false;
    }
}