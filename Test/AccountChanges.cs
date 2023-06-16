using System.IO;
using System;
using System.Collections.Generic;

namespace TestAccount;

[TestClass]
public class AccountChanges
{
    [TestInitialize]
    public void TestLoginUser() // std login
    {
        //Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        AccountsLogic.CurrentAccount = AL.GetByEmail("1033231@hr.nl").Result;
    }

    [TestMethod]
    [DataRow("NieuwTestWW")] //fail
    [DataRow("NieuwTest")] //fail
    [DataRow("Test123")] //fail
    [DataRow("NiewWachtwoord123")] //pass
    public void TestChangePassword(string newPassword)
    {
        //Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        try
        {
            AL.NewPassword(newPassword).Wait();
        }
        catch (AggregateException) { }

        // Check if password set is same as new password
        Assert.AreEqual(AccountsLogic.CurrentAccount.Password, newPassword);
    }

    [TestMethod]
    [DataRow("@.")] //fail
    [DataRow("Test.@nl")] //fail
    [DataRow("Test@mail.com")] //pass
    public void TestChangeEmail(string newEmail)
    {
        //Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        try
        {
            AL.NewEmail(newEmail).Wait();
        }
        catch (AggregateException) { }

        // Check if email set is same as new email
        Assert.AreEqual(AccountsLogic.CurrentAccount.EmailAddress, newEmail);
    }

    [TestMethod]
    public void TestChangeAdvertisements()
    {
        //Access logical functionality
        AccountsLogic AL = new AccountsLogic();

        try
        {
            AL.DeleteUser(AccountsLogic.CurrentAccount.Id).Wait();
        }
        catch (AggregateException) { }

        // Check if account is deleted / not findable
        Assert.IsTrue(AL.GetByEmail("1033231@hr.nl").Result == null);

    }
}