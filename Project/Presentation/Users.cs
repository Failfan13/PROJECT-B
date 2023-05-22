public static class User
{
    static private AccountsLogic accounts = new AccountsLogic();


    public async static void SelectUser()
    {
        List<AccountModel> allAccounts = await accounts.GetAllAccounts();
        string Question = "Which user would you like to view?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        foreach (AccountModel acc in allAccounts)
        {
            Options.Add(acc.FirstName + " " + acc.LastName);
            Actions.Add(() => Info(acc));
        }
        Options.Add("return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }
    public async static void Info(int userID)
    {
        AccountModel account = await accounts.GetById(userID)!;
        if (account != null) Info(account);
    }

    public static void Info(AccountModel account)
    {
        Console.Clear();
        string Question = $"user Info: \n-------------------------- \nID: {account.Id}\nName: {account.FirstName + " " + account.LastName} \nEmail: {account.EmailAddress} \nAdmin: {account.Admin}\n--------------------------\nWhat would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Edit user");
        Actions.Add(() => EditUser(account));

        Options.Add("Delete user");
        Actions.Add(async () => await DeleteUser(account));

        Options.Add("View Compaints");
        Actions.Add(async () => await Contact.ViewComplaints(account));

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
        Actions.Add(async () => await ChangeName(account));

        Options.Add("Change Email");
        Actions.Add(async () => await ChangeEmail(account));

        Options.Add("Change password");
        Actions.Add(async () => await ChangePass(account));

        Options.Add("\nReturn");
        Actions.Add(() => Info(account));

        MenuLogic.Question(Question, Options, Actions);
    }

    public async static Task ChangeName(AccountModel account)
    {
        string Question = "What is the new name?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        account.FirstName = QuestionLogic.AskString(Question);
        AccountsLogic aa = new AccountsLogic();
        await aa.UpdateList(account);
    }

    public async static Task ChangePass(AccountModel account)
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
        await aa.UpdateList(account);
    }

    public async static Task ChangeEmail(AccountModel account)
    {
        string Question = "What is the new email?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        account.EmailAddress = QuestionLogic.AskString(Question);
        AccountsLogic aa = new AccountsLogic();
        await aa.UpdateList(account);
    }

    public static void DeleteUser()
    {
        if (AccountsLogic.CurrentAccount == null) return;

        var userWrite = AccountsLogic.CurrentAccount.DateOfBirth.Date.ToShortDateString();
        Console.Clear();
        Console.WriteLine($"To delete the account please write: {userWrite}");
        var answer = Console.ReadLine();

        if (answer == userWrite.ToString())
        {
            DbLogic.RemoveItem<AccountModel>(AccountsLogic.CurrentAccount);
            AccountsLogic.CurrentAccount = null;
            Menu.Start();
        }
    }

    public async static Task DeleteUser(AccountModel account)
    {
        AccountsLogic AL = new AccountsLogic();

        string Question = "Are you sure you want to delete this user?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        string confdelete = QuestionLogic.AskString(Question);
        if (confdelete == "yes")
        {
            await AL.DeleteUser(account.Id);
            SelectUser();
        }
        else
        {
            Info(account);
        }
        MenuLogic.Question(Question, Options, Actions);
    }
}