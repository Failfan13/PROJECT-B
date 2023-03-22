static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login options");
        Console.WriteLine("Enter 2 add movie");
        Console.WriteLine("Enter 3 to see all timeslots for a movie");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Movies.AddNewMovie();
        }
        else if (input == "3")
        {
            TimeSlots.WhatMovie();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }
}