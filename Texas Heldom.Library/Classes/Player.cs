using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Heldom.Library.Enums;
using Texas_Heldom.Library.Interfaces;
using Texas_Heldom.Library.Structs;

namespace Texas_Heldom.Library.Classes
{
    public class Player
    {
        private Hand _hand;
        public Values HighCard1 => _hand.HighCard1;
        public string Name { get; }
        public List<Card> Cards => _hand.Cards;
        public List<Card> BestCards => _hand.Cards;
        public List<Card> PlayerCards => _hand.Cards;
        public Hands HandValue => _hand.HandValue;
        public Suits Suit => _hand.Suit;
        public int CardCount => Cards.Count();

        public Player(string name)
        {
            Name = name;
            _hand = new Hand(new HandEvaluator());
        }

        public void ReceivedCard(Card card, bool isPlayerCard = false)
        {
            _hand.AddCard(card,isPlayerCard);
        }

        public void ClearHand()
        {
            _hand.Clear();
        }

        public void EvaluteHand()
        {
            _hand.EvaluateHand();
        }

    }
}
