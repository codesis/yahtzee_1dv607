using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yahtzee_1dv607.Controller;

using Newtonsoft.Json;

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
