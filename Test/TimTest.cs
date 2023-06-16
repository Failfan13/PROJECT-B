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

}