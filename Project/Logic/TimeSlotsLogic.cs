using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class TimeSlotsLogic
{
    public async Task<List<TimeSlotModel>> GetAllTimeSlots()
    {
        return await DbLogic.GetAll<TimeSlotModel>();
    }
    // pass model to update
    public async Task UpdateList(TimeSlotModel timeSlot)
    {
        await DbLogic.UpdateItem(timeSlot);
    }

    public async Task UpsertList(TimeSlotModel timeSlot)
    {
        await DbLogic.UpsertItem(timeSlot);
    }

    public async Task<TimeSlotModel>? GetById(int id)
    {
        return await DbLogic.GetById<TimeSlotModel>(id);
    }

    public async Task<TimeSlotModel> NewTimeSlot(TimeSlotModel timeSlot)
    {
        await DbLogic.InsertItem<TimeSlotModel>(timeSlot);
        return timeSlot;
    }

    public async Task<TimeSlotModel> NewTimeSlot(int movieid, DateTime start, TheatreModel theatre, string format)
    {
        TimeSlotModel timeSlotModel = new TimeSlotModel();
        timeSlotModel = timeSlotModel.NewTimeSlotModel();
        await DbLogic.UpsertItem<TimeSlotModel>(timeSlotModel);
        return timeSlotModel;
    }

    public async Task UpdateTimeSlot(TimeSlotModel TimeSlot) //Adds or changes category to list of categories
    {
        await UpdateList(TimeSlot);
    }

    public void DeleteTimeSlot(int TimeSlotInt) // Deletes category from list of categories
    {
        // account exists and is admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            DbLogic.RemoveItemById<TimeSlotModel>(TimeSlotInt);
        }
    }

    public async void AddFormat(TimeSlotModel formatModel, string format)
    {
        formatModel.Format = format;
        await UpdateList(formatModel);
    }

    public async Task RemoveFormat(TimeSlotModel formatModel)
    {
        formatModel.Format = "standard";
        await UpdateList(formatModel);
    }

    public List<TimeSlotModel>? GetTimeslotByDate(DateTime date)
    {
        return GetAllTimeSlots().Result.FindAll(i => i.Start.Date == date.Date);
    }

    public List<TimeSlotModel>? GetTimeslotByMovieId(int movieid)
    {
        return GetAllTimeSlots().Result.FindAll(i => i.MovieId == movieid);
    }

    // public void ChangeMaxSeats(TimeSlotModel m, int i)
    // {
    //     m.MaxSeats = i;
    //     UpdateList(m);
    // }
}
