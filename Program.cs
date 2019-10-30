using System;

using yahtzee_1dv607.Controller;

namespace yahtzee_1dv607
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GameController game = new GameController();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
