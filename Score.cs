using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Minesweeper.Core
{
    // holds one score entry for the leaderboard
    public class HighScore
    {
        public int Size { get; set; }
        public int Seconds { get; set; }
        public int Moves { get; set; }
        public int Seed { get; set; }
        public string Timestamp { get; set; }
    }

    public class HighScoreManager
    {
        private string filePath;

        // sets up file path and makes sure file exists
        public HighScoreManager(string filePath)
        {
            this.filePath = filePath;

            // if file doesn't exist, create it with header row
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "size,seconds,moves,seed,timestamp\n");
        }

        // reads all scores from the csv file
        public List<HighScore> LoadScores()
        {
            var scores = new List<HighScore>();

            try
            {
                var lines = File.ReadAllLines(filePath);

                // skips header line
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(',');

                    if (parts.Length != 5) continue;

                    scores.Add(new HighScore
                    {
                        Size = int.Parse(parts[0]),
                        Seconds = int.Parse(parts[1]),
                        Moves = int.Parse(parts[2]),
                        Seed = int.Parse(parts[3]),
                        Timestamp = parts[4]
                    });
                }
            }
            catch
            {
                // returns empty list if file is messed up
                return new List<HighScore>();
            }

            return scores;
        }

        // adds a new score and keeps the best ones
        public void AddScore(HighScore score)
        {
            var scores = LoadScores();

            scores.Add(score);

            // only keep top 5 scores for each board size
            var filtered = scores
                .Where(s => s.Size == score.Size)
                .OrderBy(s => s.Seconds)
                .ThenBy(s => s.Moves)
                .Take(5);

            SaveAll(scores.Where(s => s.Size != score.Size).Concat(filtered));
        }

        // writes everything back to file
        private void SaveAll(IEnumerable<HighScore> scores)
        {
            var lines = new List<string>
            {
                "size,seconds,moves,seed,timestamp"
            };

            lines.AddRange(scores.Select(s =>
                $"{s.Size},{s.Seconds},{s.Moves},{s.Seed},{s.Timestamp}"
            ));

            File.WriteAllLines(filePath, lines);
        }
    }
}