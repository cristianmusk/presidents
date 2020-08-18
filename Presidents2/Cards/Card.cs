using System;

namespace Cards.Domain.Standard
{
    public class Card : ICard
    {
        public Suit Suit { get; set; }
        public CardNumber CardNumber { get; set; }

        public override String ToString()
        {
            return CardNumber + " of " + Suit;
        }

        public Card()
        {
        }

        public Card(CardNumber cn, Suit s)
        {
            CardNumber = cn;
            Suit = s;
        }

        public Card(string cn)
        {
        }

        public static bool operator ==(Card c1, Card c2)
        {
            return (c1.CardNumber == c2.CardNumber && c1.Suit == c2.Suit);
        }

        public static bool operator !=(Card c1, Card c2)
        {
            return (c1.CardNumber != c2.CardNumber || c1.Suit != c2.Suit);
        }
    }
}