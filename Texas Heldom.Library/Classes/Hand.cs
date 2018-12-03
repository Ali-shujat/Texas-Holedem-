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
    public class Hand
    {
        public List<Card> Cards { get; } = new List<Card>();
        public List<Card> BestCards { get; set; } = new List<Card>();
        public List<Card> PlayerCards { get; } = new List<Card>();
        private IHandEvaluator _eval { get; set; }
        public Values HighCard1 { get; set; }
        public Hands HandValue { get; private set; }
        public Suits Suit { get; private set; }

        public Hand(IHandEvaluator eval)
        {
            _eval = eval;
        }

        //public Hand(IHandEvaluator eval) => eval = _eval;

        public void Clear()        {

            HandValue = new Hands();
            Suit = new Suits();
            Cards.Clear();
            BestCards.Clear();
            PlayerCards.Clear();
        }

        public void AddCard(Card card, bool isPlayerCard)
        {
            if (isPlayerCard && PlayerCards.Count < 2)
            {
                Cards.Add(card);
            }
            else
            {
                Cards.Add(card);
            }
        }

        public void EvaluateHand()
        {
            if (Cards.Count <= 2)
                return;

            //var resultat;
            (BestCards, HandValue,HighCard1, Suit )=_eval.EvaluateHand(Cards);
            //BestCards.AddRange(resultat.Cards);
            //HandValue = resultat.HandVaue;
            //Suit = resultat.Suit;

        }
        

       


        

    }
}
