using Cards.Domain.Standard;
using System.Collections.Generic;
using System.Linq;

namespace Presidents2
{
    public class Pile
    {
        private List<Card> pile = new List<Card>();

        public void PutCards(List<Card> cards)
        {
            pile = cards;
        }

        public void WipePile()
        {
            pile.Clear();
        }

        public CardNumber getCardNumber()
        {
            CardNumber cardNumber = new CardNumber();
            foreach (Card c in pile)
            {
                //find a non-joker and update pile cardNumber to it
                if (c.CardNumber != CardNumber.Joker)
                {
                    cardNumber = c.CardNumber;
                    break;
                }
                else
                {
                    cardNumber = c.CardNumber;
                }
            }
            return cardNumber;
        }

        public int Count()
        {
            return pile.Count();
        }
    }
}