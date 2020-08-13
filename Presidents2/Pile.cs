using Cards.Domain.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presidents2
{
    class Pile
    {

        private List<Card> pile = new List<Card>();
        private CardNumber cardNumber;

        public void PutCards(List<Card> cards)
        {
            pile = cards;
            foreach(Card c in cards)
            {
                //find a non-joker and update pile cardNumber to it
                if(c.CardNumber != CardNumber.Joker) { 
                    cardNumber = c.CardNumber; 
                    break; 
                }
                else
                {
                    cardNumber = c.CardNumber;
                }
            }
        }

        public void WipePile()
        {
            pile.Clear();
        }

        public CardNumber getCardNumber()
        {
            return cardNumber;
        }


        public int Count()
        {
            return pile.Count();
        }
    }
}
