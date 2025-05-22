public struct Card
{
    private char _suit;
    private int _number;

    public Card(char suit, int number)
    {
        _suit = suit;
        _number = number;
    }

    public char getSuit()
    {
        return _suit;
    }
    public int getNumber()
    {
        return _number;
    }
}