using Cards.Domain.Standard;
using System;

namespace Presidents2
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Starting the program");
            Deck deck = new Deck();
            var p = new Program();
            //p.DealAllCards(deck);

            deck.Reset();

            Game game = new Game();
            game.AddPlayer("cristian");
            game.AddPlayer("nick");
            game.StartGame();
            deck.Reset();
            deck.Shuffle();
            //game.pile.Add(deck.TakeCard());
            //game.players[0].CanPlay(game.pile);

        }

        public void DealAllCardsToTable(Deck deck)
        {
            Card card = new Card();
            deck.Shuffle();
            while (deck.Cards.Count > 0)
            {
                card = (Card)deck.TakeCard();
                Console.WriteLine(card.ToString());

            }
            Console.WriteLine("end of dealing");
        }
    }
}
