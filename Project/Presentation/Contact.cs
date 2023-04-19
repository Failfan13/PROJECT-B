public static class Contact
{
    public static void start()
    {
        // creating a selection menu for contact
        Console.Clear();
        string Question = "Select an option\n";
        List<string> Options = new List<string>()
        {
            "Customer service","Nuisance","Emergency"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => Contact.contact());
        Actions.Add(() => Contact.Nuisance());
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

    public static void Nuisance()
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
        Actions.Add(() => Contact.Emergency());

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
        Menu.Start();
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
        Actions.Add(() => Contact.Emergency());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void Notrunning()
    {
        Console.Clear();
        Console.WriteLine("Please enter your complaint:");
        Console.WriteLine();
        string complaint = Console.ReadLine();

        // complaint is een string met de klacht. csv? json? 
        
        Console.Clear();
        Console.WriteLine("Your complaint has been sent. We will get back to you as soon as possible");
        Console.WriteLine();
        Console.WriteLine("Press any key to return");
        Console.ReadKey();
        Menu.Start();
    }

    public static void emergencycall()
    {
        Console.Clear();
        Console.WriteLine("The emergency services is getting called");
        Console.ReadKey();
    }
}