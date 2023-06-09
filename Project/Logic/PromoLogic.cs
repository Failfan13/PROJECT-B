using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
public class PromoLogic
{
    public async Task<List<PromoModel>> GetAllPromos()
    {
        return await DbLogic.GetAll<PromoModel>();
    }
    // pass model to update
    public async Task UpdateList(PromoModel promo)
    {
        await DbLogic.UpdateItem(promo);
    }

    public async Task UpsertList(PromoModel promo)
    {
        await DbLogic.UpsertItem(promo);
    }

    public async Task<PromoModel>? GetById(int id)
    {
        return await DbLogic.GetById<PromoModel>(id);
    }

    public async Task UpdateMovie(PromoModel promo) //Adds or changes category to list of categories
    {
        await UpdateList(promo);
    }

    public PromoModel NewPromo(string code)
    {
        code = code.Replace(" ", "");
        PromoModel newPromo = new PromoModel();
        newPromo = newPromo.NewPromoModel(code);
        newPromo = DbLogic.InsertItem<PromoModel>(newPromo).Result;
        return newPromo;
    }

    public double CalcAfterDiscount(double ogPrice, double discount, bool flat)
    {
        if (flat) discount = ogPrice * (discount / 100);
        return ogPrice - discount;
    }

    public async void TurnPromo(int id)
    {
        PromoModel promo = await GetById(id)!;
        promo.Active = !promo.Active;
        await UpdateList(promo);
    }

    public PromoModel GetPromo(string code)
    {
        return GetAllPromos().Result.Find(i => i.Code == code)!;
    }

    public bool GetPromo(string code, out PromoModel promoModel)
    {
        promoModel = GetAllPromos().Result.Find(i => i.Code == code)!;
        if (promoModel == null) return false;
        return true;
    }

    public bool FindPromo(string code)
    {
        return GetAllPromos().Result.Any(i => i.Code == code);
    }

    public bool VerifyCode(string code)
    {
        if (!FindPromo(code) && code.Length > 4 && code.Length <= 10)
        {
            return true;
        }

        return false;
    }

    public List<T> AllConditions<T>(PromoModel promo) where T : PricePromoModel
    {
        try
        {
            // Get possible condition by type
            string condition = (typeof(T)) switch
            {
                var x when x == typeof(PricePromoModel) => "priceDict",
                var x when x == typeof(MoviePromoModel) => "movieDict",
                var x when x == typeof(SnackPromoModel) => "snackDict",
                var x when x == typeof(SeatPromoModel) => "seatDict",
                _ => throw new ArgumentOutOfRangeException()
            };

            if (promo.Condition != null && promo.Condition.ContainsKey(condition))
            {
                // Serialize condition (remove from stream into el)
                JsonElement el = JsonSerializer.SerializeToElement(promo.Condition[condition]);
                // Deserialize unstreamed el of condition
                return JsonSerializer.Deserialize<List<T>>(el)!;
            }
            return new List<T>();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<T>();
        }
    }

    public List<PricePromoModel> AllPrices(PromoModel promo) => AllConditions<PricePromoModel>(promo);
    public List<MoviePromoModel> AllMovies(PromoModel promo) => AllConditions<MoviePromoModel>(promo);
    public List<SnackPromoModel> AllSnacks(PromoModel promo) => AllConditions<SnackPromoModel>(promo);
    public List<SeatPromoModel> AllSeats(PromoModel promo) => AllConditions<SeatPromoModel>(promo);

}