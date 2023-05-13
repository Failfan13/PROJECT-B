using System.IO;
using System.Linq;
using System.Text.Json;

public static class Contact
{
    public static void start()
    {
        // creating a selection menu for contact
        Console.Clear();
        string Question = "Select an option\n";
        List<string> Options = new List<string>()
        {
            "Customer service","Complaint","Emergency"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => Contact.contact());
        Actions.Add(() => Contact.Complaint());
        Actions.Add(() => Contact.Emergency());

        // Return to Login menu
        Options.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void contact()
    {
        Console.Clear();
        Console.WriteLine("Our contact information is:");
        Console.WriteLine();
        Console.WriteLine("Number: +31 6 49497715");
        Console.WriteLine("Email: CinemaCustomerservice@hr.nl");       
        Console.WriteLine();
        Console.WriteLine("Press any key to return");
        Console.ReadKey();
        Contact.start();
    }

    public static void Complaint()
    {
        // creating a selection menu for contact
        Console.Clear();
        string Question = "Are you currently in a running movie?";
        List<string> Options = new List<string>()
        {
            "Yes","No","Return"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => Contact.running());
        Actions.Add(() => Contact.Notrunning());
        Actions.Add(() => Contact.start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void Emergency()
    {
        // creating a selection menu for contact
        Console.Clear();
        string Question = "Is there an emergency?";
        List<string> Options = new List<string>()
        {
            "Yes","No"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => Contact.emergencycall());
        Actions.Add(() => Contact.start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void running()
    {
        Console.Clear();
        Console.WriteLine("PLease enter the room number and if applicalbe the corresponding row number.\n");
        string place = Console.ReadLine();
        
        Console.Clear();
        Console.WriteLine("An employee is on their way!");
        Console.ReadKey();
        
        Menu.Start();
    }

    public static void Notrunning()
    {
        if (AccountsLogic.CurrentAccount != null)
        {
            Console.Clear();
            // Console.WriteLine("Please enter your complaint:\n");
            // string complaint = Console.ReadLine();
                        
            if (AccountsLogic.CurrentAccount != null)
            {
                AccountsLogic.AddComplaint(AccountsLogic.CurrentAccount);
            }
        
            // Console.Clear();
            // Console.WriteLine("Your complaint has been sent. We will get back to you as soon as possible\n");
            // Console.WriteLine("Press any key to return");
            // Console.ReadKey();
            Menu.Start();
        }
        else
        {
            Console.Clear();
            string Question = "You are currently not logged in. It is required to be logged in to enter a complaint\nDo you want to login??";
            List<string> Options = new List<string>()
            {
                "Yes","No"
            };
            List<Action> Actions = new List<Action>();
            Actions.Add(() => UserLogin.Start());
            Actions.Add(() => Contact.start());

            MenuLogic.Question(Question, Options, Actions);
        }
    }

    public static void emergencycall()
    {
        Console.Clear();
        Console.WriteLine("The emergency services are getting called");
        Console.ReadKey();
    }
}