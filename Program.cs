namespace BlackJack;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();

        Game game = new Game();
        
        /*
         * 0 - Credits
         * 1 - Dealer
         * 2 - Player
         * 3 - Space / Error
         * 4 - Action Prompt
         */

        while (true)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("You have 0 credits.");
            Console.SetCursorPosition(0, currentLineCursor);
            
            game.StartGame();
            game.RunGame();
            
            Console.WriteLine("Type \"one more game\" to start again.");
            var input = Console.ReadLine();

            if (input != "one more game")
            {
                break;
            }
        }
        
        Console.WriteLine("Game Ended.");
    }
}