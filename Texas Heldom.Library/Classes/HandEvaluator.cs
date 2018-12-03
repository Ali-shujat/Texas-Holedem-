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
    public class HandEvaluator : IHandEvaluator
    {
        public List<Card> BestCards = new List<Card>();
        public Hands HandValue;
        public Suits Suit;
        public Values HighCard1 { get; set; }
        private Values HighCard2 { get; set; }
        private List<Values> Kickers { get; set; }
        private List<Card> evalHand = new List<Card>();


        public (List<Card> Cards, Hands HandVaue, Values HighCard, Suits Suit) EvaluateHand(List<Card> cards)
        {
            BestCards.Clear();
            HandValue = new Hands();
            Suit = new Suits();
            HighCard1 = new Values();

            if (cards.Count() > 2 && cards.Count() == 5)
            {
                EvaluateCard(cards);
            }
            if (cards.Count() == 6 )
            {
                EvaluateCard6(cards);
            }
            if (cards.Count() > 6)
            {
                EvaluateCard7(cards);
            }
            return (BestCards, HandValue, HighCard1, Suit);
        }

        #region Metohd for evalute Hands 5, 6, 7

        private void EvaluateCard(List<Card> evalHand)
        {
            #region Arranging cards
            evalHand.Sort();
            var suit1 = evalHand[0].Suit;
            var suit2 = evalHand[1].Suit;
            var suit3 = evalHand[2].Suit;
            var suit4 = evalHand[3].Suit;
            var suit5 = evalHand[4].Suit;
            var value1 = evalHand[0].Value;
            var value2 = evalHand[1].Value;
            var value3 = evalHand[2].Value;
            var value4 = evalHand[3].Value;
            var value5 = evalHand[4].Value;
            #endregion

            #region Has Flush ?
            var hasFlush = evalHand.Count(c => c.Suit.Equals(suit1)).Equals(5);
            if (hasFlush)
            {
                Suit = suit1;
                HandValue = Hands.Flush;
            }
            #endregion

            #region Has Straight ?
            var hasStraight =
                (value2.Equals(value1 + 1) && value3.Equals(value2 + 1) &&
                 value4.Equals(value3 + 1) && value5.Equals(value4 + 1)) ||
                (value1.Equals(Values.Two) && value2.Equals(Values.Three) &&
                value3.Equals(Values.Four) && value4.Equals(Values.Five) &&
                value5.Equals(Values.Ace));

            var isHighStraight = hasStraight && value4.Equals(Values.King) &&
                value5.Equals(Values.Ace); // (10, J, Q, K, A)

            var isLowStraight = hasStraight && value2.Equals(Values.Two) &&
                value5.Equals(Values.Ace); // (A, 2, 3, 4, 5)
            #endregion

            #region Royal Straight Flush
            if (isHighStraight && hasFlush && suit1.Equals(Suits.Hearts))
            {
                HighCard1 = value5;
                HandValue = Hands.RoyalStraightFlush;
                return;
            }
            #endregion

            #region Straight/Straight Flush
            if (hasStraight)
            {
                HighCard1 = isLowStraight ? value4 : value5;
                HandValue = hasFlush ? Hands.StraightFlush : Hands.Straight;
                return;
            }
            #endregion

            #region Flush
            if (hasFlush) return;
            #endregion

            #region Four of a Kind
            if (evalHand.Count(c => c.Value.Equals(value2)).Equals(4) ||
                evalHand.Count(c => c.Value.Equals(value1)).Equals(4))
            {
                HighCard1 = value3;
                HandValue = Hands.FourOfAKind;
                return;
            }
            #endregion

            #region Has Three of a kind ?
            var hasThreeOfAKind =
                    evalHand.Count(c => c.Value.Equals(value1)).Equals(3) ||
                    evalHand.Count(c => c.Value.Equals(value2)).Equals(3) ||
                    evalHand.Count(c => c.Value.Equals(value3)).Equals(3);
            if (hasThreeOfAKind)
            {
                HighCard1 = value3;
                HandValue = Hands.ThreeOfAKind;
            }
            #endregion

            #region Full House
            if (hasThreeOfAKind &&
               (evalHand.Count(c => c.Value.Equals(value1)).Equals(2) ||
                evalHand.Count(c => c.Value.Equals(value5)).Equals(2)))
            {
                HighCard1 = value3; // Three of a kind
                HighCard2 = evalHand.First(c => !c.Value.Equals(value3)).Value; // Pair
                HandValue = Hands.FullHouse;
                return;
            }
            #endregion

            #region Three of a Kind
            if (hasThreeOfAKind) return;
            #endregion

            #region Has Pairs ?
            List<Values> pairs = new List<Values>();
            if (evalHand.Count(c => c.Value.Equals(value1)).Equals(2)) pairs.Add(value1);
            if (evalHand.Count(c => c.Value.Equals(value3)).Equals(2)) pairs.Add(value3);
            if (evalHand.Count(c => c.Value.Equals(value5)).Equals(2)) pairs.Add(value5);
            Kickers = new List<Values>();
            #endregion

            #region Two Pairs
            if (pairs.Count.Equals(2))
            {
                HighCard1 = pairs[1];
                HighCard2 = pairs[0];
                HandValue = Hands.TwoPair;
                Kickers.Add(evalHand.First(c =>
                      !c.Value.Equals(pairs[1]) && !c.Value.Equals(pairs[0])).Value);
                return;
            }
            #endregion

            #region Pair
            if (pairs.Count.Equals(1))
            {
                HighCard1 = pairs[0];
                HandValue = Hands.Pair;
                Kickers.AddRange(
                    evalHand.Where(c => !c.Value.Equals(pairs[0])).Select(c => c.Value).Reverse()); //changed reverse location
            }
            #endregion
            BestCards.AddRange(evalHand);
        }


        private void EvaluateCard7(List<Card> evalHand)
        {
            #region Arranging cards
            evalHand.Sort();
            var suit1 = evalHand[0].Suit; var suit2 = evalHand[1].Suit; var suit3 = evalHand[2].Suit;
            var suit4 = evalHand[3].Suit; var suit5 = evalHand[4].Suit; var suit6 = evalHand[5].Suit;
            var suit7 = evalHand[6].Suit;
            var value1 = evalHand[0].Value; var value2 = evalHand[1].Value; var value3 = evalHand[2].Value;
            var value4 = evalHand[3].Value; var value5 = evalHand[4].Value; var value6 = evalHand[5].Value;
            var value7 = evalHand[6].Value;
            Kickers = new List<Values>();
            #endregion

            #region Has Flush ?
            var hasFlush = evalHand.Count(c => c.Suit.Equals(suit1)).Equals(5) || evalHand.Count(c => c.Suit.Equals(suit2)).Equals(5) || evalHand.Count(c => c.Suit.Equals(suit3)).Equals(5);

            #endregion

            #region Has Straight ?
            var hasStraight =
                (value2.Equals(value1 + 1) && value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1)) ||
                (value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1) && value6.Equals(value5 + 1)) ||
                (value4.Equals(value3 + 1) && value5.Equals(value4 + 1) && value6.Equals(value5 + 1) && value7.Equals(value6 + 1)) ||
                (value1.Equals(Values.Two) && value2.Equals(Values.Three) && value3.Equals(Values.Four) && value4.Equals(Values.Five) && value7.Equals(Values.Ace));

            var isHighStraight = hasStraight && value6.Equals(Values.King) && value7.Equals(Values.Ace); // (@, £, 10, J, Q, K, A)

            var isLowStraight = hasStraight && value2.Equals(Values.Two) && value7.Equals(Values.Ace); // (2, 3, 4, 5, @, £, A)
            #endregion

            #region Royal Straight Flush
            if (isHighStraight && hasFlush && suit3.Equals(Suits.Hearts))
            {
                HighCard1 = value7;
                HandValue = Hands.RoyalStraightFlush;
                return;
            }
            #endregion

            #region Straight/Straight Flush
            if (hasStraight)
            {
                HighCard1 = isLowStraight ? value4 : value7;
                HandValue = hasFlush ? Hands.StraightFlush : Hands.Straight;
                if (HandValue == Hands.Straight)
                {
                    if (value2.Equals(value1 + 1) && value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1))
                    {
                        HighCard1 = value5; BestCards.AddRange(from c in evalHand
                                                               where c.Value > value5
                                                               orderby c.Value
                                                               select c);
                    }
                    if (value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1) && value6.Equals(value5 + 1))
                    {
                        HighCard1 = value6; BestCards.AddRange(from c in evalHand
                                                               where c.Value.Equals(value1) && c.Value.Equals(value7)
                                                               orderby c.Value
                                                               select c);
                    }
                    if (value4.Equals(value3 + 1) && value5.Equals(value4 + 1) && value6.Equals(value5 + 1) && value7.Equals(value6 + 1))
                    {
                        HighCard1 = value7; BestCards.AddRange(from c in evalHand
                                                               where c.Value < value3 orderby c.Value
                                                               select c);
                    }
                }
                return;
            }
            #endregion

            #region Flush
            if (hasFlush)
            {
                if (evalHand.Count(c => c.Suit.Equals(suit1)).Equals(5))
                {
                    Suit = suit1; HandValue = Hands.Flush; BestCards.AddRange(from c in evalHand
                                                                              where !c.Suit.Equals(suit1)
                                                                              orderby c.Value
                                                                              select c);
                }
                if (evalHand.Count(c => c.Suit.Equals(suit2)).Equals(5))
                {
                    Suit = suit2; HandValue = Hands.Flush; BestCards.AddRange(from c in evalHand
                                                                              where !c.Suit.Equals(suit2)
                                                                              orderby c.Value
                                                                              select c);
                }
                if (evalHand.Count(c => c.Suit.Equals(suit3)).Equals(5))
                {
                    Suit = suit3; HandValue = Hands.Flush; BestCards.AddRange(from c in evalHand
                                                                              where !c.Suit.Equals(suit3)
                                                                              orderby c.Value
                                                                              select c);
                }
                return;
            }
            #endregion

            #region Four of a Kind
            if (evalHand.Count(c => c.Value.Equals(value4)).Equals(4))
            {
                HighCard1 = value4;
                HandValue = Hands.FourOfAKind;
                //new 
                BestCards.AddRange(from c in evalHand
                                   where !c.Value.Equals(value4)
                                   orderby c.Value
                                   select c);

                return;
            }
            #endregion

            #region Has Three of a kind ?
            var hasThreeOfAKind =
                    evalHand.Count(c => c.Value.Equals(value3)).Equals(3) ||
                    evalHand.Count(c => c.Value.Equals(value5)).Equals(3) 
                    //&&                    evalHand.Count(c => !c.Value.Equals(value4)).Equals(4)
                    ;
            #endregion
            #region Three of a Kind
            if (hasThreeOfAKind)
            {
                if (evalHand.Count(c => c.Value.Equals(value3)).Equals(3))
                {
                    HighCard1 = value3; BestCards.AddRange(from c in evalHand
                                                           where !c.Value.Equals(value3)
                                                           orderby c.Value
                                                           select c);
                }
                if (evalHand.Count(c => c.Value.Equals(value5)).Equals(3))
                {
                    HighCard1 = value5; BestCards.AddRange(from c in evalHand
                                                           where !c.Value.Equals(value5)
                                                           orderby c.Value
                                                           select c);
                }
                HandValue = Hands.ThreeOfAKind;
               
            }
            #endregion

            #region Full House
            if (hasThreeOfAKind &&
               (evalHand.Count(c => c.Value.Equals(value2)).Equals(2) ||
                evalHand.Count(c => c.Value.Equals(value4)).Equals(2) || evalHand.Count(c => c.Value.Equals(value6)).Equals(2)))
            {
                HandValue = Hands.FullHouse;
                if (hasThreeOfAKind && evalHand.Count(c => c.Value.Equals(value2)).Equals(2) && !evalHand.Count(c => c.Value.Equals(value2)).Equals(3))
                {
                    HighCard1 = value2; BestCards.AddRange(from c in evalHand
                                                           where c.Value.Equals(value2)
                                                           select c);
                }
                if (hasThreeOfAKind && evalHand.Count(c => c.Value.Equals(value4)).Equals(2) && !evalHand.Count(c => c.Value.Equals(value4)).Equals(3))
                {
                    HighCard1 = value4; BestCards.AddRange(from c in evalHand
                                                           where c.Value.Equals(value4) 
                                                           select c);
                }
                if (hasThreeOfAKind && evalHand.Count(c => c.Value.Equals(value6)).Equals(2) && evalHand.Count(c => c.Value.Equals(value6)).Equals(3))
                {
                    HighCard1 = value6; BestCards.AddRange(from c in evalHand
                                                           where c.Value.Equals(value6) 
                                                           select c);
                }
                //HighCard2 = evalHand.First(c => !c.Value.Equals(value5)).Value; // Pair //didnot calculate yet
                return;
            }
            #endregion

            #region Has Pairs ?
            List<Values> pairs = new List<Values>();
            if (evalHand.Count(c => c.Value.Equals(value1)).Equals(2)) pairs.Add(value1);
            if (evalHand.Count(c => c.Value.Equals(value3)).Equals(2)) pairs.Add(value3);
            if (evalHand.Count(c => c.Value.Equals(value5)).Equals(2)) pairs.Add(value5);
            if (evalHand.Count(c => c.Value.Equals(value7)).Equals(2)) pairs.Add(value7);

            #endregion

            #region Three Pairs
            if (pairs.Count.Equals(3))
            {
                HighCard1 = pairs[2];
                HighCard2 = pairs[1];
                HandValue = Hands.TwoPair;
                //// new 
                
                BestCards.AddRange(from c in evalHand
                                   where c.Value.Equals(pairs[1])
                                   orderby c.Value
                                   select c);
                Kickers.Add(evalHand.First(c =>
                      !c.Value.Equals(pairs[0]) && !c.Value.Equals(pairs[1]) && !c.Value.Equals(pairs[2])).Value);
                return;
            }
            #endregion
            #region Two Pairs
            if (pairs.Count.Equals(2))
            {
                HighCard1 = pairs[1];
                HighCard2 = pairs[0];
                HandValue = Hands.TwoPair;
                Kickers.Add(evalHand.First(c =>
                      !c.Value.Equals(pairs[1]) && !c.Value.Equals(pairs[0])).Value);
                //new 
                BestCards.AddRange(from c in evalHand
                                   where c.Value.Equals(pairs[0]) 
                                   orderby c.Value
                                   select c);
                return;
            }
            #endregion

            #region Pair
            if (pairs.Count.Equals(1))
            {
                HighCard1 = pairs[0];
                HandValue = Hands.Pair;
                Kickers.AddRange(
                    evalHand.Where(c => !c.Value.Equals(pairs[0])).Select(c => c.Value).Reverse()); //changed reverse location
                BestCards.AddRange(from c in evalHand
                                   where !c.Value.Equals(pairs[0])
                                   orderby c.Value
                                   select c);
                return;
            }
            #endregion
            
        }


        private void EvaluateCard6(List<Card> evalHand)
        {
            #region Arranging cards
            evalHand.Sort();
            var suit1 = evalHand[0].Suit; var suit2 = evalHand[1].Suit; var suit3 = evalHand[2].Suit;
            var suit4 = evalHand[3].Suit; var suit5 = evalHand[4].Suit; var suit6 = evalHand[5].Suit;

            var value1 = evalHand[0].Value; var value2 = evalHand[1].Value; var value3 = evalHand[2].Value;
            var value4 = evalHand[3].Value; var value5 = evalHand[4].Value; var value6 = evalHand[5].Value;

            Kickers = new List<Values>();
            #endregion

            #region Has Flush ?
            var hasFlush = evalHand.Count(c => c.Suit.Equals(suit1)).Equals(5) ||
                           evalHand.Count(c => c.Suit.Equals(suit2)).Equals(5);

            #endregion

            #region Has Straight ?
            var hasStraight =
                (value2.Equals(value1 + 1) && value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1)) ||
                (value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1) && value6.Equals(value5 + 1)) ||
                (value1.Equals(Values.Two) && value2.Equals(Values.Three) && value3.Equals(Values.Four) && value4.Equals(Values.Five) && value6.Equals(Values.Ace));

            var isHighStraight = hasStraight && value6.Equals(Values.King) && value6.Equals(Values.Ace); // (@, £, 10, J, Q, K, A)

            var isLowStraight = hasStraight && value2.Equals(Values.Two) && value6.Equals(Values.Ace); // (2, 3, 4, 5, @, £, A)
            #endregion

            #region Royal Straight Flush
            if (isHighStraight && hasFlush && suit3.Equals(Suits.Hearts))
            {
                HighCard1 = value6;
                HandValue = Hands.RoyalStraightFlush;
                return;
            }
            #endregion

            #region Straight/Straight Flush
            if (hasStraight)
            {
                HighCard1 = isLowStraight ? value4 : value6;
                HandValue = hasFlush ? Hands.StraightFlush : Hands.Straight;
                if (HandValue == Hands.Straight)
                {
                    if (value2.Equals(value1 + 1) && value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1))
                    {
                        HighCard1 = value5;
                        BestCards.AddRange(from c in evalHand
                                           where c.Value > value5
                                           orderby c.Value
                                           select c);
                    }
                    if (value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1) && value6.Equals(value5 + 1))
                    {
                        HighCard1 = value6;
                        BestCards.AddRange(from c in evalHand
                                           where c.Value.Equals(value1) && c.Value.Equals(value6)
                                           orderby c.Value
                                           select c);
                    }
                    //if (value4.Equals(value3 + 1) && value5.Equals(value4 + 1) && value6.Equals(value5 + 1) && value7.Equals(value6 + 1))
                    //{
                    //    HighCard1 = value7;
                    //    //BestCards.AddRange(from c in evalHand   where c.Value < value3
                    //    //                                       orderby c.Value   select c);
                    //}
                }
                return;
            }
            #endregion

            #region Flush
            if (hasFlush)
            {
                if (evalHand.Count(c => c.Suit.Equals(suit1)).Equals(5))
                {
                    Suit = suit1; HandValue = Hands.Flush; BestCards.AddRange(from c in evalHand
                                                                              where !c.Suit.Equals(suit1)
                                                                              orderby c.Value
                                                                              select c);
                }
                if (evalHand.Count(c => c.Suit.Equals(suit2)).Equals(5))
                {
                    Suit = suit2; HandValue = Hands.Flush; BestCards.AddRange(from c in evalHand
                                                                              where !c.Suit.Equals(suit2)
                                                                              orderby c.Value
                                                                              select c);
                }
                //if (evalHand.Count(c => c.Suit.Equals(suit3)).Equals(5))
                //{
                //    Suit = suit3; HandValue = Hands.Flush; BestCards.AddRange(from c in evalHand
                //                                                              where !c.Suit.Equals(suit3)
                //                                                              orderby c.Value
                //                                                              select c);
                //}
                return;
            }
            #endregion

            #region Four of a Kind
            if (evalHand.Count(c => c.Value.Equals(value4)).Equals(4))
            {
                HighCard1 = value4;
                HandValue = Hands.FourOfAKind;
                //new 
                BestCards.AddRange(from c in evalHand
                                   where !c.Value.Equals(value4)
                                   orderby c.Value
                                   select c);

                return;
            }
            #endregion

            #region Has Three of a kind ?
            var hasThreeOfAKind =
                    evalHand.Count(c => c.Value.Equals(value3)).Equals(3) ||
                    evalHand.Count(c => c.Value.Equals(value5)).Equals(3) &&
                    evalHand.Count(c => !c.Value.Equals(value4)).Equals(4);
            #endregion
            #region Three of a Kind
            if (hasThreeOfAKind)
            {
                if (evalHand.Count(c => c.Value.Equals(value3)).Equals(3))
                {
                    HighCard1 = value3; BestCards.AddRange(from c in evalHand
                                                           where !c.Value.Equals(value3)
                                                           orderby c.Value
                                                           select c);
                }
                if (evalHand.Count(c => c.Value.Equals(value5)).Equals(3))
                {
                    HighCard1 = value5; BestCards.AddRange(from c in evalHand
                                                           where !c.Value.Equals(value5)
                                                           orderby c.Value
                                                           select c);
                }
                HandValue = Hands.ThreeOfAKind;
                return;
            }
            #endregion

            #region Full House
            if (hasThreeOfAKind &&
               (evalHand.Count(c => c.Value.Equals(value2)).Equals(2) ||
                evalHand.Count(c => c.Value.Equals(value4)).Equals(2) || evalHand.Count(c => c.Value.Equals(value6)).Equals(2)))
            {
                HandValue = Hands.FullHouse;
                if (hasThreeOfAKind && evalHand.Count(c => c.Value.Equals(value2)).Equals(2))
                {
                    HighCard1 = value2; BestCards.AddRange(from c in evalHand
                                                           where !c.Value.Equals(value2) && !c.Value.Equals(value5)
                                                           orderby c.Value
                                                           select c);
                }
                if (hasThreeOfAKind && evalHand.Count(c => c.Value.Equals(value4)).Equals(2))
                {
                    HighCard1 = value4; BestCards.AddRange(from c in evalHand
                                                           where !c.Value.Equals(value4) && !c.Value.Equals(value5)
                                                           orderby c.Value
                                                           select c);
                }
                if (hasThreeOfAKind && evalHand.Count(c => c.Value.Equals(value6)).Equals(2))
                {
                    HighCard1 = value6; BestCards.AddRange(from c in evalHand
                                                           where !c.Value.Equals(value6) && !c.Value.Equals(value3)
                                                           orderby c.Value
                                                           select c);
                }
                HighCard2 = evalHand.First(c => !c.Value.Equals(value5)).Value; // Pair //didnot calculate yet
                return;
            }
            #endregion

            #region Has Pairs ?
            List<Values> pairs = new List<Values>();
            if (evalHand.Count(c => c.Value.Equals(value1)).Equals(2)) pairs.Add(value1);
            if (evalHand.Count(c => c.Value.Equals(value3)).Equals(2)) pairs.Add(value3);
            if (evalHand.Count(c => c.Value.Equals(value5)).Equals(2)) pairs.Add(value5);
            //if (evalHand.Count(c => c.Value.Equals(value7)).Equals(2)) pairs.Add(value7);

            #endregion

            #region Three Pairs
            if (pairs.Count.Equals(3))
            {
                HighCard1 = pairs[2];
                HighCard2 = pairs[1];
                HandValue = Hands.TwoPair;
                //// new 
                BestCards.AddRange(from c in evalHand
                                   where !c.Value.Equals(pairs[0]) && !c.Value.Equals(pairs[1]) && !c.Value.Equals(pairs[2])
                                   orderby c.Value
                                   select c);
                return;
            }
            #endregion
            #region Two Pairs
            if (pairs.Count.Equals(2))
            {
                HighCard1 = pairs[1];
                HighCard2 = pairs[0];
                HandValue = Hands.TwoPair;
                Kickers.Add(evalHand.First(c =>
                      !c.Value.Equals(pairs[1]) && !c.Value.Equals(pairs[0])).Value);
                //new 
                BestCards.AddRange(from c in evalHand
                                   where !c.Value.Equals(pairs[0]) && !c.Value.Equals(pairs[1])
                                   orderby c.Value
                                   select c);
                return;
            }
            #endregion

            #region Pair
            if (pairs.Count.Equals(1))
            {
                HighCard1 = pairs[0];
                HandValue = Hands.Pair;
                Kickers.AddRange(
                    evalHand.Where(c => !c.Value.Equals(pairs[0])).Select(c => c.Value).Reverse()); //changed reverse location
                BestCards.AddRange(from c in evalHand
                                   where !c.Value.Equals(pairs[0])
                                   orderby c.Value
                                   select c);
            }
            #endregion
            //method end
        }

        #endregion

        //// Class end
    }




    /// namespace end 
}

