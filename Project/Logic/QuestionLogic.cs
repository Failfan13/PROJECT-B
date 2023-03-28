public static class QuestionLogic
{
    public static string AskString(string question)
    {
        bool Correct = true;
        string awnser = null;

        while (Correct)
        {
            Console.WriteLine(question);
            awnser = Console.ReadLine();
            if (awnser != null)
            {
                Correct = false;
            }
            else
            {
                Console.WriteLine("Can't be empty");
            }
        }
        return awnser;
    }

    public static int AskNumber(string question)
    {
        bool Correct = true;
        int awnser = -1;
        while (Correct)
        {
            Console.WriteLine(question);
            try
            {
                awnser = Convert.ToInt32(Console.ReadLine());
                Correct = false;
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Must be a number");
            }
        }
        return awnser;
    }

    public static void AskEnter()
    {
        Console.WriteLine("Press enter to return");
        Console.ReadLine();
    }
}