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
                Action recolorSelection = () => Console.Write("");

                if (i == selectedOption)
                {
                    recolorSelection = () => Parallel.Invoke(
                        () => Console.ForegroundColor = ConsoleColor.Black,
                        () => Console.BackgroundColor = ConsoleColor.White
                    );
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                // SHOW OPTIONS  //  if option contains '~' print options after
                // -- if next option doesnt contain '~' print
                if (i + 1 < options.Count && options[i].Contains('~') && !options[i + 1].Contains('~'))
                {
                    Console.ResetColor();
                    Console.Write(" - ");
                    recolorSelection();
                    Console.WriteLine(options[i].ToString().Trim('~'));
                }
                // -- if next option contains '~' print
                else if (i + 1 < options.Count && options[i + 1].Contains('~'))
                    Console.Write(options[i].ToString().Trim('~'));
                // -- normally
                else Console.WriteLine(options[i]);

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

            // Key press directions
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = findNextOption(options, selectedOption, "up");
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = findNextOption(options, selectedOption, "down");
                    break;
                case ConsoleKey.LeftArrow:
                    selectedOption = findNextOption(options, selectedOption, "left");
                    break;
                case ConsoleKey.RightArrow:
                    selectedOption = findNextOption(options, selectedOption, "right");
                    break;
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

    private static int findNextOption(List<string> options, int currentOption, string towards)
    {
        int newOptionIndex = 0;
        bool up = false;
        bool down = false;
        bool right = false;
        bool left = false;

        switch (towards)
        {
            case "up":
                up = true;
                break;
            case "down":
                down = true;
                break;
            case "left":
                left = true;
                break;
            case "right":
                right = true;
                break;
        }

        // Add next index
        if (up || left) newOptionIndex = currentOption -= 1;
        else newOptionIndex = currentOption += 1;

        // Check if index in range && set to begin or end depending
        int nextNotNull = MoveInOptions(options, newOptionIndex);
        if (nextNotNull != newOptionIndex) return nextNotNull;

        // Directional ~ check
        // LEFT
        if (left && options[newOptionIndex].Contains('~') && options[currentOption].Contains('~'))
        {
            return newOptionIndex;
        }
        // RIGHT
        else if (right && options[newOptionIndex].Contains('~') && options[currentOption].Contains('~'))
        {
            return newOptionIndex;
        }
        // UP
        else if (up && options[newOptionIndex].Contains('~'))
        {
            while (true)
            {
                newOptionIndex--;
                newOptionIndex = MoveInOptions(options, newOptionIndex);

                if (!options[newOptionIndex].Contains('~')) break;
            }
        }
        // DOWN
        else if (down && options[newOptionIndex].Contains('~'))
        {
            while (true)
            {
                newOptionIndex++;
                newOptionIndex = MoveInOptions(options, newOptionIndex);

                if (!options[newOptionIndex].Contains('~')) break;
            }
        }
        return newOptionIndex;
    }

    private static int MoveInOptions(List<string> options, int currentOption)
    {
        if (currentOption >= options.Count)
        {
            return 0;
        }
        else if (currentOption < 0)
        {
            return options.Count - 1;
        }
        return currentOption;
    }
    public static void ClearLastLines(int linesToClear, bool threadDelay = false)
    {
        try
        {
            if (threadDelay) Thread.Sleep(3000);

            linesToClear += 1; // adds new input to clear
                               // current cursor position
            int currentLine = Console.CursorTop + 1; // sets top one line lower for input

            Console.SetCursorPosition(0, currentLine - linesToClear);

            // Clear lines replace with empty strings
            Console.Write(new string(' ', Console.WindowWidth * linesToClear));

            Console.SetCursorPosition(0, currentLine - linesToClear);
        }
        catch (System.ArgumentOutOfRangeException) // more then amount of lines to clear
        {
            return;
        }
    }

    public static void ClearFromTop(int fromTop, bool threadDelay = false)
    {
        try
        {
            if (threadDelay) Thread.Sleep(3000);
            // current cursor position
            int currentLine = fromTop; // sets top one line lower for input

            Console.SetCursorPosition(0, currentLine);

            // Clear lines replace with empty strings
            Console.Write(new string(' ', Console.WindowWidth * (Console.WindowHeight - fromTop)));

            Console.SetCursorPosition(0, currentLine);
        }
        catch (System.ArgumentOutOfRangeException) // more then amount of lines to clear
        {
            return;
        }
    }

    public static void ColorString(string str, ConsoleColor color = ConsoleColor.DarkBlue, bool newLine = true)
    {
        Console.ForegroundColor = color;
        if (newLine) Console.WriteLine(str); // takes whole line
        else Console.Write(str); // used inline
        Console.ResetColor();
    }
    public static string BoldString(string str) => $"\x1b[1m{str}\x1b[0m";
}