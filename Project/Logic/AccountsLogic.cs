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
    public static void AddComplaint(AccountModel account)
    {
        Console.Clear();

        if (account.Complaints.Count < 3)
        {
            Console.WriteLine("Please enter your complaint:\n");

            string complaint = Console.ReadLine();
            account.Complaints.Add(complaint);

            AccountsLogic temp = UserLogin.GetAccountsLogicInstance();
            temp.UpdateList(account);

            Console.Clear();
            Console.WriteLine("Your complaint has been sent. We will get back to you as soon as possible\nPress any key to return");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("You have reached maximum amount of tickets you can submit.\nPlease get in touch with our customer service for further assitance\nPress any key to return");
            Console.ReadKey();
            Contact.contact();
        }
    }
    // Method to display menu for admin to manage complaints.
    public static void AdminSelectionComplaints()
    {
        // function so admin can search for an account by email.
        Console.Clear();
        Console.WriteLine("Enter your email address (press space to cancel):");

        string email = "";
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Spacebar)
            {
                Admin.Start();
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (email.Length > 0)
                {
                    email = email.Substring(0, email.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else if (Char.IsLetterOrDigit(key.KeyChar) || key.KeyChar == '.' || key.KeyChar == '@')
            {
                email += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        }

        // search email and if found assign fetch the account.
        AccountModel account = null;
        Console.Clear();
        foreach (AccountModel accounts in AccountsAccess.LoadAll())
        {
            if (accounts.EmailAddress == email)
            {
                account = accounts;
            }
        }
        // if account was found print all complaints else could not be found
        if (account != null)
        {
            if (account.Complaints.Count > 0)
            {

                // creating the menu 
                string Question = $"Select a complaint to modify or delete from user {account.FullName}, {account.EmailAddress}";

                List<string> Options = new List<string>();
                List<Action> Actions = new List<Action>();

                for (int i = 0; i < account.Complaints.Count; i++)
                {
                    Options.Add($"{i + 1}: {account.Complaints[i]}\n");
                    int index = i; // capture the index in a local variable
                    Actions.Add(() => AccountsLogic.choose(account, index)); // pass the index to choose method
                }
                MenuLogic.Question(Question, Options, Actions);
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("This user has submitted no complaints");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Account could not be found");
            Console.ReadKey();
        }
    }

    // in-between method to determine what as been selected in previous screen.
    public static void choose(AccountModel account, int index)
    {
        Console.Clear();
        string Question = "Select what you would like to do\n";
        List<string> Options = new List<string>()
        {
            "Delete","Modify","Return"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => Delete(account, index));

        Actions.Add(() => Modify(account, index));

        Actions.Add(() => AdminSelectionComplaints());
        MenuLogic.Question(Question, Options, Actions);
    }

    // Deletes complaint
    public static void Delete(AccountModel account, int index)
    {
        Console.WriteLine($"The complaint '{account.Complaints[index]}' has been deleted.");
        account.Complaints.RemoveAt(index);
        AccountsLogic temp = UserLogin.GetAccountsLogicInstance();
        temp.UpdateList(account);
        Console.ReadKey();
    }
    // Modify complaint
    public static void Modify(AccountModel account, int index)
    {
        Console.Write($"Enter new complaint (current complaint: {account.Complaints[index]}): ");
        string newComplaint = Console.ReadLine();
        if (!string.IsNullOrEmpty(newComplaint))
        {
            account.Complaints[index] = newComplaint;
            Console.WriteLine("Complaint updated successfully.");
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
        AccountsLogic temp = UserLogin.GetAccountsLogicInstance();
        temp.UpdateList(account);
        Console.ReadKey();
    }
}