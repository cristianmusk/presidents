using Cards.Domain.Standard;
using System;

namespace Presidents2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting the program");

            Game game = new Game();
            //game.AddPlayer("CRISTIAN");
            //game.AddPlayer("NICK");
            //game.AddPlayer("JOSH");
            game.StartGame();
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