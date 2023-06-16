using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test;

[TestClass]
public class UnitTest10
    {
    [TestMethod]    
    public void TestCreateTimeSlotAndEdit()
    {
        // Create a mock TheatreModel object
        TheatreModel Theatre = new TheatreModel { Id = 1 };

        // Create a new TimeSlotModel object
        TimeSlotModel timeSlot = new TimeSlotModel();

        // Create a new TimeSlotModel using the NewTimeSlotModel method
        int movieId = 123;
        DateTime start = DateTime.Now;
        string format = "Standard";
        TimeSlotModel newTimeSlot = timeSlot.NewTimeSlotModel(movieId, start, Theatre, format);

        // Assert that the created TimeSlotModel has the correct properties
        Assert.AreEqual(movieId, newTimeSlot.MovieId);
        Assert.AreEqual(start, newTimeSlot.Start);
        Assert.AreEqual(format, newTimeSlot.Format);
        Assert.AreEqual(Theatre.Id, newTimeSlot.Theatre.TheatreId);

        // Edit the TimeSlotModel
        int newMovieId = 456;
        DateTime newStart = DateTime.Now.AddDays(1);
        string newFormat = "Premium";

        newTimeSlot.MovieId = newMovieId;
        newTimeSlot.Start = newStart;
        newTimeSlot.Format = newFormat;

        // Assert that the TimeSlotModel has been successfully edited
        Assert.AreEqual(newMovieId, newTimeSlot.MovieId);
        Assert.AreEqual(newStart, newTimeSlot.Start);
        Assert.AreEqual(newFormat, newTimeSlot.Format);
    }
}

