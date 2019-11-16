namespace src.level.selection {
    public class SelectableLevel {
        public int Index { get; }
        public LevelPack LevelPack { get; }
        
        public bool Finished { get; }
        public int Score { get; }
        public LevelHeader LevelHeader { get; }
        public string LevelXml { get; }

        public SelectableLevel(LevelPack levelPack, int index, string levelXml, LevelHeader header, bool finished, int score) {
            Finished = finished;
            Score = score;
            LevelHeader = header;
            LevelXml = levelXml;
            Index = index;
            LevelPack = levelPack;
        }
    }
}