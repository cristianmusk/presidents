using Cards.Domain.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presidents2
{
    class Player
    {
        public List<Card> hand;
        public String name;


        public Player(String name)
        {
            this.name = name;
            hand = new List<Card>();
        }

        public List<Card> FindPlayableCards(Pile pile)
        {

            int jokerCount = 0;
            //if there are no cards on the table, all cards are playable.
            if (pile.Count().Equals(0))
            {
                return hand;
            }
            else
            {
                //TODO uncomment after debugging
                CardNumber pileCardNumber = pile.getCardNumber();
                //for debugging
                //CardNumber pileCardNumber = CardNumber.Four;

                //determine if player has same or equal number of cards (including jokers) with higher value
                jokerCount = hand.Where(n => n.CardNumber == CardNumber.Joker).Count();


                //find cards which have greater value (CardNumber)
                var query = from c in hand
                            where c.CardNumber > pileCardNumber

                            select c;


                //further limit playableCards to find cards where the groupBy Count + Joker >= pile.Count()

                return null;
            }
            

        }

        public void ClearHand()
        {
            hand.Clear();      
        }


    }
}
