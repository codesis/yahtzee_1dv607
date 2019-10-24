using System;
using System.Collections.Generic;
using System.IO;

using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Rules;

namespace yahtzee_1dv607.Model
{
    class Database
    {
        private readonly string path = $"{Environment.CurrentDirectory}/Database/";
        private string fileName = "";

        private InterfaceRules rules;
        private GameType gameType;
        private Variant variant;

        public Database(Variant variant, InterfaceRules rules, GameType gameType)
        {
            this.variant = variant;
            this.gameType = gameType;
            this.rules = rules;
            this.fileName = gameType.ToString();
            Directory.CreateDirectory(path);
        }
        public string SaveGameToFile(DateTime date, int roundNumber, List<Player> players) 
        {
            string dateStr = date.ToString();
            dateStr = dateStr.Substring(2, 2) + dateStr.Substring(5, 2) + dateStr.Substring(8, 2) + dateStr.Substring(11, 2) + dateStr.Substring(14, 2) + dateStr.Substring(17, 2) + ".txt";

            StreamWriter file = new StreamWriter(path + fileName + dateStr);

            string output = date.ToString();
            output = date.ToString();
            file.WriteLine(output);
            output = roundNumber.ToString();
            file.WriteLine(output);

            foreach (Player player in players)
            {
                output = player.Name;
                file.WriteLine(output);
                output = player.IsAI.ToString().ToLower();
                file.WriteLine(output);
                Score[] score = player.GetScoreList();

                for (int i=0; i < score.Length; i++)
                {
                    output = "";
                    output += score[i].Points + "|" + score[i].ChosenVariant;
                    file.WriteLine(output);
                }
            }

            file.Close();
            return path + fileName + dateStr;
        }

        public List<Player> GetPlayersFromFile(InterfaceRules rules, string fileName, out DateTime date, out int roundNumber)
        {
            string line;
            List<Player> players = new List<Player>();
            List<string> items = new List<string>();
            StreamReader file = new StreamReader(path+fileName);

            while((line = file.ReadLine()) != null)
            {
                items.Add(line);
            }

            file.Close();

            date = Convert.ToDateTime(items[0]);
            roundNumber = int.Parse(items[1]);

            string name = "";
            bool isAI = false;
            int rowsForPlayer = roundNumber + 2;
            int noOfPlayers = (items.Count - 2) / (roundNumber + 2);
            int indexStartPlayer = 2;

            for (int i = 0; i < noOfPlayers ;i++)
            {
                indexStartPlayer = 2 + i * rowsForPlayer;
                List<Score> scoreList = new List<Score>();
                name = items[indexStartPlayer];
                isAI = bool.Parse(items[indexStartPlayer + 1]);
                string[] scoreItems;

                for (int j = 0; j < roundNumber; j++)
                {
                    scoreItems = items[indexStartPlayer + 2 + j].Split('|');
                    int point = Int32.Parse(scoreItems[0]);
                    Variant.Type cat = (Variant.Type)Enum.Parse(typeof(Variant.Type), (scoreItems[1]));
                    Score score = new Score(cat, point);
                    scoreList.Add(score);
                }
                if (isAI)
                {
                    Ai ai = new Ai(name, rules, variant, scoreList, gameType);
                    players.Add(ai);
                }
                else
                {
                    Player player = new Player(name, scoreList);
                    players.Add(player);
                }
            }
            return players;
        }

        public FileInfo[] ListSavedGames()
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] files = d.GetFiles("*" + gameType.ToString() + "*.txt");

            return files;
        }
    }
}
