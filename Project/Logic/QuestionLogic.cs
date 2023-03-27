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

    public static int AskNumber(string question, bool menuType = false)
    {
        bool Correct = true;
        int awnser = -1;
        // Guide for menuTypes
        if (menuType)
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
                Correct = false;
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Must be a number");
            }
        }
        return awnser;
    }
}