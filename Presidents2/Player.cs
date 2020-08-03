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
        public int tablePosition;
        public String name;


        public Player(String name)
        {
            this.name = name;
            hand = new List<Card>();
        }

        public bool CanPlay(List<Card> pile)
        {
            //pile Value
            CardNumber pileCardNumber;
            foreach (Card c in pile)
            {
                if (c.CardNumber != CardNumber.Joker)
                {
                    pileCardNumber = c.CardNumber;
                    break;
                }
            }

            //determine if player has same or equal number of cards (including jokers) with higher value
            int count = hand.Where(n => n.CardNumber == CardNumber.Joker).Count();
            //count += hand.Where(n => n.CardNumber > pileCardNumber).GroupBy<n.CardNumber>;

            var query = hand.GroupBy(x => x.CardNumber).Count();
            return true;
            //var query = tapesTable.GroupBy(x => x.Tape)
            //          .Select(x => x.OrderByDescending(t => t.Count)
            //                        .First());
        }

    }
}
