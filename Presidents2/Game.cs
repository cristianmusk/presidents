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
        public Pile pile;
        public int lastPersonToPlay;
        public int currentPlayerTurn = -1;
        public Deck deck = new Deck();


        public Game()
        {
            players = new List<Player>();
            pile = new Pile();

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
            deck.Reset();
            deck.Shuffle();

            //last player deals, starting dealing cards to the "left" which would be i=0
            int i = players.Count() - 1;
            while (deck.Cards.Count > 0)
            {
                //deal card player
                players[i].hand.Add((Card)deck.TakeCard());

                Console.WriteLine(players[i].hand.Last().ToString());
                //if last player, then restart with the first player. change this to Game.NextPlayer or seat arrangement
                i++;
                if (i == (players.Count)) { i = 0; }
            }
            Console.WriteLine("end of dealing");
        }

        public void Turn(int i)
        {
            //if not empty and pile.cardnumber =0 or 2 (15), skip turn and log message
            if (!pile.Count().Equals(0))
            {
                if (pile.getCardNumber() == CardNumber.Two || pile.getCardNumber() == CardNumber.Joker)
                {
                    Console.WriteLine("Pile is a 2, skip turn");
                }
            }

            if (lastPersonToPlay == currentPlayerTurn)
            {
                pile.WipePile();
            }

                players[i].FindPlayableCards(pile);
            
            
            


            //check if user can play
            // Type your username and press enter
            Console.WriteLine("Type Play or Pass");
            // Create a string variable and get user input from the keyboard and store it in the variable
            string playOrPass = Console.ReadLine();
            if ("Play".Equals(playOrPass))
            {

            }
            else
            {
                //pass, do nothing
            }

            //increment player turn
            currentPlayerTurn++;
        }

        public void StartGame()
        {
            Boolean isFirstGame = true;
            Boolean keepGoing = true;
            while (keepGoing)
            {
                Deal();
                if (!isFirstGame)
                {
                    //TODO first place and last player trade 2 cards
                    //TODO 2nd place and 2nd last trade 1 cards
                }

                isFirstGame = false;
                Find3Clubs();
                //currentPlayerTurn is already set to the player with the 3 of clubs
                Turn(currentPlayerTurn);

            }
        }
        //finding the 3 of clubs while deailng
        private int Find3Clubs()
        {
            int i = 0;

            Card threeOfClubs = new Card(CardNumber.Three, Suit.Club);
            foreach (Player p in players)
            {

                //check for 3 of clubs here to save off who will start the game, rather than search all players hands later.
                if (p.hand.Exists(t =>
    (t.CardNumber == threeOfClubs.CardNumber && t.Suit == threeOfClubs.Suit)))
                {
                    currentPlayerTurn = i;
                    return i;
                }
                i++;

            }
            return i;

        }
    }

}
