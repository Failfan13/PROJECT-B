using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{
    private List<AccountModel> _accounts;


    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    public static AccountModel? CurrentAccount { get; private set; }

    public static string UserName()
    {
        return AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.FullName : null;
    }

    public static int? UserId()
    {
        return AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.Id : null;
    }

    public AccountsLogic()
    {
        _accounts = AccountsAccess.LoadAll();
    }


    public void UpdateList(AccountModel acc)
    {
        //Find if there is already an model with the same id
        int index = _accounts.FindIndex(s => s.Id == acc.Id);

        if (index != -1)
        {
            //update existing model
            _accounts[index] = acc;
            Logger.LogDataChange<AccountModel>(acc.Id, "Updated");
        }
        else
        {
            //add new model
            _accounts.Add(acc);
            Logger.LogDataChange<AccountModel>(acc.Id, "Added");
        }
        AccountsAccess.WriteAll(_accounts);

    }

    public AccountModel? GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    public List<AccountModel> GetAllAccounts()
    {
        return _accounts;
    }

    public int GetNewestId()
    {
        return (_accounts.OrderByDescending(item => item.Id).First().Id) + 1;
    }

    public AccountModel? CheckLogin(string email, string password)
    {
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = _accounts.Find(i => i.EmailAddress == email && i.Password == password);
        return CurrentAccount;
    }

    public void LogOut()
    {
        CurrentAccount = null;
        Logger.SystemLog("Logged out");
    }

    public void NewAccount(string email, string name, string password)
    {
        int NewID = GetNewestId();
        AccountModel account = new AccountModel(NewID, email, password, name);
        UpdateList(account);
    }

    public void NewPassword(string newpassword)
    {
        CurrentAccount.Password = newpassword;
        UpdateList(CurrentAccount);
    }

    public int GetAccountIdFromList()
    {
        int ReturnId = -1;
        string Question = "What user do you want to use?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (AccountModel acc in _accounts)
        {
            Options.Add(acc.FullName);
            Actions.Add(() => ReturnId = acc.Id);
        }

        // lists all users and returns there id;
        MenuLogic.Question(Question, Options, Actions);

        return ReturnId;
    }

    public void DeleteUser(int id)
    {
        _accounts.RemoveAll(i => i.Id == id);
        AccountsAccess.WriteAll(_accounts);
        Logger.LogDataChange<AccountModel>(id, "Deleted");
    }
}




