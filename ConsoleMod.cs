namespace BlackJack;

public class ConsoleMod
{
    public static void ClearLine(int row)
    {
        Console.SetCursorPosition(0, row);
        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
    }

    public static void ClearLines(int row, int count)
    {
        Console.SetCursorPosition(0, row);

        for (int i = 1; i <= count; i++)
        {
            Console.WriteLine("\r" + new string(' ', Console.WindowWidth) + "\r");
        }

        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
    }
}