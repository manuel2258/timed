using System;

namespace src.level {
    public class LevelHeader {
        
        public string Name { get; }
        public Guid GUID { get; }
        public int Difficulty { get; }
        public int[] Scores { get; }  

        public LevelHeader(string name, string guid, int difficulty, int[] scores) {
            Name = name;
            GUID = new Guid(guid);
            Difficulty = difficulty;
            Scores = scores;
        }
    }
}