static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("INFO TEXT");

        Console.WriteLine("Make a choice from the menu by entering the number.\n");

        Console.WriteLine("Main Menu:");
        Console.WriteLine("1 Login");
        Console.WriteLine("2 Create new account");
        Console.WriteLine("3 Return to menu");
        Console.WriteLine("4 Change password (Temporarily here only use after being logged in)");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            Login();
        }
        else if (input == "2")
        {
            CreateNewUser();
        }
        else if (input == "3")
        {
            Menu.Start();
        }
        else if (input == "4")
        {
            ChangePassword();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }

    public static void CreateNewUser()
    {
        bool CorrectEmail = true;
        bool CorrectName = true;
        bool CorrectPass = true;
        string pass = "";
        string Email = "";
        string Name = "";

        Console.Clear();
        while (CorrectEmail)
        {
            Console.WriteLine("Please enter your email address:");
            Email = Console.ReadLine();
            if (Email != "")
            {
                CorrectEmail = false;
            }
            else
            {
                Console.WriteLine("Invalid value");
            }
        }

        Console.Clear();
        while (CorrectName)
        {
            Console.WriteLine("Please enter your full name:");
            Name = Console.ReadLine();
            if (Name != "")
            {
                CorrectName = false;
            }
            else
            {
                Console.WriteLine("Invalid value");
            }
        }

        Console.Clear();
        while (CorrectPass)
        {
            Console.WriteLine("Please enter your password:");
            string pass1 = Console.ReadLine();
            Console.WriteLine("Please enter your password again:");
            string pass2 = Console.ReadLine();
            if (pass1 == pass2 && pass1 != "")
            {
                pass = pass1;
                CorrectPass = false;
            }
            else
            {
                Console.WriteLine("Passwords dont match");
            }
        }

        accountsLogic.NewAccount(Email, Name, pass);


    }

    public static void Login()
    {
        if (AccountsLogic.CurrentAccount == null)
        {
            Console.Clear();
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine();
            Console.WriteLine("Please enter your password");
            string password = Console.ReadLine();
            AccountModel acc = accountsLogic.CheckLogin(email, password);
            if (acc != null)
            {
                Console.WriteLine("Welcome back " + acc.FullName);
                Console.WriteLine("Your email number is " + acc.EmailAddress);

                //Write some code to go back to the menu
                Start();
            }
            else
            {
                Console.WriteLine("No account found with that email and password");
                Start();
            }
        }
        else
        {
            Console.WriteLine($"You are logged in with: {AccountsLogic.CurrentAccount.EmailAddress}.\nDo you want to log out? Y/N");
            string awnser = Console.ReadLine();
            if (awnser.ToLower() == "y")
            {
                accountsLogic.LogOut();
            }
            else
            {
                Menu.Start();
            }
        }

    }
    public static void ChangePassword()
    {
        Console.WriteLine("Please enter old password");
        string oldpws = Console.ReadLine();
        if  (accountsLogic.CheckLogin(AccountsLogic.CurrentAccount.EmailAddress, oldpws) == null)
        {
            Console.WriteLine("Wrong password");
            ChangePassword();
        }
        else
        {
            Console.WriteLine("Please enter new password");
            string newpassword = Console.ReadLine();
            accountsLogic.NewPassword(newpassword);
        }
    }
}