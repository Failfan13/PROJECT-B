namespace Test;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMakeTheather()
    {
        TheatherLogic logic = new TheatherLogic();
        logic.MakeTheather(10, 10);
    }
}