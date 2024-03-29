﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Globalization;


public class AccountsLogic
{
    public static AccountModel? CurrentAccount { get; set; }
    private DbLogic DbLogic = new DbLogic();

    // all existing accounts
    public async Task<List<AccountModel>> GetAllAccounts()
    {
        return await DbLogic.GetAll<AccountModel>();
    }
    // pass model to update
    public async Task UpdateList(AccountModel account)
    {
        if (GetById(account.Id).Result == null)
            await DbLogic.UpsertItem(account);
        else
            await DbLogic.UpdateItem(account);

    }

    // get currect account userId
    public static int? UserId()
    {
        return AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.Id : null;
    }

    // gets the account by associated id
    public async Task<AccountModel>? GetById(int id)
    {
        return await DbLogic.GetById<AccountModel>(id);
    }

    // Logs in is exists otherwise sets to null
    public async Task<AccountModel> LogIn(string email, string password)
    {
        var loginDetails = new Dictionary<string, string>(){
            {"email_address", email},
            {"password", password}
        };

        var account = await DbLogic.LoginAs<AccountModel>(loginDetails);

        if (account != null)
        {
            CurrentAccount = account;
            Logger.SystemLog("Logged in", CurrentAccount.Id);
        }
        else return null!;
        return CurrentAccount;
    }

    // checks for the login
    public async Task<AccountModel>? CheckLogin(string email, string password)
    {
        if (email == null || password == null)
        {
            return null!;
        }
        CurrentAccount = await LogIn(email, password);
        return CurrentAccount;
    }

    // logout from current account
    public void LogOut()
    {
        int id = CurrentAccount.Id;
        CurrentAccount = null;
        Logger.SystemLog("Logged out", id);
    }

    public async Task<AccountModel> NewAccount(string email, string name, string password, string date)
    {
        AccountModel account = new AccountModel();

        DateTime newDate = DateTime.Now;

        if (DateTime.TryParse(date, out newDate))
            return NewAccount(email, name, password, newDate).Result;

        return null!;
    }

    // creates a new account
    public async Task<AccountModel> NewAccount(string email, string name, string password, DateTime date)
    {
        EmailLogic emailLogic = new EmailLogic();
        AccountModel account = new AccountModel();

        bool valPassword = false;
        bool valEmail = false;
        bool preExist = false;

        if (emailLogic.ValidateEmail(email)) valEmail = true;
        if (ValidatePassword(password)) valPassword = true;

        if (DbLogic.GetByEmail<AccountModel>(email).Result != null) preExist = true;

        if (valEmail && valPassword && !preExist)
        {
            account = account.NewAccountModel(email, password, name, date.AddDays(1));
            await DbLogic.InsertItem<AccountModel>(account);
            return account;
        }

        return null!;
    }

    // Changes the password
    public async Task NewPassword(string newpassword)
    {
        if (CurrentAccount == null || !ValidatePassword(newpassword)) return;
        CurrentAccount.Password = newpassword;
        await DbLogic.UpdateItem<AccountModel>(CurrentAccount);
        Logger.LogDataChange<AccountModel>(CurrentAccount.Id, "Changed");
    }

    public bool ValidatePassword(string password)
    {
        if (password.Length < 8) return false;
        if (!password.Any(char.IsUpper)) return false;
        if (!password.Any(char.IsLower)) return false;
        if (!password.Any(char.IsDigit)) return false;
        return true;
    }

    public async Task NewEmail(string newEmail)
    {
        EmailLogic emailLogic = new EmailLogic();

        if (CurrentAccount == null || !emailLogic.ValidateEmail(newEmail)) return;
        CurrentAccount.EmailAddress = newEmail;
        await DbLogic.UpdateItem<AccountModel>(CurrentAccount);
    }


    public int GetAccountIdFromList()
    {
        int ReturnId = -1;
        string Question = "What user do you want to use?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (AccountModel acc in GetAllAccounts().Result)
        {
            Options.Add(acc.FirstName + " " + acc.LastName);
            Actions.Add(() => ReturnId = acc.Id);
        }

        // lists all users and returns there id;
        MenuLogic.Question(Question, Options, Actions);

        return ReturnId;
    }

    public async Task DeleteUser(int id)
    {
        await DbLogic.RemoveItemById<AccountModel>(id);
        Logger.LogDataChange<AccountModel>(id, "Deleted");
    }

    // method so user can sumbit complaint
    public async Task AddComplaint(string type, string complaintMsg)
    {
        if (AccountsLogic.CurrentAccount == null) return;

        AccountModel account = AccountsLogic.CurrentAccount;
        if (account.Complaints == null)
        {
            account.Complaints = new List<string> { };
            account.Complaints.Add(complaintMsg);
            UpdateList(account).Wait();
        }
        else if (account.Complaints.Count < 10)
        {
            account.Complaints.Add(complaintMsg);
            UpdateList(account).Wait();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("You have reached the maximum number of complaints");
            QuestionLogic.AskEnter();
            Contact.contact();
        }
    }

    public async static Task EditComplaint(AccountModel account = null!, int complaintIndex = -1)
    {
        Console.Clear();

        if (account == null)
        {
            Console.WriteLine("Enter user id or email address to edit complaint");
            string inputIdOrMail = Console.ReadLine()!;
            // Get Email
            if (inputIdOrMail.Contains("@")) account = await DbLogic.GetByEmail<AccountModel>(inputIdOrMail);
            // Get Id
            //else if (int.TryParse(inputIdOrMail, out int id)) account = await DbLogic.GetById<AccountModel>(id)!;///////////////////
            // No account found
            else return;

            if (account == null) return; // No account found
        }

        string Question = "Select what you would like to do\n";
        List<string> Options = new List<string>()
        {
            "Delete complaint","Modify complaint"
        };
        List<Action> Actions = new List<Action>();

        Actions.Add(async () => await DeleteComplaint(account, complaintIndex));
        Actions.Add(async () => await ModifyComplaint(account, complaintIndex));


        Options.Add("Return");
        Actions.Add(() => Menu.Start());
        MenuLogic.Question(Question, Options, Actions);
    }

    // Deletes complaint
    public async static Task DeleteComplaint(AccountModel account, int ComplaintIndex)
    {
        AccountsLogic AL = new AccountsLogic();

        if (ComplaintIndex == -1) // ask what complaint to delete
        {
            string Question = "Select complaint to delete\n";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            for (int i = 0; i < account.Complaints.Count; i++)
            {
                Options.Add(account.Complaints[i].ToString());
                Actions.Add(async () => await AccountsLogic.DeleteComplaint(account, i));
            }

            MenuLogic.Question(Question, Options, Actions);
        }
        else // delete complaint
        {
            Console.Clear();
            account.Complaints.RemoveAt(ComplaintIndex);
            await AL.UpdateList(account);
        }

        await Contact.ViewAllComplaints(account);
    }

    // Modify complaint
    public async static Task ModifyComplaint(AccountModel account, int ComplaintIndex = -1)
    {
        AccountsLogic AL = new AccountsLogic();

        if (ComplaintIndex == -1) // ask what complaint to edit
        {
            string Question = "Select complaint to modify\n";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            for (int i = 0; i < account.Complaints.Count; i++)
            {
                Options.Add(account.Complaints[i].ToString());
                Actions.Add(async () => await AccountsLogic.ModifyComplaint(account, i));
            }

            MenuLogic.Question(Question, Options, Actions);
        }
        else // edit complaint
        {
            Console.Clear();
            string newComplaint = ModifyComplaint(account.Complaints[ComplaintIndex]);
            account.Complaints[ComplaintIndex] = account.Complaints[ComplaintIndex].Split(':').First() + ":" + newComplaint;
            await AL.UpdateList(account);
        }
    }

    private static string ModifyComplaint(string complaint)
    {
        Console.Clear();
        Console.WriteLine("Enter the new complaint");
        return Console.ReadLine()!;
    }

    public async Task<AccountModel> GetByEmail(string email)
    {
        return await DbLogic.GetByEmail<AccountModel>(email);
    }

    public static bool CheckOfAge()
    {
        if (CurrentAccount == null) return false;
        else if (CurrentAccount.DateOfBirth == null) return false;
        return DateTime.Today.Year - CurrentAccount.DateOfBirth.Year >= 18;
    }
}