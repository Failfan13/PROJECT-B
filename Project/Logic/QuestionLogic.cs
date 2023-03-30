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

    public static int AskNumber(string question, int menuLength = 0)
    {
        bool Correct = true;
        int awnser = -1;
        // Guide for menuTypes
        if (menuLength != 0)
        {
            Console.WriteLine("\n" + @"Make a choice from the menu by entering the
number associated by the function.");
        }
        while (Correct)
        {
            Console.WriteLine(question);
            try
            {
                awnser += Convert.ToInt32(Console.ReadLine());
                // if awnser 0 or less & awnser higher then menu if one appointed
                if (awnser < 0 || (menuLength != 0 ? awnser > menuLength : false))
                {
                    Console.WriteLine("Not a menu option");
                    awnser = -1;
                }
                else
                {
                    Correct = false;
                }
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