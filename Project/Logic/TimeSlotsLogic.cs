using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class TimeSlotsLogic
{
    private List<TimeSlotModel> _timeslots;


    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public TimeSlotModel? TimeSlot { get; private set; }

    public TimeSlotsLogic()
    {
        _timeslots = TimeSlotAccess.LoadAll();
    }

    public void UpdateList(TimeSlotModel ts)
    {
        //Find if there is already an model with the same id
        int index = _timeslots.FindIndex(s => s.Id == ts.Id);

        if (index != -1)
        {
            //update existing model
            _timeslots[index] = ts;
            Logger.LogDataChange<TimeSlotModel>(ts.Id, "Updated");
        }
        else
        {
            //add new model
            _timeslots.Add(ts);
            Logger.LogDataChange<TimeSlotModel>(ts.Id, "Added");
        }
        TimeSlotAccess.WriteAll(_timeslots);

    }

    public TimeSlotModel? GetById(int id)
    {
        return _timeslots.Find(i => i.Id == id);
    }

    public List<TimeSlotModel>? GetByTime(DateTime dateTime)
    {
        return _timeslots.FindAll(i => i.Start >= dateTime);
    }

    public List<TimeSlotModel>? GetByDate(DateTime date)
    {
        return _timeslots.FindAll(i => i.Start.Date == date.Date);
    }

    public List<TimeSlotModel>? GetByMovieId(int movieid)
    {
        List<TimeSlotModel> tsmlist = new List<TimeSlotModel>();
        foreach (TimeSlotModel tsm in _timeslots)
        {
            if (tsm.MovieId == movieid)
            {
                tsmlist.Add(tsm);
            }
        }
        return tsmlist;
    }

    public int GetNewestId()
    {
        return (_timeslots.OrderByDescending(item => item.Id).First().Id) + 1;
    }

    public void NewTimeSlot(int movieid, DateTime start, TheatreModel theatre, string format)
    {
        int NewID = GetNewestId();
        TimeSlotModel timeslot = new TimeSlotModel(NewID, movieid, start, theatre, format);
    }
    public void NewTimeSlot(int movieid, DateTime start)
    {
        int NewID = GetNewestId();
        TheatreLogic TL = new TheatreLogic();

        // create new theatre menu & return new theatre ID
        int newTheatreId = Theatre.MakeNewTheatre();

        try
        {
            TimeSlotModel timeslot = new TimeSlotModel(NewID, movieid, start, TL.GetById(newTheatreId)!.Result!, "");
            UpdateList(timeslot);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            Thread.Sleep(5000);
        }
    }

    public List<TimeSlotModel> AllTimeSlots()
    {
        return _timeslots;
    }

    public void AddFormat(TimeSlotModel formatModel, string format)
    {
        formatModel.Format = format;
        UpdateList(formatModel);
    }

    public void RemoveFormat(TimeSlotModel formatModel)
    {
        formatModel.Format = "";
        UpdateList(formatModel);
    }
}
