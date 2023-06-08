using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class UnitTest8
{
    [TestMethod]
    public void BlockAndUnblockSeat_ModifiesTheatre()
    {
        // blocking seat
            // Arrange
            var theatre = new TheatreModel(1, 15, 10, 10);
            var theatres = new TheatreLogic();
            var seatToBlock = 0;
            bool init;

            // Act
            theatres.BlockSeat(theatre, seatToBlock);
            
            // Assert
            init = theatre.LayoutSpecs.BlockedSeatIndexes.Contains(seatToBlock);
            // Assert.AreEqual(true, init);
        
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

}