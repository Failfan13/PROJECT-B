public static class QuestionLogic
{
    public static string AskString(string question)
    {
        Console.Clear();
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

    public static double AskNumber(string question)
    {
        Console.Clear();
        bool Correct = true;
        double awnser = -1;
        while (Correct)
        {
            Console.WriteLine(question);
            try
            {
                awnser = Convert.ToDouble(Console.ReadLine().Replace(",", "."));

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
        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }
}