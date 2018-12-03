using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Texas_Heldom.Library.Classes
{
    public class Table
    {
       public Deck _deck;
       public List<Player> Players { get; } = new List<Player>();
       public Player Dealer { get; } = new Player("Dealer");

        public Table(string[] PlayerNames)
        {
            
            if (PlayerNames == null && PlayerNames.Length < 2 && PlayerNames.Length > 4)
            {
                throw new ArgumentException("Incorrect number of players");
            }
            else
                foreach (string name in PlayerNames)
                { Players.Add(new Player(name)); }
                   
        }

       public void DealNewHand()
        {
            _deck = new Deck();
            _deck.ShuffleDack(111);
            Dealer.ClearHand();
            DealPlayersCards();
        }
        private void DealPlayersCards()
        {
            foreach (Player player in Players)
            {
                player.ClearHand();
                for (int i = 0; i < 2; i++)
                    player.ReceivedCard(_deck.DrawCard(), true);

            }
        }

        public void evaluteHand()
        {
            foreach (var item in Players)
            {
                item.EvaluteHand();
            }
            if (Dealer.Cards.Count() ==5)
            {
                Dealer.EvaluteHand();
            }
        }

        public void DelerDrawsCard(int count = 1)
        {

            if (Dealer.CardCount > 5)
                return;
            for (int i = 0; i < count; i++)
            {
                var card = _deck.DrawCard();
                Dealer.ReceivedCard(card);
                foreach (var players in Players)
                { players.ReceivedCard(card); }
            }

        }
    }
}
