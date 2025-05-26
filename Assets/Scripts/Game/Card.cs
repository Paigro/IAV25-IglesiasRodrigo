/// <summary>
/// A card with its SUIT and its NUMBER.
/// </summary>
public class Card
{
    /// <summary>
    /// Representa el palo de la carta.
    /// </summary>
    private char _suit;

    /// <summary>
    /// Representa el numero de la carta.
    /// </summary>
    private int _number;

    public Card(char suit, int number)
    {
        _suit = suit;
        _number = number;
    }

    /// <summary>
    /// Saber el palo de la carta.
    /// </summary>
    /// <returns>El palo de la carta.</returns>
    public char GetCardSuit()
    {
        return _suit;
    }

    /// <summary>
    /// Saber el numero de la carta.
    /// </summary>
    /// <returns>El numero de la carta.</returns>
    public int GetCardNumber()
    {
        return _number;
    }

    /// <summary>
    /// El nombre de la carta. En formato PN (PaloNumero)
    /// </summary>
    /// <returns>La combiancion en string del palo y el numero de la carta.</returns>
    public string GetCardName()
    {
        return (_suit.ToString() + _number.ToString());
    }
}