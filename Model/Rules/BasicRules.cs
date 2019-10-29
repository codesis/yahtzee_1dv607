using yahtzee_1dv607.Model.Dices;

namespace yahtzee_1dv607.Model.Rules
{
    class BasicRules
    {
        private DiceCollection diceCollection;

        public BasicRules(DiceCollection diceCollection)
        {
            this.diceCollection = diceCollection;
        }

        public bool HasPair()
        {
            if (diceCollection.GetMaxNumberOfSameValues() >= 2)
            {
                return true;
            }
            return false;
        }

        public bool HasTwoPair()
        {
            int[] diceVal = diceCollection.GetNumberOfDiceFaceValue();
            bool retValue = false;
            for (int i = 0; i < diceVal.Length; i++)
            {
                if (diceVal[i] >= 2)
                {
                    for (int j = i + 1; j < diceVal.Length; j++)
                    {
                        if (diceVal[j] >= 2)
                            retValue = true;
                    }
                }
            }
            return retValue;
        }

        public bool HasThreeOfAKind()
        {

            if (diceCollection.GetMaxNumberOfSameValues() >= 3)
            {
                return true;
            }
            return false;
        }

        public bool HasFourOfAKind()
        {

            if (diceCollection.GetMaxNumberOfSameValues() >= 4)
            {
                return true;
            }
            return false;
        }
        public bool HasFullHouse()
        {
            int[] diceVal = diceCollection.GetNumberOfDiceFaceValue();
            bool retValue = false;
            for (int i = 0; i < diceVal.Length; i++)
            {
                if (diceVal[i] == 2)
                {
                    for (int j = 0; j < diceVal.Length; j++)
                    {
                        if (diceVal[j] == 3)
                            retValue = true;
                    }
                }
            }
            return retValue;
        }

        public bool HasYahtzee()
        {
            int[] diceVal = diceCollection.GetNumberOfDiceFaceValue();
            bool retVal = false;
            for (int i = 0; i < diceVal.Length; i++)
            {
                if (diceVal[i] == 5)
                    retVal = true;
            }
            return retVal;
        }
    }
}
