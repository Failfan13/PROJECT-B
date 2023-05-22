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
            Question = @$"Currently logged in as: {AccountsLogic.CurrentAccount!.FirstName + " " + AccountsLogic.CurrentAccount!.LastName} 
e-mail address: {AccountsLogic.CurrentAccount!.EmailAddress}
";
        }

        Question += "What would you like to do?";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>();

        if (AccountsLogic.CurrentAccount == null)
        {
            Options.Add("Login");
            Actions.Add(() => Login());
            Options.Add("Create new account");
            Actions.Add(async () => await CreateNewUser());
        }
        else
        {
            Options.Add("Change password");
            Actions.Add(async () => await ChangePassword());

            Options.Add("Change advertisement settings");
            Actions.Add(async () => await ChangeAdvertation());

            Options.Add("Delete account");
            Actions.Add(() => User.DeleteUser());
        }

        Options.Add("Return to main menu");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);

        Start();
    }

    public async static Task CreateNewUser()
    {
        EmailLogic EmailLogic = new EmailLogic();
        AccountsLogic AccountsLogic = new AccountsLogic();
        bool CorrectName = false;
        bool CorrectPass = false;
        bool CorrectDate = false;
        string pass = "";
        string Email = "";
        string Name = "";
        DateTime Date = DateTime.Now;

        string subject = "";
        string body = "";

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

            if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime dateOfBirth))
            {
                Console.WriteLine("Valid date: " + dateOfBirth.ToShortDateString());
                Date = dateOfBirth;
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

        var acc = await AccountsLogic.NewAccount(Email, Name, pass, Date);

        AccountsLogic.LogIn(Email, pass).Wait();

        Menu.Start();
    }

    public static void Login() // Without .Result not working
    {
        if (AccountsLogic.CurrentAccount == null)
        {
            Console.Clear();
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine()!;
            Console.WriteLine("Please enter your password");
            string password = Console.ReadLine()!;
            AccountModel acc = Task.Run(() => accountsLogic.CheckLogin(email, password)).Result;
            if (acc != null)
            {
                Console.Clear();
                Logger.SystemLog("Logged in");
                Menu.Start();
            }
            else
            {
                Console.WriteLine("No account found with that email and password");
                QuestionLogic.AskEnter();
                Menu.Start();
            }
        }
        Menu.Start();
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

    public async static Task ChangePassword()
    {
        Console.WriteLine("Please enter old password");
        string oldpws = Console.ReadLine()!;
        if (AccountsLogic.CurrentAccount!.Password != oldpws)
        {
            Console.WriteLine("Wrong password");
            await ChangePassword();
        }
        else
        {
            Console.WriteLine("Please enter new password");
            string newpassword = Console.ReadLine()!;
            await accountsLogic.NewPassword(newpassword);
        }
    }

    public async static Task ChangeAdvertation()
    {
        Console.WriteLine($"Would you like to{(AccountsLogic.CurrentAccount!.AdMails ? " still " : " ")}receive ad-mails? (y/n)");
        string adChoice = Console.ReadLine()!.ToLower();

        if (adChoice == "y")
        {
            AccountsLogic.CurrentAccount.AdMails = true;
            Console.WriteLine("The ad-mails" + (AccountsLogic.CurrentAccount.AdMails ? " will be " : " are already ") + "enabled");
        }
        else if (adChoice == "n")
        {
            AccountsLogic.CurrentAccount.AdMails = false;
            Console.WriteLine("The ad-mails" + (AccountsLogic.CurrentAccount.AdMails ? " will be " : " are already ") + "disabled");
        }

        try
        {
            await DbLogic.UpdateItem<AccountModel>(AccountsLogic.CurrentAccount);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async static void SignUpMails(string existingEmail = "")
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
                AccountsLogic.CurrentAccount.AdMails = true;
                await DbLogic.UpdateItem<AccountModel>(AccountsLogic.CurrentAccount);
                email = AccountsLogic.CurrentAccount.EmailAddress;
            }

            subject = "Subscribed to ad-mails";
            body = @$"Hello {(AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.FirstName : "Guest")},
    
Thank you for subscribing to the cinema ads.

You will receive deals and information about upcomming movies with this subscription.

To unsubscribe from these emails, please log into your account and turn off the add option.";

            EmailLogic.SendEmail(email, subject, body);
        }
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