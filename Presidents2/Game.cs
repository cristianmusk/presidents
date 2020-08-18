using Cards.Domain.Standard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presidents2
{
    public class Game
    {
        public List<Player> players;
        public List<Player> nextGamePlayers;

        //play pile. Pils is changed after each play
        public Pile pile;

        public int lastPersonToPlay = -1;
        public int currentPlayerTurn = -1;
        public Deck deck = new Deck();
        private static string PASS = "PASS";
        private static string PLAY = "PLAY";
        private Boolean isFirstHand = true;

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

        public void SetupAndDeal()
        {
            nextGamePlayers.Clear();
            isFirstHand = true;
            deck.Reset();
            deck.Shuffle();

            Console.WriteLine("TABLE ORDER : ");
            int a = 0;
            foreach (Player p in players)
            {
                Console.WriteLine(a + ": " + p.name);
                a++;
            }

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

                //Console.WriteLine(players[i].hand.Last().ToString());
                //if last player, then restart with the first player. change this to Game.NextPlayer or seat arrangement
                i++;
                if (i == (players.Count)) { i = 0; }
            }
            Console.WriteLine("Dealing finished");

            OrderHands();
        }

        private void OrderHands()
        {
            foreach (Player p in players)
            {
                p.hand = p.hand.OrderBy(o => o.CardNumber).ToList();
            }
        }

        private string TakeInput(string message, string[] validValues)
        {
            string userInput = "";

            while (!validValues.Contains(userInput))
            {
                Console.WriteLine(message);
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        //TODO convert error scenarios into exceptions
        //TODO WRITE UNIT TESTS - input for mixed cards or less than required card count (pile.Count())
        public List<Card> ValidateAndConvertCardInput(string s, List<Card> playableCards)
        {
            List<Card> cardInput = new List<Card>();

            string[] strArrayCardInput = s.Split(',');
            //if no input, invalid
            if (strArrayCardInput.Length == 0)
            {
                //invalid
                return null;
            }
            else
            {
                int[] intArrayCardInput = new int[strArrayCardInput.Length];

                //parse and validate input
                for (int i = 0; i < strArrayCardInput.Length; i++)
                {
                    strArrayCardInput[i] = strArrayCardInput[i].Trim();
                    intArrayCardInput[i] = Int32.Parse(strArrayCardInput[i]);
                    //out of bounds
                    if (intArrayCardInput[i] < 0 || intArrayCardInput[i] >= playableCards.Count())
                    {
                        //invalid
                        return null;
                    }
                }

                //dedup
                int[] intArrayDedupCardInput = intArrayCardInput.Distinct().ToArray();

                for (int i = 0; i < intArrayDedupCardInput.Length; i++)
                {
                    cardInput.Add(playableCards[intArrayDedupCardInput[i]]);
                }
                //validate they are all the same cards other than jokers
                var query = cardInput
                  .Where(n => n.CardNumber != CardNumber.Joker)
                    .GroupBy(l => l.CardNumber)
                  .Select(g => new
                  {
                      CardNumber = g.Key,
                      Count = g.Select(l => l.CardNumber).Distinct().Count()
                  });

                if (query.Count() > 1)
                {
                    //invalid entry, can only play cards of the same type
                    return null;
                }

                //validate that enough cards are being played
                if (cardInput.Count() < pile.Count())
                {
                    return null;
                }

                //if this is the first hand of a game, the first player must be playing the 3 of clubs
                if (isFirstHand == true && !Find3Clubs(cardInput))
                {
                    return null;
                }
            }

            return cardInput;
        }

        public void Turn(int i)
        {
            //if the current player is already out, go to the next player turn
            if (players[i].hand.Count() == 0) { IncrementPlayerTurn(); return; }

            //if the turn has gone all the way around, wipe the pile and play
            if (lastPersonToPlay == currentPlayerTurn)
            {
                Console.WriteLine("Wipe the pile");
                pile.WipePile();
            }

            string passOrPlay = null;
            //if not empty and pile.cardnumber =0 or 2 (15), skip turn and log message
            if (!pile.Count().Equals(0))
            {
                if (pile.getCardNumber() == CardNumber.Two || pile.getCardNumber() == CardNumber.Joker)
                {
                    Console.WriteLine("Pile is a 2, skip turn");
                    passOrPlay = PASS;
                }
            }
            else
            {
                //if empty pile, play
                passOrPlay = PLAY;
            }

            //Find playable cards
            List<Card> playableCards = players[i].FindPlayableCards(isFirstHand, pile);

            Console.WriteLine("**************************************" + players[i].name + "'S HAND *************************************");
            PrintCards(players[i].hand, false);

            Console.WriteLine("************************************** PLAYABLE CARDS: *************************************");
            PrintCards(playableCards, true);
            //int z = 0;
            //foreach (Card c in playableCards)
            //{
            //    Console.WriteLine(z + ": " + c.ToString());
            //    z++;
            //}

            Console.WriteLine(players[i].name + " HAS " + playableCards.Count() + " PLAYABLE CARDS OUT OF " + players[i].hand.Count() + " IN HAND");
            if (pile.Count() == 0)
            {
                Console.WriteLine("PILE IS EMPTY");
            }
            else
            {
                Console.WriteLine("CURRENT PILE IS " + pile.Count() + "x " + pile.getCardNumber() + "'s");
            }

            if (!String.IsNullOrEmpty(passOrPlay))
            {
                //do nothing, PASS or PLAY is already set
            }
            //check if user can only pass
            else if (playableCards != null && playableCards.Count() == 0)
            {
                passOrPlay = TakeInput("You can only PASS. Type 'PASS'", new string[] { PASS });
            }

            //If player hasn't passed
            if (!PASS.Equals(passOrPlay))
            {
                //1. user input for playCards - all must be same CardNumbers or Jokers
                string userPlayAction = null;
                bool isValidInput = false;
                List<Card> playedCards = new List<Card>();
                while (!isValidInput)
                {
                    Console.WriteLine("Enter all card indexes to play, comma separate or type 'PASS' ");
                    userPlayAction = Console.ReadLine();
                    if (!PASS.Equals(userPlayAction.ToUpper()))
                    {
                        playedCards = ValidateAndConvertCardInput(userPlayAction, playableCards);
                        if (playedCards != null) { isValidInput = true; }
                    }
                    else if(isFirstHand) {
                        Console.WriteLine("CANNOT PASS WITH 3 OF CLUBS");
                    }
                    else {
                        isValidInput = true;
                    }
                }

                if (!PASS.Equals(userPlayAction.ToUpper()))
                {
                    //2. remove playCards from player hand, and
                    lastPersonToPlay = i;
                    players[i].hand.RemoveAll(x => playedCards.Contains(x));
                    //3. put playCards on pile
                    pile.PutCards(playedCards);
                }

            }


            //if the current player has finished their hand, wipe the pile for the next player
            if (players[i].hand.Count() == 0)
            {
                pile.WipePile();
                //add the player to the next game list
                nextGamePlayers.Add(players[i]);
                Console.WriteLine("&&&&&&&&&&&&&&&&&&&      " + players[i].name + " HAS GONE OUT      &&&&&&&&&&&&&&&&&&& ");
            }

            //increment player turn
            IncrementPlayerTurn();
            isFirstHand = false;
        }

        public void StartGame()
        {

            //Add players
            string playerName = null;
            while (playerName != "DONE")
            {
                Console.WriteLine("ENTER A PLAYER NAME OR 'DONE'");
                playerName = Console.ReadLine();
                if (!"DONE".Equals(playerName))
                {
                    AddPlayer(playerName);
                }
            }

            Boolean isFirstGame = true;
            Boolean keepGoing = true;

            while (keepGoing)
            {
                //setup game
                SetupAndDeal();

                //if this is the first game, there is no trading cards
                //UNCOMMENTED FOR DEBUGGING ONLY 
                //isFirstGame = false;
                if (!isFirstGame)
                {
                    //first place (PRESIDENT) requests 2 cards from last player (ASSHOLE)
                    RequestCards(2, 0, players.Count() - 1);
                    GiveCards(2, 0, players.Count() - 1);
                    //only if there are 4 or more players
                    //second place (VICE PRESIDENT) requests 1 card from 2nd to last player (JERK)
                    if (players.Count() >= 4)
                    {
                        RequestCards(1, 1, players.Count() - 2);
                        GiveCards(1, 1, players.Count() - 2);
                    }
                    //there has been a change of cards, order the hand again
                    OrderHands();
                }

                isFirstGame = false;

                //sets currentPlayerTurn to player with 3 of clubs
                for (int i = 0; i < players.Count(); i++)
                {
                    if (Find3Clubs(players[i].hand))
                    {
                        currentPlayerTurn = i;
                    }
                }

                //check if only 1 player remains
                while (!IsGameFinished())
                {
                    Turn(currentPlayerTurn);
                }
                Console.WriteLine("Game finished");
                //add last player to nextgamePlayers list and then swap the lists
                MoveLastPlayer();
                players = new List<Player>(nextGamePlayers);


            }
        }

        private void PrintCards(List<Card> cards, Boolean showIndex)
        {

            for (int i = 0; i < cards.Count(); i++)
            {
                if (showIndex)
                {
                    Console.WriteLine(i + ": " + cards[i].ToString());
                }
                else
                {
                    Console.WriteLine(cards[i].ToString());
                }


            }

        }

        private void GiveCards(int numberOfCards, int top, int bottom)
        {
            while (numberOfCards > 0)
            {
                Console.WriteLine("HAND FOR " + players[top].name + ". GIVE AWAY " + numberOfCards + "CARD");
                int z = 0;
                foreach (Card c in players[top].hand)
                {
                    Console.WriteLine(z + ": " + c.ToString());
                    z++;
                }
                Console.WriteLine("ENTER CODE FOR A CARD TO GIVE");
                int userInput = Int32.Parse(Console.ReadLine());

                //TODO USE TRYPARSE AND VALIDATE IT IS WITHIN THE BOUNDS
                Card cardInput = new Card();
                players[bottom].hand.Add(players[top].hand[userInput]);
                players[top].hand.RemoveAt(userInput);
                numberOfCards--;
            }
        }

        private void RequestCards(int numberOfCards, int top, int bottom)
        {
            Console.WriteLine(players[top].name + "'S HAND");
            PrintCards(players[top].hand, false);

            Console.WriteLine(players[top].name + " GETS " + numberOfCards + " CARDS FROM " + players[bottom].name);
            Deck deck = new Deck();
            deck.ShowCardOptions();
            Card tradeCard = new Card();
            int intUserInput = -1;
            string strUserInput = null;
            Boolean foundCard = false;
            while (numberOfCards > 0)
            {
                foundCard = false;
                Console.WriteLine("ENTER CODE OF A REQUESTED CARD");

                strUserInput = Console.ReadLine().ToUpper();
                if ("3C".Equals(strUserInput))
                {
                    Card threeOfClubs = new Card(CardNumber.Three, Suit.Club);
                    //foundCard = players[bottom].hand.Find(x => x.CardNumber == threeOfClubs.CardNumber && x.Suit == threeOfClubs.Suit);

                    var query = from c in players[bottom].hand
                                where c == threeOfClubs
                                select c;

                    if (query.Count() > 0)
                    {
                        foundCard = true;
                        tradeCard = query.First();
                    }
                }
                else
                {
                    intUserInput = Int32.Parse(strUserInput);
                    CardNumber cn = (CardNumber)intUserInput;
                    //foundCard = players[bottom].hand.Find(x => x.CardNumber == cn);

                    var query = from c in players[bottom].hand
                                where c.CardNumber == cn
                                select c;

                    if (query.Count() > 0)
                    {
                        foundCard = true;
                        tradeCard = query.First();
                    }
                }

                if (foundCard)
                {
                    players[top].hand.Add(tradeCard);
                    players[bottom].hand.Remove(tradeCard);
                    Console.WriteLine(players[top].name + " TOOK THE " + tradeCard.ToString() + " FROM " + players[bottom].name);
                    numberOfCards--;
                }
                else
                {
                    Console.WriteLine("CARD NOT FOUND");
                }
            }
        }

        private void IncrementPlayerTurn()
        {
            currentPlayerTurn++;
            //if its gone around, reset to the beginning
            if (currentPlayerTurn == players.Count()) { currentPlayerTurn = 0; }
        }

        //private void Find3Clubs()
        //{
        //    int i = 0;

        //    Card threeOfClubs = new Card(CardNumber.Three, Suit.Club);
        //    foreach (Player p in players)
        //    {
        //        //check for 3 of clubs here to save off who will start the game, rather than search all players hands later.
        //        if (p.hand.Exists(t =>
        //             (t.CardNumber == threeOfClubs.CardNumber && t.Suit == threeOfClubs.Suit)))
        //        {
        //            currentPlayerTurn = i;
        //            return;
        //        }
        //        i++;
        //    }
        //    return;
        //}

        private Boolean Find3Clubs(List<Card> cards)
        {
            Card threeOfClubs = new Card(CardNumber.Three, Suit.Club);
            //check for 3 of clubs
            if (cards.Exists(t =>
                 (t.CardNumber == threeOfClubs.CardNumber && t.Suit == threeOfClubs.Suit)))
            {
                return true;
            }

            return false;
        }

        private bool IsGameFinished()
        {
            int i = 0;
            foreach (Player p in players)
            {
                if (p.hand.Count() != 0) { i++; }
            }

            //if there is only 1 player with cards, game is finished
            if (i == 1) { return true; } else { return false; }
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