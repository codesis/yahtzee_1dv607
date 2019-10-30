using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yahtzee_1dv607.Model.Variants
{
    class VariantYatzy : Variant
    {
        new public enum Type
        {
            Aces, 
            Twos, 
            Threes, 
            Fours, 
            Fives, 
            Sixes, 
            Pair, 
            TwoPair, 
            ThreeOfAKind, 
            FourOfAKind, 
            FullHouse, 
            SmallStraight, 
            LargeStraight, 
            Yahtzee, 
            Chance
        }

        public override int Length() 
        { 
            return Enum.GetNames(typeof(Type)).Length; 
        }

        public override string GetName(Variant.Type i) 
        { 
            return Enum.GetName(typeof(Type), i); 
        }

        public override string GetName(int i) 
        { 
            return Enum.GetName(typeof(Type), i); 
        }

        public override Array GetList() 
        { 
            return Enum.GetValues(typeof(Type)); 
        }

        public override int GetValue(Variant.Type type) 
        { 
            return (int)type; 
        }

        public override Variant.Type GetVariant(int i) 
        { 
            return (Variant.Type)i; 
        }

        public override Variant.Type Yahtzee()
        {
            return (Variant.Type)Type.Yahtzee;
        }

        public override Variant.Type SmallStraight()
        {
            return (Variant.Type)Type.SmallStraight;
        }

        public override Variant.Type LargeStraight()
        {
            return (Variant.Type)Type.LargeStraight;
        }

        public override Variant.Type FullHouse()
        {
            return (Variant.Type)Type.FullHouse;
        }

        public override Variant.Type Chance()
        {
            return (Variant.Type)Type.Chance;
        }

        public override Variant.Type Threes()
        {
            return (Variant.Type)Type.Threes;
        }

    }
}
