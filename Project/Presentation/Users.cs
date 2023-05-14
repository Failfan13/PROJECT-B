public static class User
{
    static private AccountsLogic accounts = new AccountsLogic();


    public static void SelectUser()
    {
        List<AccountModel> allAccounts = accounts.GetAllAccounts();
        string Question = "Which user would you like to view?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        foreach (AccountModel acc in allAccounts)
        {
            Options.Add(acc.FullName);
            Actions.Add(() => Info(acc));
        }

        MenuLogic.Question(Question, Options, Actions);
    }
    public static void Info(int userID)
    {
        AccountModel account = accounts.GetById(userID)!;
        if (account != null) Info(account);
    }

    public static void Info(AccountModel account)
    {
        Console.Clear();
        string Question = $"user Info: \n-------------------------- \nID: {account.Id}\nName: {account.FullName} \nEmail: {account.EmailAddress} \nAdmin: {account.Admin}\n--------------------------\nWhat would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Edit user");
        Actions.Add(() => EditUser(account));

        Options.Add("Delete user");
        Actions.Add(() => DeleteUser(account));

        Options.Add("View Compaints");
        Actions.Add(() => Contact.ViewComplaints(account));

        Options.Add("\nReturn");
        Actions.Add(() => SelectUser());

        MenuLogic.Question(Question, Options, Actions);
    }


    public static void EditUser(AccountModel account)
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Change name");
        Actions.Add(() => ChangeName(account));

        Options.Add("Change Email");
        Actions.Add(() => ChangeEmail(account));

        Options.Add("Change password");
        Actions.Add(() => ChangePass(account));

        Options.Add("\nReturn");
        Actions.Add(() => Info(account));

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ChangeName(AccountModel account)
    {
        string Question = "What is the new name?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        account.FullName = QuestionLogic.AskString(Question);
        AccountsLogic aa = new AccountsLogic();
        aa.UpdateList(account);
    }

    public static void ChangePass(AccountModel account)
    {
        string Question = "What is the new password?";
        string Question2 = "Confirm password";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        string askpass1 = QuestionLogic.AskString(Question);
        if (askpass1.Length > 0)
        {
            string askpass2 = QuestionLogic.AskString(Question2);
            if (askpass1 == askpass2)
            {
                account.Password = askpass1;
            }
            else
            {
                Console.WriteLine("Passwords do not match");
            }
        }
        AccountsLogic aa = new AccountsLogic();
        aa.UpdateList(account);
    }

    public static void ChangeEmail(AccountModel account)
    {
        string Question = "What is the new email?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        account.EmailAddress = QuestionLogic.AskString(Question);
        AccountsLogic aa = new AccountsLogic();
        aa.UpdateList(account);
    }

    public static void DeleteUser(AccountModel account)
    {
        string Question = "Are you sure you want to delete this user?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        string confdelete = QuestionLogic.AskString(Question);
        if (confdelete == "yes")
        {
            AccountsLogic aa = new AccountsLogic();
            aa.DeleteUser(account.Id);
            aa.UpdateList(account);
            SelectUser();
        }
        else
        {
            Info(account);
        }
        MenuLogic.Question(Question, Options, Actions);
    }
}