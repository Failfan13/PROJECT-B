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
    //public static AccountModel? CurrentAccount { get; private set; }
    public static AccountModel? CurrentAccount { get; set; }

    public static string UserName()
    {
        return AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.FullName : null!;
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
        if (_accounts.Count == 0)
        {
            return 1;
        }
        else
        {
            return _accounts.Max(a => a.Id) + 1;
        }
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
        if (CurrentAccount == null) return;
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

    // method so user can sumbit complaint
    public void AddComplaint(string type, string complaintMsg)
    {
        if (AccountsLogic.CurrentAccount == null) return;

        AccountModel account = AccountsLogic.CurrentAccount;

        if (account.Complaints.Count < 10)
        {
            account.Complaints.Add(complaintMsg);
        }
        else
        {
            Console.Clear();
            Console.WriteLine("You have reached the maximum number of complaints");
            QuestionLogic.AskEnter();
            Contact.contact();
        }
    }

    public static void EditComplaint(AccountModel account = null!, int complaintIndex = -1)
    {
        AccountsLogic AL = new AccountsLogic();
        Console.Clear();

        if (account == null)
        {
            Console.WriteLine("Enter user id or email address to edit complaint");
            string inputIdOrMail = Console.ReadLine()!;
            // Get Email
            if (inputIdOrMail.Contains("@")) account = AL.GetByEmail(inputIdOrMail);
            // Get Id
            else if (int.TryParse(inputIdOrMail, out int id)) account = AL.GetById(id)!;
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
        if (complaintIndex == -1)
        {
            Actions.Add(() => DeleteComplaint(account));
            Actions.Add(() => ModifyComplaint(account));
        }
        else
        {
            Actions.Add(() => DeleteComplaint(account, complaintIndex));
            Actions.Add(() => ModifyComplaint(account, complaintIndex));
        }


        Options.Add("Return");
        Actions.Add(() => Menu.Start());
        MenuLogic.Question(Question, Options, Actions);
    }

    // Deletes complaint
    public static void DeleteComplaint(AccountModel account, int ComplaintIndex = -1)
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
                Actions.Add(() => AccountsLogic.DeleteComplaint(account, i));
            }

            MenuLogic.Question(Question, Options, Actions);
        }
        else // delete complaint
        {
            Console.Clear();
            account.Complaints.RemoveAt(ComplaintIndex);
            AL.UpdateList(account);
        }
    }

    // Modify complaint
    public static void ModifyComplaint(AccountModel account, int ComplaintIndex = -1)
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
                Actions.Add(() => AccountsLogic.ModifyComplaint(account, i));
            }

            MenuLogic.Question(Question, Options, Actions);
        }
        else // edit complaint
        {
            Console.Clear();
            account.Complaints[ComplaintIndex] = ModifyComplaint(account.Complaints[ComplaintIndex]);
            AL.UpdateList(account);
        }
    }

    private static string ModifyComplaint(string complaint)
    {
        Console.Clear();
        Console.WriteLine("Enter the new complaint");
        return Console.ReadLine()!;
    }

    public AccountModel GetByEmail(string email)
    {
        return _accounts.Find(i => i.EmailAddress == email)!;
    }
}