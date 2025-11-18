namespace BlackJack;

public class Card
{
    public int CardValue;
    public string CardType;

    public Card(string cardType, int cardValue)
    {
        CardValue = cardValue;
        CardType = cardType;
    }
}