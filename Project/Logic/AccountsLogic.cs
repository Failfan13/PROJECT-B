using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Globalization;


public class AccountsLogic
{
    private List<AccountModel> _accounts;
    public static AccountModel? CurrentAccount { get; set; }
    private DbLogic DbLogic = new DbLogic();

    // all existing accounts
    public List<AccountModel> GetAllAccounts()
    {
        return DbLogic.GetAll<AccountModel>();
    }
    // pass model to update
    public void UpdateList(AccountModel account)
    {
        DbLogic.UpdateItem(account);
    }

    // get currect account userId
    public static int? UserId()
    {
        return AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.Id : null;
    }

    // gets the account by associated id
    public AccountModel? GetById(int id)
    {
        return DbLogic.GetById<AccountModel>(id);
    }
    // kan verbetert worden door andere GetById te gebruiken

    // Zelfde als hiervoor
    public bool UserById(int id, out AccountModel account)
    {
        var foundAccount = GetById(id);
        if (foundAccount != null)
        {
            account = foundAccount;
            return true;
        }
        account = null!;
        return false;
    }

    // Logs in is exists otherwise sets to null
    public AccountModel LogIn(string email, string password)
    {
        var loginDetails = new Dictionary<string, string>(){
            {"email_address", email},
            {"password", password}
        };
        CurrentAccount = DbLogic.LoginAs<AccountModel>(loginDetails);
        Logger.SystemLog("Logged in");
        return CurrentAccount;
    }

    // checks for the login
    public AccountModel? CheckLogin(string email, string password)
    {
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = LogIn(email, password);
        return CurrentAccount;
    }

    // logout from current account
    public void LogOut()
    {
        CurrentAccount = null;
        Logger.SystemLog("Logged out");
    }

    // creates a new account
    public void NewAccount(string email, string name, string password, string date)
    {
        AccountModel account = new AccountModel();
        account.NewAccountModel(email, password, name, date);
        UpdateList(account);
    }

    // Changes the password
    public void NewPassword(string newpassword)
    {
        if (CurrentAccount == null) return;
        CurrentAccount.Password = newpassword;
        UpdateList(CurrentAccount);
    }

    // menu with accounts
    public int GetAccountIdFromList()
    {
        int ReturnId = -1;
        string Question = "What user do you want to use?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (AccountModel acc in GetAllAccounts())
        {
            Options.Add(acc.FirstName + " " + acc.LastName);
            Actions.Add(() => ReturnId = acc.Id);
        }

        // lists all users and returns there id;
        MenuLogic.Question(Question, Options, Actions);

        return ReturnId;
    }

    public void DeleteUser(int id)
    {
        DbLogic.RemoveItemById<AccountModel>(id);
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
            UpdateList(account);
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

        Actions.Add(() => DeleteComplaint(account, complaintIndex));
        Actions.Add(() => ModifyComplaint(account, complaintIndex));


        Options.Add("Return");
        Actions.Add(() => Menu.Start());
        MenuLogic.Question(Question, Options, Actions);
    }

    // Deletes complaint
    public static void DeleteComplaint(AccountModel account, int ComplaintIndex)
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

        Contact.ViewAllComplaints(account);
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
            string newComplaint = ModifyComplaint(account.Complaints[ComplaintIndex]);
            account.Complaints[ComplaintIndex] = account.Complaints[ComplaintIndex].Split(':').First() + ":" + newComplaint;
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

    public static bool CheckOfAge()
    {
        if (CurrentAccount == null) return false;
        else if (CurrentAccount.DateOfBirth == null) return false;

        string date = CurrentAccount.DateOfBirth;
        DateTime dateOfBirth = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        return DateTime.Today.Year - dateOfBirth.Year >= 18;
    }
}