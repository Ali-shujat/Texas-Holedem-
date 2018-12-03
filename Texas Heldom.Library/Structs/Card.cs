
using System;
using Texas_Heldom.Library.Enums;


namespace Texas_Heldom.Library.Structs
{
    public struct Card : IComparable
    {
        public Values Value { get; }
        public Suits Suit { get; }
        public string Output {
            get {
                var value = (int)Value <= 10 ? ((int)Value).ToString() : Value.ToString().Substring(0, 1);
                return $"{value} \n {(char)Suit}";
                //Enum.Parse(typeof(Suits), Suit.ToString())}
        }
        }
        //(char)(Symbol)Enum.Parse(typeof(Symbol), _suit.ToString());
        public Card(Values value, Suits suit)
        {
            Value = value;
            Suit = suit;
        }

        public int CompareTo(object obj)
        {
            var obj1 = (int)Value;
            var obj2 = (int)((Card)obj).Value;
            return obj1.CompareTo(obj2);
        }
    }
}
