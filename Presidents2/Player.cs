using Cards.Domain.Standard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presidents2
{
    public class Player
    {
        public List<Card> hand;
        public String name;

        public Player(String name)
        {
            this.name = name;
            hand = new List<Card>();
        }

        public List<Card> FindPlayableCards(Boolean isFirstHand, Pile pile)
        {
            List<Card> playableCards = new List<Card>();
            int jokerCount = 0;

            //***********************************
            //FOR DEBUGGING ADDING SOME CARDS TO THE PILE
            //***********************************
            //pile.PutCards(new List<Card> { new Card(CardNumber.Seven, Suit.Club) });

            if (isFirstHand)
            {
                //if this is the first hand, 3's and jokers are acceptable
                playableCards.AddRange(hand.Where(x => x.CardNumber.Equals(CardNumber.Three) || x.CardNumber.Equals(CardNumber.Joker)));
                return playableCards;
            }
            else if (pile.Count().Equals(0))
            {
                //if there are no cards on the table, all cards are playable.
                return hand;
            }
            else
            {
                CardNumber pileCardNumber = pile.getCardNumber();

                //determine if player has same or equal number of cards (including jokers) with higher value
                jokerCount = hand.Where(n => n.CardNumber == CardNumber.Joker).Count();

                //find cards which have greater value (CardNumber) and Quantity + JokerCount >= pileCount

                var group = from c in hand
                            where c.CardNumber > pileCardNumber
                            group c by c.CardNumber into g
                            where g.Count() + jokerCount >= pile.Count()
                            select g;

                //add items in the result
                for (int i = 0; i < group.Count(); i++)
                {
                    for (int j = 0; j < group.ElementAt(i).Count(); j++)
                    {
                        playableCards.Add(group.ElementAt(i).ElementAt(j));
                    }
                }

                //if there are any jokers, add them if other cards are playable, or there is enough jokers to cover the piler and play only jokers
                if (playableCards.Count() > 0 || jokerCount >= pile.Count())
                {
                    playableCards.AddRange(hand.FindAll(FindJoker));
                }

                return playableCards;
            }
        }

        // Explicit predicate delegate.
        private static bool FindJoker(Card c)
        {
            if (c.CardNumber == CardNumber.Joker)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FindAndRemoveCard(Card c)
        {
            //hand.Find
            return true;
        }

        public void ClearHand()
        {
            hand.Clear();
        }
    }
}