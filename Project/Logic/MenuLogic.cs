using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class MenuLogic
{
    public static int Question(string question, List<string> options, List<Action> actions = null)
    {
        System.Console.Clear();
        var option = WhatToDo(question, options);
        int opt = Convert.ToInt32(option.ToString());
        if (actions != null)
        {
            actions[opt - 1]();
        }
        return opt;
    }

    private static char WhatToDo(string question, List<string> options)
    {
        char option = Actions(question, options);
        while (option < '1' || option > Convert.ToChar((options.Count).ToString()))
        {
            System.Console.Clear();
            Console.WriteLine("I do not understand");
            option = Actions(question, options);
        }
        return option;
    }

    private static char Actions(string question, List<string> options)
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