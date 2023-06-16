public class GuestAdsLogic
{
    public async Task<List<GuestAdModel>> GetAllGuestAccounts()
    {
        return await DbLogic.GetAll<GuestAdModel>();
    }
    // pass model to update
    public async Task UpdateList(GuestAdModel guestAd)
    {
        await DbLogic.UpdateItem(guestAd);
    }

    public async Task UpsertList(GuestAdModel guestAd)
    {
        await DbLogic.UpsertItem(guestAd);
    }

    public async Task<GuestAdModel>? GetById(int id)
    {
        return await DbLogic.GetById<GuestAdModel>(id);
    }

    public async Task UpdateMovie(GuestAdModel guestAd) //Adds or changes category to list of categories
    {
        await UpdateList(guestAd);
    }

    public void NewguestAd(string email)
    {
        GuestAdModel newGuestAd = new GuestAdModel();
        newGuestAd = newGuestAd.NewGuestAdModel(email);
        DbLogic.InsertItem<GuestAdModel>(newGuestAd);
    }
}