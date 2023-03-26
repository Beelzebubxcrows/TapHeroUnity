using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace DataManager
{
    public class LevelData
    {
        [JsonProperty("ln")]
        public int LevelNumber;
        [JsonProperty("cs")]
        public float CharacterSpeed;
        [JsonProperty("cmh")]
        public float CharacterMaxHealth;
        [JsonProperty("cdp")]
        public float CharacterDamagePercentage;
        [JsonProperty("citymh")]
        public int CityMaxHealth;
        [JsonProperty("citydp")]
        public int CityDamagePercentage;
    }
    public class LevelDataLoaderProvider
    {
        private const string LABEL = "configuration";

        private Dictionary<int, LevelData> _levelDataDictionary;
        public async Task LoadLevelData()
        {
            _levelDataDictionary = new Dictionary<int, LevelData>();
            var levelDataLoadHandle =  Addressables.LoadAssetAsync<TextAsset>(LABEL);
            await levelDataLoadHandle.Task;
            if (levelDataLoadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                ParseLevelJson(levelDataLoadHandle.Result.text);
            }
            else
            {
                Debug.LogError("Loading the Level Json through Addressables has failed.");
            }
        }

        private void ParseLevelJson(string jsonString)
        {
            var levelDesignData = JsonConvert.DeserializeObject<List<LevelData>>(jsonString);
            if (levelDesignData == null)
            {
                Debug.LogError("Json Conversion failed.");
                return;
            }
            
            foreach (var level in levelDesignData)
            {
                _levelDataDictionary.Add(level.LevelNumber, level);
            }
        }

        public LevelData GetLevelData(int level)
        {
            if (_levelDataDictionary.ContainsKey(level))
            {
                return _levelDataDictionary[level];
            }
            else
            {
                Debug.LogError("Level Does not exist :" + level);
                return null;
            }
        }

        public int GetEOCLevel()
        {
            return _levelDataDictionary.Count;
        }

        public void Dispose()
        {
            
        }
    }
}