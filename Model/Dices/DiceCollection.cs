using System.Collections.Generic;
using System.Threading;

using yahtzee_1dv607.Model.Observer;

namespace yahtzee_1dv607.Model.Dices
{
    class DiceCollection
    {
        public const int NoOfDice = 5;
        private List<InterfaceDiceObserver> observers = new List<InterfaceDiceObserver>();

        public DiceCollection()
        {
            Dice = new List<Dice>();

            for (int i = 1; i <= NoOfDice; i++)
            {
                Thread.Sleep(20);
                Dice.Add(new Dice(i));
            }
        }

        public List<Dice> Dice { get; private set; }

        public void Roll(bool[] diceToRoll)
        {
            foreach (Dice dice in Dice)
            {
                if (diceToRoll[dice.Id-1])
                {
                    dice.Roll();
                }
            }
            NotifyAll();
        }

        public int[] GetDice()
        {
            int[] diceValues = { 0, 0, 0, 0, 0 };
            int index = 0;

            foreach (Dice dice in Dice)
            {
                diceValues[index++] = dice.Value;
            }
            return diceValues;
        }

        public int GetSumOfDice()
        {
            int sum = 0;
            
            foreach (Dice dice in Dice)
            {
                sum += dice.Value;
            }
            return sum;
        }

        public int[] GetNumberOfDiceFaceValue()
        {
            int[] diceValues = { 0, 0, 0, 0, 0, 0 };

            foreach (Dice dice in Dice)
            {
                diceValues[dice.Value - 1]++;
            }
            return diceValues;
        }

        public int GetMaxNumberOfSameValues()
        {
            int[] diceValues = GetNumberOfDiceFaceValue();
            int highestNumberOfSame = 0;

            for (int i = 0; i < 6; i++)
            {
                if (diceValues[i] > highestNumberOfSame)
                    highestNumberOfSame = diceValues[i];
            }
            return highestNumberOfSame;
        }

        public void Subscribe(InterfaceDiceObserver observer)
        {
            observers.Add(observer);
        }

        private void NotifyAll()
        {
            foreach (var observer in observers)
            {
                if (observer != null) observer.DiceRolled(GetNumberOfDiceFaceValue(), GetDice());
            }
        }
    }
}
