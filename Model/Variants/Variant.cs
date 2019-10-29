using System;

namespace yahtzee_1dv607.Model.Variants
{
    public abstract class Variant
    {
        public enum Type { }

        abstract public string GetName(Type type);

        abstract public string GetName(object type);

        abstract public string GetName(int index);

        abstract public string[] GetNames();

        abstract public Array GetList();

        abstract public int Length();

        abstract public Variant.Type[] Variants();

        abstract public int GetValue(Type type);

        abstract public int GetValue(object type);

        public abstract Type GetVariant(int i);

        abstract public Type SmallStraight();

        abstract public Type LargeStraight();

        abstract public Type FullHouse();

        abstract public Type Yahtzee();

        abstract public Type Threes();

        abstract public Type Sixes();

        abstract public Type Chance();
    }

}
