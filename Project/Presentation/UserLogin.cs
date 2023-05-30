using System.Globalization;
static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    // need access for Contact.cs and Accountslogic.cs
    public static AccountsLogic GetAccountsLogicInstance()
    {
        return accountsLogic;
    }

    public static void Start()
    {
        string Question = "";

        if (AccountsLogic.CurrentAccount != null)
        {
            Question = @$"Currently logged in as: {AccountsLogic.CurrentAccount!.FullName} e-mail address: {AccountsLogic.CurrentAccount!.EmailAddress}";
        }

        Question += "What would you like to do?";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>();

        if (AccountsLogic.CurrentAccount == null)
        {
            Options.Add("Login");
            Actions.Add(() => Login());
            Options.Add("Create new account");
            Actions.Add(() => CreateNewUser());
        }
        else
        {
            Options.Add("Change password");
            Actions.Add(() => ChangePassword());

            Options.Add("Change advertisement settings");
            Actions.Add(() => ChangeAdvertation());
        }

        Options.Add("Return to main menu");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void CreateNewUser()
    {
        EmailLogic EmailLogic = new EmailLogic();
        bool CorrectName = false;
        bool CorrectPass = false;
        bool CorrectDate = false;
        string pass = "";
        string Email = "";
        string Name = "";
        string Date = "";

        string subject = "";
        string body = "";

        int newAccountId = -1;

        Console.Clear();
        Email = AskEmail();

        Console.Clear();
        while (!CorrectName)
        {
            Console.WriteLine("Please enter your full name:");
            Name = Console.ReadLine()!;
            if (Name != "")
            {
                CorrectName = true;
            }
            else
            {
                Console.WriteLine("Invalid value");
            }
        }

        Console.Clear();
        while (!CorrectDate)
        {
            Console.WriteLine("Please enter your date of birth (dd/mm/yyyy):");
            string input = Console.ReadLine();
            DateTime dateOfBirth;
            bool isValidDate = DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);

            if (isValidDate)
            {
                Console.WriteLine("Valid date: " + dateOfBirth.ToShortDateString());
                Date = dateOfBirth.ToShortDateString();
                CorrectDate = true;
            }
            else
            {
                Console.WriteLine("Invalid date");
            }

        }

        Console.Clear();

        while (!CorrectPass)
        {
            Console.WriteLine("Please enter your password:");
            string pass1 = Console.ReadLine()!;
            Console.WriteLine("Please enter your password again:");
            string pass2 = Console.ReadLine()!;
            if (pass1 == pass2 && pass1 != "")
            {
                pass = pass1;
                CorrectPass = true;
            }
            else
            {
                Console.WriteLine("Passwords dont match");
            }
        }

        SignUpMails(Email);

        subject = "User account has been created";
        body = @$"Hello {Name},
        
An account with this email address has been created.

If this wasent a mistake, please contact the administrator.

Thank you.";

        EmailLogic.SendEmail(Email, subject, body);

        newAccountId = accountsLogic.NewAccount(Email, Name, pass, Date);

        Login(newAccountId);
    }

    public static void Login(int accountId = -1)
    {
        if (accountsLogic.UserById(accountId, out AccountModel account))
        {
            AccountsLogic.CurrentAccount = account;
            return;
        }

        if (AccountsLogic.CurrentAccount == null)
        {
            Console.Clear();
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine()!;
            Console.WriteLine("Please enter your password");
            string password = Console.ReadLine()!;
            AccountModel acc = accountsLogic.CheckLogin(email, password)!;
            if (acc != null)
            {
                Console.Clear();
                Logger.SystemLog("Logged in");
                Console.WriteLine("Welcome back " + acc.FullName);

                QuestionLogic.AskEnter();
                Menu.Start();
            }
            else
            {
                Console.WriteLine("No account found with that email and password");
                QuestionLogic.AskEnter();
                Menu.Start();
            }
        }
        else
        {
            Menu.Start();
        }
    }

    public static void Logout()
    {
        Console.Clear();
        Console.WriteLine($"You are logged in with: {AccountsLogic.CurrentAccount!.EmailAddress}.\nDo you want to log out? Y/N");
        string awnser = Console.ReadLine()!;
        if (awnser.ToLower() == "y")
        {
            accountsLogic.LogOut();
        }
        else
        {
            Menu.Start();
        }
    }
    public static void ChangePassword()
    {
        Console.WriteLine("Please enter old password");
        string oldpws = Console.ReadLine()!;
        if (accountsLogic.CheckLogin(AccountsLogic.CurrentAccount!.EmailAddress, oldpws) == null)
        {
            Console.WriteLine("Wrong password");
            ChangePassword();
        }
        else
        {
            Console.WriteLine("Please enter new password");
            string newpassword = Console.ReadLine()!;
            accountsLogic.NewPassword(newpassword);
        }
    }

    public static void ChangeAdvertation()
    {
        Console.WriteLine($"Would you like to {(AccountsLogic.CurrentAccount!.AdMails ? "still" : "")} receive ad-mails? (y/n)");
        string adChoice = Console.ReadLine()!;

        if (adChoice.ToLower() == "n") AccountsLogic.CurrentAccount.AdMails = false;

        Console.WriteLine("The ad-mails will be " + (AccountsLogic.CurrentAccount.AdMails ? "enabled" : "disabled"));

        Start();
    }

    public static void SignUpMails(string existingEmail = "")
    {
        EmailLogic EmailLogic = new EmailLogic();
        AccountsLogic AccountsLogic = new AccountsLogic();

        string email = "";
        string subject;
        string body;

        bool corrEmail = false;

        Console.WriteLine("Would you like to sign up for ad-mails? (y/n)");
        var answer = Console.ReadLine();
        if (answer == "y")
        {
            if (AccountsLogic.CurrentAccount == null)
            {
                while (!corrEmail)
                {
                    if (existingEmail == "")
                    {
                        Console.WriteLine("Please enter your email address");
                        email = Console.ReadLine()!;
                    }
                    else
                    {
                        email = existingEmail;
                    }
                    corrEmail = EmailLogic.ValidateEmail(email);
                    Console.Clear();
                    if (corrEmail == false) Console.WriteLine("Invalid email address");
                }
            }
            else
            {
                var account = AccountsLogic.GetById(AccountsLogic.CurrentAccount.Id);
                account.AdMails = true;
                email = account.EmailAddress;
            }

            subject = "Subscribed to ad-mails";
            body = @$"Hello {(AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.FullName : "Guest")},
    
Thank you for subscribing to the cinema ads.

You will receive deals and information about upcomming movies with this subscription.

To unsubscribe from these emails, please log into your account and turn off the add option.";

            EmailLogic.SendEmail(email, subject, body);
        }
        return;
    }
    public static string AskEmail()
    {
        EmailLogic EmailLogic = new EmailLogic();
        bool CorrectEmail = false;
        string Email = "";

        if (AccountsLogic.CurrentAccount == null)
        {
            while (!CorrectEmail)
            {
                Console.WriteLine("Please enter your email address:");
                Email = Console.ReadLine()!;
                CorrectEmail = EmailLogic.ValidateEmail(Email);
                Console.Clear();
                if (CorrectEmail == false) Console.WriteLine("Invalid email address");
            }
            return Email;
        }
        Email = AccountsLogic.CurrentAccount.EmailAddress;
        return Email;
    }
}