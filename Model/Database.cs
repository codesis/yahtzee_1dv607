using System;
using System.Collections.Generic;
using System.IO;

using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Players;

namespace yahtzee_1dv607.Model
{
    class Database
    {
        private readonly string path = $"{Environment.CurrentDirectory}/SavedGames/";
        private string fileName = "";

        private InterfaceRules rules;
        private GameType gameType;
        private Variant variant;
        internal List<Player> playersfromfile = new List<Player>();

        public Database(Variant variant, InterfaceRules rules, GameType gameType)
        {
            this.variant = variant;
            this.gameType = gameType;
            this.rules = rules;
            this.fileName = gameType.ToString();
            this.playersfromfile = new List<Player>();
            Directory.CreateDirectory(path + fileName);
        }
        public string SaveGameToFile(DateTime date, int roundNumber, List<Player> players) 
        {
            string dateStr = date.ToString();
            dateStr = DateTime.Now.ToString("ddMMyyyy") + ".txt";
            var savingDirectory = Path.Combine(path + "/" + gameType.ToString() + "/" + fileName + dateStr);

            int j = 1;
            while (File.Exists(savingDirectory))
            {
                string uniqueId = string.Format("({0})", j);

                if (File.Exists(savingDirectory + uniqueId))
                {
                    j++;
                    uniqueId = string.Format("({0})", j);
                }

                savingDirectory = Path.Combine(savingDirectory + uniqueId);

            }

            StreamWriter file = new StreamWriter(savingDirectory);

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
                    output += score[i].Points + "|" + score[i].TakenChoice;
                    file.WriteLine(output);
                }
            }

            file.Close();
            return path + fileName + dateStr;
        }

        public List<Player> GetPlayersFromFile(InterfaceRules rules, string fileName, out DateTime date, out int roundNumber)
        {
            string line;
            this.playersfromfile = new List<Player>();
            List<string> items = new List<string>();
            var savingDirectory = Path.Combine(path + "/" + gameType.ToString() + "/" + fileName);
            StreamReader file = new StreamReader(savingDirectory);

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
                    Variant.Type vari = (Variant.Type)Enum.Parse(typeof(Variant.Type), (scoreItems[1]));
                    Score score = new Score(vari, point);
                    scoreList.Add(score);
                }
                if (isAI)
                {
                    Ai ai = new Ai(name, rules, variant, scoreList, gameType);
                    playersfromfile.Add(ai);
                }
                else
                {
                    Player player = new Player(name, scoreList);
                    playersfromfile.Add(player);
                }
            }
            return playersfromfile;
        }

        public FileInfo[] ListSavedGames()
        {
            DirectoryInfo d = new DirectoryInfo(path + "/" + gameType.ToString());

            return d.GetFiles();

        }
    }
}
