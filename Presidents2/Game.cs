using Cards.Domain.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Presidents2
{
    class Game
    {
        public List<Player> players;
        public List<Player> nextGamePlayers;

        //play pile. reset after each turn
        public Pile pile;
        public int lastPersonToPlay = -1;
        public int currentPlayerTurn = -1;
        public Deck deck = new Deck();

        public Game()
        {
            players = new List<Player>();
            nextGamePlayers = new List<Player>();

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

            //clear hands -- in subsequent games, 1 player would have a remaining hand
            foreach (Player p in players)
            {
                p.ClearHand();
            }

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
            //if the current player is already out, go to the next player turn
            if (players[i].hand.Count() == 0) { currentPlayerTurn++; return; }

            string passOrPlay = null;
            //if not empty and pile.cardnumber =0 or 2 (15), skip turn and log message
            if (!pile.Count().Equals(0))
            {
                if (pile.getCardNumber() == CardNumber.Two || pile.getCardNumber() == CardNumber.Joker)
                {
                    Console.WriteLine("Pile is a 2, skip turn");
                    passOrPlay = "Pass";
                }
            }
            else
            {
                passOrPlay = "Play";
            }

            //if the turn has gone all the way around, wipe the pile and play
            if (lastPersonToPlay == currentPlayerTurn)
            {
                pile.WipePile();
            }

            //TODO finish implementation of find playable cards
            players[i].FindPlayableCards(pile);

            //check if user can play
            // Type your username and press enter

            if (!string.IsNullOrEmpty(passOrPlay))
            {
                Console.WriteLine("Type 'Pass' or 'Play'");
                // Create a string variable and get user input from the keyboard and store it in the variable
                passOrPlay = Console.ReadLine();

            }
            if ("Play".Equals(passOrPlay))
            {
                //TODO 1. user input for playCards - all must be same CardNumbers or Jokers
                //TODO 2. remove playCards from player hand, and 
                //TODO 3. put playCards on pile 
                //pile.PutCards(playCards);
            }
            else
            {
                //pass, do nothing
            }

            //if the current player has finished their hand, wipe the pile for the next player
            if (players[i].hand.Count() == 0)
            {
                pile.WipePile();
                //add the player to the next game list
                nextGamePlayers.Add(players[i]);
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
                //sets currentPlayerTurn to player with 3 of clubs
                Find3Clubs();

                //check if only 1 player remains
                while (!IsGameFinished())
                {
                    Turn(currentPlayerTurn);

                }

                //add last player to nextgamePlayers list and then swap the lists
                MoveLastPlayer();
                players = nextGamePlayers;

            }
        }

        private void Find3Clubs()
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
                    return;
                }
                i++;

            }
            return;

        }

        private bool IsGameFinished()
        {
            int i = 0;
            foreach (Player p in players)
            {
                if (p.hand.Count() != 0) { i++; }

            }

            //as long as there are 2 players, keep going
            if (i > 1) { return true; } else { return false; }

        }

        private void MoveLastPlayer()
        {

            foreach (Player p in players)
            {
                if (p.hand.Count() != 0)
                {
                    nextGamePlayers.Add(p);
                }
            }
            return;
        }

    }

}
