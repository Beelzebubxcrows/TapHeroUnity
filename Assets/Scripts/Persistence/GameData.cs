using Newtonsoft.Json;

namespace Persistence
{
    
    public class GameData
    {
        [JsonProperty("latestLevel")]
        public int LatestLevel;
        
        [JsonProperty("selectedLevel")]
        public int SelectedLevel;

        public GameData(int level, int selectedLevel)
        {
            LatestLevel = level;
            SelectedLevel = selectedLevel;
        }
    }
}