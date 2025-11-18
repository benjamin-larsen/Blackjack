namespace BlackJack;

public class Game
{
    public int Credits = 100;
    private int _currentBet = 0;

    public List<Card> CardDeck = new List<Card>();
    public List<Card> PlayerCards = new List<Card>();
    public List<Card> DealerCards = new List<Card>();

    private static Random _rng = new Random();

    private void shuffleCards()
    {
        int n = CardDeck.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            Card value = CardDeck[k];
            CardDeck[k] = CardDeck[n];
            CardDeck[n] = value;
        }
    }

    private static string[] cardTemplates =
    [
        "A",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "J",
        "Q",
        "K"
    ];

    private void generateCardDeck()
    {
        CardDeck.Clear();

        for (int i = 0; i < cardTemplates.Length; i++)
        {
            CardDeck.Add(new Card(cardTemplates[i], Math.Min(i + 1, 10)));
            CardDeck.Add(new Card(cardTemplates[i], Math.Min(i + 1, 10)));
            CardDeck.Add(new Card(cardTemplates[i], Math.Min(i + 1, 10)));
            CardDeck.Add(new Card(cardTemplates[i], Math.Min(i + 1, 10)));
        }

        shuffleCards();
    }

    private Card drawCard()
    {
        var topCard = CardDeck[0];
        CardDeck.RemoveAt(0);

        return topCard;
    }

    public void StartGame(int bet)
    {
        _currentBet = bet;

        generateCardDeck();
        PlayerCards.Clear();
        DealerCards.Clear();

        PlayerCards.Add(drawCard());
        DealerCards.Add(drawCard());

        PlayerCards.Add(drawCard());
        DealerCards.Add(drawCard());
    }

    private bool shouldDealerDraw(List<Card> cards)
    {
        bool isSoft = false;
        bool hasAce = false;
        int sum = 0;

        foreach (var item in cards)
        {
            sum += item.CardValue;
            if (item.CardType == "A")
            {
                hasAce = true;
            }
        }

        if (hasAce && sum < 12)
        {
            isSoft = true;
            sum += 10;
        }

        if (sum < 17) return true;
        if (sum == 17 && isSoft) return true;

        return false;
    }

    private int getCardSum(List<Card> cards, out string str, bool finalGame)
    {
        bool hasAce = false;
        str = "";
        int sum = 0;

        foreach (var item in cards)
        {
            sum += item.CardValue;

            if (item.CardType == "A")
            {
                hasAce = true;
            }
        }

        if (hasAce && sum < 12)
        {
            sum += 10;

            str = finalGame ? $"{sum}" : $"{sum - 10}/{sum}";
        }
        else
        {
            str = $"{sum}";
        }

        return sum;
    }

    public void PrintGame(bool isDealing, bool finalGame)
    {
        int prevLineCursor = Console.CursorTop;
        ConsoleMod.ClearLines(1, 2);
        Console.SetCursorPosition(0, 1);

        if (isDealing)
        {
            getCardSum(DealerCards, out string dealerSumStr, finalGame);
            Console.WriteLine(
                $"Dealer Cards: {string.Join(", ", DealerCards.Select(card => card.CardType))} ({dealerSumStr})");
        }
        else
        {
            Console.WriteLine($"Dealer Cards: {DealerCards[0].CardType}, ?");
        }

        getCardSum(PlayerCards, out string playerSumStr, finalGame);
        Console.WriteLine(
            $"Your Cards: {string.Join(", ", PlayerCards.Select(card => card.CardType))} ({playerSumStr})");

        Console.SetCursorPosition(0, prevLineCursor);
    }

    public enum Action
    {
        Hit,
        Stand
    }

    public Action RequestAction()
    {
        ConsoleMod.ClearLines(3, 2);


        Loop:
        ConsoleMod.ClearLine(4);
        Console.SetCursorPosition(0, 4);
        Console.Write("Select an action ([h]it/[s]tand): ");
        Action? input = Console.ReadLine() switch
        {
            "hit" => Action.Hit,
            "h" => Action.Hit,
            "stand" => Action.Stand,
            "s" => Action.Stand,
            _ => null
        };

        if (input != null)
        {
            return input.Value;
        }

        Console.SetCursorPosition(0, 3);
        Console.Write("Action Error: Invalid Action.");
        goto Loop;
    }

    public void RunGame()
    {
        if (getCardSum(DealerCards, out _, false) == 21) goto EndGame;

        while (getCardSum(PlayerCards, out _, false) < 21)
        {
            PrintGame(false, false);
            Action action = RequestAction();

            if (action == Action.Hit)
            {
                PlayerCards.Add(drawCard());
            }
            else if (action == Action.Stand)
            {
                goto EndGame;
            }
        }

        EndGame:
        ConsoleMod.ClearLines(3, 2);
        Console.SetCursorPosition(0, 4);

        var playerSum = getCardSum(PlayerCards, out _, false);

        if (playerSum > 21)
        {
            PrintGame(false, true);
            Console.WriteLine("You Busted. You lost.");
            return;
        }

        while (shouldDealerDraw(DealerCards))
        {
            PrintGame(true, false);
            DealerCards.Add(drawCard());
            Thread.Sleep(1000);
        }

        var dealerSum = getCardSum(DealerCards, out _, false);

        PrintGame(true, true);

        bool dealerBlackjack = dealerSum == 21 && DealerCards.Count == 2;
        bool playerBlackjack = playerSum == 21 && PlayerCards.Count == 2;

        if (playerBlackjack && !dealerBlackjack)
        {
            Credits += (_currentBet * 3);
            Console.WriteLine("Blackjack!");
        }
        else if (dealerSum > 21)
        {
            Credits += (_currentBet * 2);
            Console.WriteLine("Dealer Busted. You won.");
        }
        else if (playerSum > dealerSum)
        {
            Credits += (_currentBet * 2);
            Console.WriteLine("You won.");
        }
        else if (playerSum < dealerSum || (dealerBlackjack && !playerBlackjack))
        {
            Console.WriteLine("You lost.");
        }
        else
        {
            Credits += _currentBet;
            Console.WriteLine("Push.");
        }
    }
}