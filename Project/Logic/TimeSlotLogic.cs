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
        }
        else
        {
            //add new model
            _timeslots.Add(ts);
        }
        TimeSlotAccess.WriteAll(_timeslots);

    }

    public TimeSlotModel? GetById(int id)
    {
        return _timeslots.Find(i => i.Id == id);
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


    public void NewTimeSlot(int movieid, DateTime start, TheaterModel theater)
    {
        int NewID = GetNewestId();
        TimeSlotModel timeslot = new TimeSlotModel(NewID, movieid, start, theater);
        UpdateList(timeslot);
    }

    public List<TimeSlotModel> AllTimSlots()
    {
        return _timeslots;
    }

}
