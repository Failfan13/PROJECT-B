using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class MenuLogic
{
    public static int Question(string question, List<string> options, List<Action> actions = null, string BottomString = null)
    {
        Console.CursorVisible = false;
        ConsoleKeyInfo key;
        int selectedOption = 0;
        do
        {
            // start of visual
            Console.Clear();
            Console.WriteLine("Use arrow keys to navigate and press Enter to select an option:\n");

            // ask question
            Console.WriteLine(question);

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.WriteLine(options[i]);

                Console.ResetColor();
            }

            // if bottomstring display it
            if (BottomString != null)
            {
                Console.WriteLine(BottomString);
            }

            // read key
            key = Console.ReadKey();

            // if key is x return to start;
            if (key.Key == ConsoleKey.X)
            {
                Menu.Start();
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                selectedOption--;
                if (selectedOption < 0)
                {
                    selectedOption = options.Count - 1;
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedOption++;
                if (selectedOption >= options.Count)
                {
                    selectedOption = 0;
                }
            }
        } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.X);


        if (key.Key == ConsoleKey.Enter)
        {
            if (actions != null)
            {
                // if there is an action, use it
                actions[selectedOption]();
            }
        }

        // no action, return the index of the option
        return selectedOption;
    }
}