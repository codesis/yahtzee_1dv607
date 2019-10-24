using System.Collections.Generic;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Variants;

namespace yahtzee_1dv607.Model
{
    class Ai : Player
    {
        private Variant variant;
        private InterfaceRules rules;
        
        private bool[] DiceToRoll { get; set; }

        delegate bool del(int[] diceVal, int[] dice);

        private del KeepStraightChance;

        public Ai(string name, InterfaceRules rules, Variant variant, List<Score> scores, GameType gameType)
            : base(name, scores, true)
        {
            this.rules = rules;
            this.variant = variant;
            Strategy(gameType);
        }

        public Ai(int id, InterfaceRules rules, Variant variant, GameType gameType)
            : base("Ai" + id, true)
        {
            this.rules = rules;
            this.variant = variant;
            Strategy(gameType);
        }

        private void Strategy(GameType gameType)
        {
            if (gameType == GameType.Yahtzee)
            {
                KeepStraightChance = YahtzeeStraightChance;
            }
            else if (gameType == GameType.Yatzy)
            {
                KeepStraightChance = YatzyStraightChance;
            }
        }

        public bool[] DecideDiceToRoll(int[] diceVal, int[] dice)
        {
            DiceToRoll = new[] { false, false, false, false, false };

            if (Stand())
            {}
            else if (KeepThreeOrFourOfAKind(diceVal, dice))
            {}
            else if (KeepStraightChance(diceVal, dice))
            {}
            else if (KeepTwoPair(diceVal, dice))
            {}
            else if (KeepPair(diceVal, dice))
            {}
            else
            {
                DiceToRoll = new[] {true, true, true, true, true};
            }
            return DiceToRoll;
        }
        
        public Variant.Type SelectVariantToUse()
        {
            int highestValue = 0;
            Variant.Type highVariant = 0;
            int[] getValueByVariants = new int[variant.Length()];

            foreach (Variant.Type vari in variant.GetValues())
            {
                int i = variant.GetValue(vari);
                getValueByVariants[i] = rules.GetValueByVariant(vari);
                if (vari != variant.Chance() && !GetVariantChosen(vari) && getValueByVariants[i] >= highestValue)
                {
                    highestValue = getValueByVariants[i];
                    highVariant = vari;
                }
            }

            getValueByVariants[12] = rules.GetValueByVariant(variant.Chance());
            if (!GetVariantChosen(variant.Chance()) && getValueByVariants[12] > highestValue && highestValue < 10 && highVariant > variant.Threes() && highVariant < variant.Yahtzee())  // Only chance if nothing better or equal
            {
                highVariant = variant.Chance();
            }
            return highVariant;
        }
        private bool KeepTwoPair(int[] diceVal, int[] dice)
        {
            if (!GetVariantChosen(variant.FullHouse()))
            {
                int firstPairValue;
                int secondPairValue = 0;
                for (int i = 0; i < diceVal.Length; i++)
                {
                    if (diceVal[i] == 2)
                    {
                        firstPairValue = i+1;
                        for (int j = 0; j < diceVal.Length; j++)
                        {
                            if ((diceVal[j] == 2) && (j+1 != firstPairValue))
                            {
                                for (int k = 0; k < dice.Length; k++)
                                {
                                    secondPairValue = j+1;
                                    if ((dice[k] != firstPairValue) && (dice[k] != secondPairValue))
                                    {
                                        DiceToRoll[k] = true;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool KeepPair(int[] diceVal, int[] dice)
        {
            for (int i = 2; i < diceVal.Length; i++)
            {
                if (diceVal[i] == 2)
                {
                    for (int j = 0; j < dice.Length; j++)
                    {
                        if (dice[j] != i + 1)
                            DiceToRoll[j] = true;
                    }
                    return true;
                }
            }
            return false;
        }


        private bool KeepThreeOrFourOfAKind(int[] diceVal, int[] dice)
        {
            for (int i = 0; i < diceVal.Length; i++)
            {
                if ((diceVal[i] == 4) || (diceVal[i] == 3))
                {
                    for (int j = 0; j < dice.Length; j++)
                    {
                        if (dice[j] != i + 1)
                            DiceToRoll[j] = true;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool YahtzeeStraightChance(int[] diceVal, int[] dice)
        {
            if (!GetVariantChosen(variant.LargeStraight()) || !GetVariantChosen(variant.SmallStraight()))
            {
                for (int i = 5; i > 2; i--)
                {
                    if ((diceVal[i] > 0) && (diceVal[i - 1] > 0) && (diceVal[i - 2] > 0))
                    {
                        DiceToRoll = new bool[] { true, true, true, true, true };
                        for (int j = 0; j < dice.Length; j++)
                        {
                            if (dice[j] == i + 1)
                            {
                                DiceToRoll[j] = false;
                                for (int k = 0; k < dice.Length; k++)
                                {
                                    if (dice[k] == i)
                                    {
                                        DiceToRoll[k] = false;
                                        for (int m = 0; m < dice.Length; m++)
                                        {
                                            if (dice[m] == i - 1)
                                            {
                                                DiceToRoll[m] = false;
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return false;
        }
        private bool YatzyStraightChance(int[] diceVal, int[] dice)
        {
            int missing = 0;
            int twice = 0;
            DiceToRoll = new bool[] { false, false, false, false, false };

            if (!GetVariantChosen(variant.LargeStraight()))
            {   
                for (int i = 1; i < diceVal.Length; i++)
                {
                    if (diceVal[i] == 0)
                        missing++;
                    if (diceVal[i] == 2)
                        twice = i + 1;
                }
                if (missing <= 1)
                {
                    for (int i = 0; i < dice.Length; i++)
                    {
                        if (dice[i] == 1)
                            DiceToRoll[i] = true;
                        if (dice[i] == twice)
                        {
                            DiceToRoll[i] = true;
                            twice = 0;  
                        }
                    }
                    return true;
                }
            }
            if (!GetVariantChosen(variant.SmallStraight()))
            {   
                missing = 0;

                for (int i = 0; i < diceVal.Length - 1; i++)
                {
                    if (diceVal[i] == 0)
                        missing++;
                    if (diceVal[i] == 2)
                        twice = i + 1;
                }

                if (missing <= 1)
                {
                    for (int i = 0; i < dice.Length; i++)
                    {
                        if (dice[i] == 6)
                            DiceToRoll[i] = true;
                        if (dice[i] == twice)
                        {
                            DiceToRoll[i] = true;
                            twice = 0;  
                        }
                    }
                    return true;
                }
            }
            return false;
        }
                
        private bool Stand()
        {
            if ((rules.BasicRules.HasYahtzee()) && !GetVariantChosen(variant.Yahtzee()) ||
                (rules.BasicRules.HasFullHouse()) && !GetVariantChosen(variant.FullHouse()) ||
                (rules.HasLargeStraight()) && !GetVariantChosen(variant.LargeStraight()) ||
                (rules.HasSmallStraight()) && !GetVariantChosen(variant.SmallStraight()))
            {
                return true;
            }
            return false;
        }
    }
}
