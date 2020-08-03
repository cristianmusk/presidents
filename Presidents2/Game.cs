using Cards.Domain.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presidents2
{
    class Game
    {
        public List<Player> players;
        //play pile. reset after each turn
        public List<Card> pile;
        public Player lastPersonToPlay;
        public Player currentPlayerTurn;

        public Game()
        {
            players = new List<Player>();
            pile = new List<Card>(); 
            
        }

        public Game(int numOfPlayers)
        {
            players = new List<Player>(numOfPlayers);
        }
        public void AddPlayer(string name)
        {
            Player p = new Player(name);
            players.Add(p);
        }

        public void Deal()
        {
            Deck deck = new Deck();
            Card card = new Card();
            deck.Shuffle();
            int i = 0;
            while (deck.Cards.Count > 0)
            {
                //deal card player
                players[i].hand.Add((Card)deck.TakeCard());
                Console.WriteLine(card.ToString());
                //if last player, then restart with the first player. change this to Game.NextPlayer or seat arrangement
                i++;
                if (i == (players.Count)) { i = 0; }
            }
            Console.WriteLine("end of dealing");
        }

        public void Turn(Player player)
        {
            
        }
    }

}
