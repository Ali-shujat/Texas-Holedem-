using System.Collections.Generic;
using Texas_Heldom.Library.Enums;
using Texas_Heldom.Library.Structs;

namespace Texas_Heldom.Library.Interfaces
{
    public interface IHandEvaluator
    {
        (List<Card> Cards, Hands HandVaue,Values HighCard, Suits Suit) EvaluateHand(List<Card> cards);
        
    }
}
