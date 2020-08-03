using System;
using System.Collections.Generic;
using System.Linq;

namespace Cards.Domain.Standard
{
    public class Deck
    {
        public Deck()
        {
            Reset();
        }

        public List<Card> Cards { get; set; }

        public void Reset()
        {
            Cards = Enumerable.Range(1, 4)
                //Select cards from 3 up through Ace (14) and 2's (15)
                .SelectMany(s => Enumerable.Range(3, 13)
                                    .Select(c => new Card()
                                    {
                                        Suit = (Suit)s,
                                        CardNumber = (CardNumber)c
                                    }
                                            )
                            )
                   .ToList();

            //add 2 jokers
            Card jokercard1 = new Card() { CardNumber = 0, Suit = Suit.Heart};
            Cards.Add(jokercard1);
            Card jokercard2 = new Card() { CardNumber = 0, Suit = Suit.Diamond};
            Cards.Add(jokercard2);
        }

        public void Shuffle()
        {
            Cards = Cards.OrderBy(c => Guid.NewGuid())
                         .ToList();
        }

        public Card TakeCard()
        {
            var card = Cards.FirstOrDefault();
            Cards.Remove(card);

            return card;
        }

        public IEnumerable<Card> TakeCards(int numberOfCards)
        {
            var cards = Cards.Take(numberOfCards);

            var takeCards = cards as Card[] ?? cards.ToArray();
            Cards.RemoveAll(takeCards.Contains);

            return takeCards;
        }
    }
}