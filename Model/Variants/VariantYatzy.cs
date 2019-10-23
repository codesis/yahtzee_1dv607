using System;

namespace yahtzee_1dv607.Model.Variants
{
    class VariantYatzy : Variant
    {
        new public enum Type
        {
            Aces = 0, 
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

        public override int Length() { return Enum.GetNames(typeof(Type)).Length; }

        public override Variant.Type[] Variants() { return (Variant.Type[])Enum.GetValues(typeof(Type)); }

        public override string[] GetNames() { return Enum.GetNames(typeof(Type)); }

        public override string GetName(Variant.Type i) { return Enum.GetName(typeof(Type), i); }

        public override string GetName(int i) { return Enum.GetName(typeof(Type), i); }

        public override string GetName(object i) { return Enum.GetName(typeof(Type), i); }

        public override Array GetValues() { return Enum.GetValues(typeof(Type)); }

        public override int GetValue(Variant.Type type) { return (int)type; }

        public override int GetValue(object type) { return (int)type; }

        public override Variant.Type GetVariant(int i) { return (Variant.Type)i; }

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

        public override Variant.Type Sixes()
        {
            return (Variant.Type)Type.Sixes;
        }
    }
}
