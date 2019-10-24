using System;

namespace yahtzee_1dv607.Model
{
    class Dice
    {
        private Random random;

        public int Id { get; private set; }

        public int Value { get; private set; }

        public Dice(int id)
        {
            random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Id = id;
            Value = 0;
        }

        public void Roll()
        {
            Value = random.Next(1, 7);
        }
    }
}
