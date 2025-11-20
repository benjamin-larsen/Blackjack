using BlackJack;

Game game = new Game();

/*
 * 0 - Credits
 * 1 - Dealer
 * 2 - Player
 * 3 - Space / Error
 * 4 - Action Prompt
 */

ProgramLoop:
if (game.Credits <= 0)
{
    ConsoleMod.ClearLines(0, 10);
    Console.SetCursorPosition(0, 0);
    Console.WriteLine("You have no more credits left. Game over!");
    goto EndGame;
}

ConsoleMod.ClearLines(0, 10);
PrintCredits(game);

int bet = RequestBet(game);

game.Credits -= bet;

PrintCredits(game);

game.StartGame(bet);
game.RunGame();

PrintCredits(game);

Console.SetCursorPosition(0, 5);

Console.WriteLine("Type \"one more game\" to start again.");
var input = Console.ReadLine();

if (input == "one more game")
{
    goto ProgramLoop;
}

EndGame:
Console.WriteLine("Game Ended.");

static int RequestBet(Game game)
{
    BetLoop:
    ConsoleMod.ClearLine(1);
    Console.Write("Place your bet: ");
    var input = Console.ReadLine();

    if (int.TryParse(input, out int bet))
    {
        if (bet > game.Credits)
        {
            ConsoleMod.ClearLine(2);
            Console.Write("Error: You don't have enough credits. Do you want to take a loan? Type \"loan\".");
            goto BetLoop;
        }

        if (bet <= 0)
        {
            ConsoleMod.ClearLine(2);
            Console.Write("Error: You must bet a positive amount.");
            goto BetLoop;
        }

        return bet;
    }

    ConsoleMod.ClearLine(2);
    Console.Write("Error: Invalid Bet.");
    goto BetLoop;
}

static void PrintCredits(Game game)
{
    ConsoleMod.ClearLine(0);
    Console.SetCursorPosition(0, 0);
    Console.WriteLine($"You have {game.Credits} credits.");
}