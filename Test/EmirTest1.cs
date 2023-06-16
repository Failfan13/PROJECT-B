namespace Test;

[TestClass]
public class UnitTest9
{
    // blocking theather seats
    [TestMethod]
    public void TestBlockAndUnblockSeat_ModifiesTheatre()
    {
        // blocking seat
        // Arrange
        var theatre = new TheatreModel();
        theatre.NewTheatreModel(15, 10, 10);
        var theatres = new TheatreLogic();
        var seatToBlock = 0;
        bool init;

        // Act
        theatres.BlockSeat(theatre, seatToBlock);

        // Assert
        init = theatre.LayoutSpecs.BlockedSeatIndexes.Contains(seatToBlock);
        Assert.AreEqual(true, init);

        // unblocking seat
        // Arrange
        var seatToUnblock = 0;
        bool notinit;

        // Act
        theatres.BlockSeat(theatre, seatToBlock);

        // Assert
        notinit = theatre.LayoutSpecs.BlockedSeatIndexes.Contains(seatToBlock);
        Assert.AreEqual(false, notinit);
    }


    // modifying seat prices
    [TestMethod]
    public void ModifyLuxurySeatPrice_ShouldUpdateLuxurySeatPrice()
    {
        // Arrange
        var theatre = new TheatreModel();
        double newLuxurySeatPrice = 25.0;

        // Act
        theatre.NewTheatreModel(10.0, 100, 100); // Create a new theatre model with initial seat prices
        theatre.SeatPrices.Luxury = newLuxurySeatPrice;

        // Assert
        Assert.AreEqual(newLuxurySeatPrice, theatre.SeatPrices.Luxury);
    }
}