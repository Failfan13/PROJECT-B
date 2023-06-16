namespace Test;

[TestClass]
public class UnitTestTim
{
    [TestMethod]
    public void TestEditMaxSeats()
    {
        TimeSlotsLogic TL = new();
        
        TimeSlotModel t = TL.GetById(18).Result;
        TL.ChangeMaxSeats(t, 6);
        TimeSlotModel t2 = TL.GetById(18).Result;
        int actual = t2.MaxSeats;
        int expected = 6;
        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    [DataRow("test1", 1)]
    [DataRow("test0", 0)]
    [DataRow("test-1", -1)] 
    public void AddSnacksMinvalue(string name, double price)
    {
        SnacksLogic SL = new();

        SnackModel snack = new();
        snack.Name = name;
        snack.Price = price;
        SnackModel Sn = SL.NewSnack(snack).Result;
        double actual = Sn.Price;
        Assert.AreEqual(actual,1);
    }

    [TestMethod]
    [DataRow(19, 0, "basic", 5, 0, -1, null)]
    [DataRow(18, -1, "basic", 99, 9999, 1, null)]
    [DataRow(99, 10, "basic", 1, -1, 99, null)]
    [DataRow(-1, 10, "basic", 0, 1, 25, null)]
    public void MakeReservationTest(int timeSlotId, int seatid, string seattype, int snackId, int snackammount, int accountId, string format)
    {
        ReservationLogic RL = new();
        ReservationModel ress = new ReservationModel();

        List<SeatModel> seats = new();
        SeatModel seat = new SeatModel(seatid,seattype);
        seats.Add(seat);
        Dictionary<int, int> snacks = new();
        snacks.Add(snackId, snackammount);
        ress = ress.NewReservationModel(timeSlotId, seats, snacks, accountId, DateTime.Now, format);
        Assert.IsNotNull(ress);   
    }

}