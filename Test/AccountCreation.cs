using System.IO;
using System;
using System.Collections.Generic;

namespace TestAccount;

[TestClass]
public class AccountCreation
{
    [TestMethod]
    [DataRow("1033231@hr.nl", "Erik Maan", "Test123Woord", "2002-04-13")] // pass
    [DataRow("1033231@hr.nl", "Erik Maan", "Test123Woord", "2002-04-13")] // fail
    [DataRow("1033231@hr.nl", "Erik Maan", "Test", "2002-04-13")] // fail
    [DataRow("testMail2@hr.nl", "Erik Maan", "Test123Woord", "13-04-2002")] // fail
    [DataRow("", "", "", "")] // fail
    [DataRow("testMail@hr.nl", "Erik Maan", "1", "2002-04-13")] // fail
    [DataRow("test@.nl", "Erik Maan", "Test123Woord", "2002-04-13")] // fail
    public void TestCreatingUser(string email, string fullName, string password, string dateOfBirth)
    {
        // Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        // Call method to create account (awaitable)
        AccountModel account = AL.NewAccount(email, fullName, password, dateOfBirth).Result;

        // Account created is not null
        Assert.IsNotNull(account);
    }

    [TestMethod]
    [DataRow("1033231@hr.nl", "TestWachtWoord")] // fail
    [DataRow("1033231@hr.nl", "Test123Woord")] // pass
    [DataRow("1033231@hr.nl", "Test")] // fail
    public void TestRequestAccount(string email, string password)
    {
        //Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        // Exception check because of logger file path (pass if exception)
        try
        {
            AL.LogIn(email, password).Wait();
        }
        catch (AggregateException)
        {
            Assert.Fail();
            return;
        }

        Assert.IsTrue(AccountsLogic.CurrentAccount.Info());
    }

    [TestMethod]
    [DataRow("1033231@hr.nl", "Erik Maan", "Test123Woord", "2002-04-13")]
    public void TestRequestData(string email, string fullName, string password, string dateOfBirth)
    {
        //Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        // Aggregate exception file directory logger
        try
        {
            AL.LogIn(email, password).Wait();
        }
        catch (AggregateException) { }

        DateTime date = DateTime.Parse(dateOfBirth);

        if (AccountsLogic.CurrentAccount == null) Assert.Fail();
        // Check if data is same as input
        Assert.AreEqual(email, AccountsLogic.CurrentAccount.EmailAddress);
        Assert.AreEqual(fullName, AccountsLogic.CurrentAccount.FirstName + " " + AccountsLogic.CurrentAccount.LastName);
        Assert.AreEqual(password, AccountsLogic.CurrentAccount.Password);
        Assert.AreEqual(date, AccountsLogic.CurrentAccount.DateOfBirth);
    }
}