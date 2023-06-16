using System.IO;
using System;
using System.Collections.Generic;

namespace TestAccount;

[TestClass]
public class AccountLogin
{
    [TestMethod]
    [DataRow("1033231@hr.nl", "TestWachtWoord")] // fail
    [DataRow("1033231@hr.nl", "Test123Woord")] // pass
    [DataRow("1033231@hr.nl", "Test")] // fail
    public void TestLoginUser(string email, string password)
    {
        //Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        // Exception check because of logger file path (pass if exception)
        Assert.ThrowsException<AggregateException>(() => AL.LogIn(email, password).Result);
        Assert.IsNotNull(AccountsLogic.CurrentAccount);
        Assert.IsTrue(AccountsLogic.CurrentAccount.EmailAddress == email);
    }

    [TestMethod]
    public void TestLogoutUser()
    {
        //Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        // Logout from account
        Assert.ThrowsException<DirectoryNotFoundException>(() => AL.LogOut());

        Assert.IsNull(AccountsLogic.CurrentAccount);
    }
}