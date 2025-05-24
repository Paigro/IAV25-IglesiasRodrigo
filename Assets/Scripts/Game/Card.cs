/// <summary>
/// A card with its SUIT and its NUMBER.
/// </summary>
public class Card
{
    private char _suit;
    private int _number;

    public Card(char suit, int number)
    {
        _suit = suit;
        _number = number;
    }

    public char GetCardSuit()
    {
        return _suit;
    }
    public int GetCardNumber()
    {
        return _number;
    }
    public string GetCardName()
    {
        return (_suit.ToString() + _number.ToString());
    }
}