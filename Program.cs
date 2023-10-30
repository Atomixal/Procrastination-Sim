using System.Threading;
using System.Collections.Generic;
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the 'Try not to procrastinate challange'!");
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine("Duration: 1-week half term holiday.");
        Console.WriteLine("Goal: Complete your homework with a 100% score.");
        Console.WriteLine("Note: Procrastination can be detrimental to your sanity!");
        Console.WriteLine("Focus: Your homework completion chances depend on it.");
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine("Instructions:");
        Console.WriteLine("  - Use the arrow keys to navigate options.");
        Console.WriteLine("  - Press Enter to make a choice.");
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();

        int fatigueLevel = 0;
        Random rng = new Random();
        bool eventOccurred = false;


        int homeworkCompletion = 0;
        int sanityLevel = 10;
        int holidayDuration = 7; // 1 weeks
        int daysSpent = 0;

        bool hasWon = false;
        bool hasLostDueToHolidayEnd = false;
        bool hasLostDueToSanity = false;

        int selectedOption = 0;
        int consecutiveProcrastination = 0;

        string activity = "None";  // Initialize activity to "None" at the start

        Console.CursorVisible = false;

        do
        {
            Console.Clear();

            int timeRemaining = holidayDuration - daysSpent;
            Console.WriteLine($"Time remaining: {timeRemaining} days");
            Console.WriteLine("------------------------------------------------------");
            DisplayMeters(homeworkCompletion, sanityLevel);

            if (rng.Next(1, 11) > 8) // 20% chance
            {
                HandleRandomEvent(ref sanityLevel, ref homeworkCompletion, ref fatigueLevel);
                eventOccurred = true;
            }
            if (eventOccurred)
            {
                Thread.Sleep(3000); // Pause to let the player read the event text
                eventOccurred = false; // Reset the flag
            }
            if (fatigueLevel >= 10)
            {
                Console.WriteLine("You're too fatigued to work. You must rest!");
                continue;
            }

            List<string> menuOptions = new List<string>();

            if (consecutiveProcrastination == 0)
            {
                menuOptions.Add("Work on homework");
            }

            menuOptions.Add("Procrastinate");
            menuOptions.Add("Quit the game");

            selectedOption = DisplayMenu("Select an option:", menuOptions.ToArray(), selectedOption);


            // Homework Types
            if (selectedOption == 0 && consecutiveProcrastination == 0)
            {
                string type = HandleHomeworkType();
                switch (type)
                {
                    case "Easy":
                        homeworkCompletion += 1;
                        fatigueLevel += 1;
                        break;
                    case "Medium":
                        homeworkCompletion += 2;
                        fatigueLevel += 3;
                        break;
                    case "Hard":
                        homeworkCompletion += 3;
                        fatigueLevel += 5;
                        break;
                }
                sanityLevel--;
            }
            else if (selectedOption == 1)
            {
                // Procrastinate
                sanityLevel++;
                fatigueLevel -= 3;
                fatigueLevel = Clamp(fatigueLevel, 0, 10);
                daysSpent++;
            }

            if (sanityLevel <= 0)
            {
                hasLostDueToSanity = true;
                break;
            }

            if (selectedOption == 0)
            {
                // Homework logic
                if (consecutiveProcrastination == 0)
                {
                    string[] homeworkActivities = { "Reading", "Writing", "Calculating", "Researching" };
                    activity = homeworkActivities[new Random().Next(homeworkActivities.Length)];
                    Console.WriteLine($"You made progress on your homework. Activity: {activity}");
                    homeworkCompletion++;
                    sanityLevel--;
                }
            }
            else if (selectedOption == 1)
            {
                // Procrastination logic
                string[] procrastinationActivities = { "Watching TV", "Playing games", "Daydreaming", "Socializing" };
                activity = procrastinationActivities[new Random().Next(procrastinationActivities.Length)];
                Console.WriteLine($"You procrastinated. Activity: {activity}");
                sanityLevel++;
                daysSpent++;
            }
            else if (selectedOption == 2)
            {
                // Quit logic
                Console.WriteLine("You've quit the game.");
                return;
            }

            // Clamp the values
            sanityLevel = Clamp(sanityLevel, 0, 10);
            homeworkCompletion = Clamp(homeworkCompletion, 0, 10);
            daysSpent++;

            if (homeworkCompletion >= 10)
            {
                hasWon = true;
            }

            if (daysSpent >= holidayDuration)
            {
                hasLostDueToHolidayEnd = true;
            }

            DisplayStickman(sanityLevel, activity);

            Thread.Sleep(1000);

        } while (!hasWon && !hasLostDueToHolidayEnd && !hasLostDueToSanity);

        if (hasWon)
        {
            Console.WriteLine($"Congratulations! You've completed your homework with a 100% score in {daysSpent} days.");
        }
        else if (hasLostDueToHolidayEnd)
        {
            Console.WriteLine("You've run out of time. Summer holiday has ended, and your homework is incomplete!");
        }
        else if (hasLostDueToSanity)
        {
            Console.WriteLine("You've lost because your sanity reached 0%. You need to take care of your mental health!");
        }

        Console.CursorVisible = true;
        Console.ReadLine();
    }

    static int DisplayMenu(string prompt, string[] options, int selectedOption)
    {
        Console.SetCursorPosition(0, 10);
        Console.WriteLine(prompt);
        for (int i = 0; i < options.Length; i++)
        {
            if (i == selectedOption)
                Console.Write("> ");
            else
                Console.Write("  ");

            Console.WriteLine(options[i]);
        }
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < options.Length - 1)
                selectedOption++;
            else if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 0)
                selectedOption--;
            else if (keyInfo.Key == ConsoleKey.Enter)
                return selectedOption;

            // Clear previous menu
            Console.SetCursorPosition(0, 11);
            for (int i = 0; i < options.Length; i++)
            {
                Console.Write(new string(' ', options[i].Length + 2));
                Console.WriteLine();
            }
            // Draw new menu
            Console.SetCursorPosition(0, 11);
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedOption)
                    Console.Write("> ");
                else
                    Console.Write("  ");

                Console.WriteLine(options[i]);
            }
        }
    }


    static void DisplayMeters(int homeworkCompletion, int sanityLevel)
    {
        Console.WriteLine("Homework Completion:");
        Console.Write("[");
        for (int i = 0; i < 10; i++)
        {
            if (i < homeworkCompletion)
                Console.Write("#");
            else
                Console.Write(" ");
        }
        Console.WriteLine("]");

        Console.WriteLine("Sanity Level:");
        Console.Write("[");
        for (int i = 0; i < 10; i++)
        {
            if (i < sanityLevel)
                Console.Write("#");
            else
                Console.Write(" ");
        }
        Console.WriteLine("]");
    }

    static int Clamp(int value, int min, int max)
    {
        return (value < min) ? min : (value > max) ? max : value;
    }

    static void DisplayStickman(int sanityLevel, string activity)
    {
        string stickman = "";
        switch (sanityLevel)
        {
            case int n when (n > 7):
                stickman = "  ^_^\n /   \\\n -----";
                break;
            case int n when (n > 4):
                stickman = "  -_-\n /   \\\n -----";
                break;
            case int n when (n > 2):
                stickman = "  T_T\n /   \\\n -----";
                break;
            case int n when (n > 0):
                stickman = "  >_<\n /   \\\n -----";
                break;
            default:
                stickman = "  X_X\n /   \\\n -----";
                break;
        }
        Console.WriteLine($"Activity: {activity}");
        Console.WriteLine(stickman);
    }

    static string HandleHomeworkType()
    {
        string[] homeworkTypes = { "Easy", "Medium", "Hard" };
        Console.WriteLine("Choose the type of homework:");
        int selected = DisplayMenu("Select a homework type:", homeworkTypes, 0);
        return homeworkTypes[selected];
    }

    static void HandleRandomEvent(ref int sanityLevel, ref int homeworkCompletion, ref int fatigueLevel)
    {
        List<string> events = new List<string>
    {
        "Your friends invite you out. Do you go and recover some sanity but lose a day?",
        "Your computer crashes, losing a day of work but saving some sanity since you just have to walk away."
    };

        int chosenEvent = new Random().Next(events.Count);
        Console.WriteLine($"Event: {events[chosenEvent]}");
        string[] choices = { "Yes", "No" };
        int selected = DisplayMenu("What will you do?", choices, 0);

        if (chosenEvent == 0)
        {
            if (selected == 0)
            {
                sanityLevel += 3;
                fatigueLevel -= 3;
            }
        }
        else if (chosenEvent == 1)
        {
            if (selected == 0)
            {
                sanityLevel += 2;
                homeworkCompletion--;
            }
        }
    }

}