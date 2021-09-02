using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartMaster9000.Class;
using Newtonsoft.Json;

namespace DartMaster9000.Tools
{
    public static class SaveManager
    {
        static string path = $"{AppDomain.CurrentDomain.BaseDirectory}/SaveFile/SaveFile.json";

        private static Dictionary<string,Stats> LoadPlayers()
        {
            string json = File.ReadAllText(path);
            Dictionary<string, Stats> playersStats = null;
            playersStats = JsonConvert.DeserializeObject<Dictionary<string, Stats>>(json);
            return playersStats;
        }

        public static void Save(List<Player> Players)
        {
            Dictionary<string,Stats> playersStats = LoadPlayers();

            foreach (Player p in Players)
            {
                Stats stat = new Stats();
                stat.Victories = p.MyStats.Victories;
                if (playersStats.ContainsKey(p.Name) == false)
                    playersStats.Add(p.Name, stat);
                else
                    playersStats[p.Name] = stat;
            }

            string json = JsonConvert.SerializeObject(playersStats);
            File.WriteAllText(path, json);
        }

        public static Player GetPlayer(Player p)
        {
            Dictionary<string, Stats> playersStats = LoadPlayers();

            if (playersStats.ContainsKey(p.Name) == false)
                return p;

            Stats stats = playersStats[p.Name];
            p.MyStats = stats;
            return p;
        }

        public static List<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();
            foreach (var item in LoadPlayers())
            {
                Player p = new Player(item.Key);
                p.MyStats = item.Value;
                players.Add(p);
            }
            return players;
        }
    }
}
