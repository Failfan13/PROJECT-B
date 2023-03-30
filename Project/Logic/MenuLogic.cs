using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class MenuLogic
{
    public static void Question(string question, List<string> options, List<Action> actions)
    {
        System.Console.Clear();
        var option = WhatWouldYouLikeToDo(question, options);
        int opt = Convert.ToInt32(option.ToString());
        actions[opt - 1]();

    }

    private static char WhatWouldYouLikeToDo(string question, List<string> options)
    {
        char option = AskForAction(question, options);
        while (option < '1' || option > Convert.ToChar((options.Count).ToString()))
        {
            System.Console.Clear();
            Console.WriteLine("I do not understand");
            option = AskForAction(question, options);
        }
        return option;
    }

    private static char AskForAction(string question, List<string> options)
    {
        Console.WriteLine($"{question}");
        for (int i = 1; i < options.Count + 1; i++)
        {
            Console.WriteLine($"{i}.{options[i - 1]}");
        }
        char option = Console.ReadKey().KeyChar;
        Console.WriteLine("\n");
        return option;
    }
}