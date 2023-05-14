using System.IO;
using System.Linq;
using System.Text.Json;

public static class Contact
{
    private static AccountsLogic AL = new AccountsLogic();

    public static void Start()
    {
        // creating a selection menu for contact
        Console.Clear();
        string Question = "Select an option\n";
        List<string> Options = new List<string>()
        {
            "Contact information","Make a Complaint","Emergency"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => Contact.contact());
        Actions.Add(() => Contact.ComplaintMenu());
        Actions.Add(() => Contact.Emergency());

        // Return to Login menu
        Options.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void contact()
    {
        Console.Clear();
        Console.WriteLine("Our contact information is:\n");
        Console.WriteLine("Tel number: +31 6 49497715\n");
        Console.WriteLine("Email: ProjectB.TeamE.Infomatic@gmail.com\n");
        QuestionLogic.AskEnter();
        Start();
    }

    public static void ComplaintMenu()
    {
        // creating a selection menu for contact
        Console.Clear();

        string Question = "What type of complaint do you have?";
        List<string> Options = new List<string>()
        {
            "User complaint","Employee complaint", "other"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => AddComplaint("User"));
        Actions.Add(() => AddComplaint("Employee"));
        Actions.Add(() => AddComplaint("Other"));

        Options.Add("Return");
        Actions.Add(() => Start());

        MenuLogic.Question(Question, Options, Actions);

        Start();
    }

    private static void AddComplaint(string type)
    {
        bool ongoingMovie = false;

        if (type == "User")
        {
            string Question = "Are you currently watching a movie?";
            List<string> Options = new List<string>()
            {
            "Yes","No"
            };
            List<Action> Actions = new List<Action>();
            Actions.Add(() => ongoingMovie = true);
            Actions.Add(() => ongoingMovie = false);

            Options.Add("Return");
            Actions.Add(() => ComplaintMenu());

            MenuLogic.Question(Question, Options, Actions);
        }

        if (ongoingMovie) // movie is ongoing
        {
            Console.Clear();
            Console.WriteLine("Please enter the room number");
            string place = Console.ReadLine()!;

            Console.WriteLine("if applicable the corresponding row number");
            string seat = Console.ReadLine()!;

            Console.Clear();
            Console.WriteLine("An employee is on their way!");
            QuestionLogic.AskEnter();

            place = type + $": Complaint in room: {place} Seat: {seat}";

            if (AccountsLogic.CurrentAccount != null)
            {
                AL.AddComplaint(type, place);
            }
            Start();
        }
        else if (!ongoingMovie) // movie is not ongoing
        {
            Console.Clear();

            if (AccountsLogic.CurrentAccount == null)
            {
                Console.WriteLine("To use this feature, you must be logged in.");
                QuestionLogic.AskEnter();
                Menu.Start();
            }
            else
            {
                Console.WriteLine("Please enter your complaint:");
                string complaint = Console.ReadLine()!;

                complaint = type + $": {complaint}";

                AL.AddComplaint(type, complaint);

                Console.Clear();
                Console.WriteLine("Your complaint has been sent. We will get back to you as soon as possible");
                QuestionLogic.AskEnter();
                Start();
            }
        }
    }

    public static void ViewComplaints(AccountModel account) => ViewAllComplaints(account);

    public static void ViewAllComplaints(AccountModel account = null!)
    {
        AccountsLogic AL = new AccountsLogic();

        if (account == null)
        {
            foreach (var acc in AL.GetAllAccounts().Where(a => a.Complaints.Count > 0))
            {
                Console.Write("UserId:" + acc.Id + "\n");
                foreach (var complaint in acc.Complaints)
                {
                    Console.Write(complaint.ToString() + "\n");
                }
                Console.WriteLine("\n");
            }

            QuestionLogic.AskEnter();
        }
        else
        {
            string Question = "Select a complaint to edit";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            for (int i = 0; i < account.Complaints.Count; i++)
            {
                Options.Add(account.Complaints[i].ToString());
                Actions.Add(() => AccountsLogic.EditComplaint(account, i));
            }

            Options.Add("Return");
            Actions.Add(() => User.Info(account));

            MenuLogic.Question(Question, Options, Actions);
        }
    }

    public static void Emergency()
    {
        // creating a selection menu for contact
        Console.Clear();
        string Question = "Is there an emergency?/n/nDeclaring emergency without cause is punishable";
        List<string> Options = new List<string>()
        {
            "Yes","No"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => Contact.EmergencyCall());
        Actions.Add(() => Start());

        MenuLogic.Question(Question, Options, Actions);

        Menu.Start();
    }

    public static void EmergencyCall()
    {
        Console.Clear();
        Console.WriteLine("Enter the room number");
        string roomNumber = Console.ReadLine()!;

        Console.Write("Emergency services have been called towards room: " + roomNumber);
        Console.WriteLine("\nPlease locate towards the emergency exit and await peramedic arrival");
    }

}