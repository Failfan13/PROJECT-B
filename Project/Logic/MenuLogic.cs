using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class MenuLogic
{
    public static int Question(string question, List<string> options, List<Action> actions = null)
    {
        System.Console.Clear();
        Console.WriteLine($"{question}");
        Console.CursorVisible = false;
        ConsoleKeyInfo key;
        int selectedOption = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("Use arrow keys to navigate and press Enter to select an option:\n");

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

            key = Console.ReadKey();

            if (key.Key == ConsoleKey.UpArrow)
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

        } while (key.Key != ConsoleKey.Enter);

        if (actions != null)
        {
            actions[selectedOption]();
        }
        return selectedOption;


    }
}