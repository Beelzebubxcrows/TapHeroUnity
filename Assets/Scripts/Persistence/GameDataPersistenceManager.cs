using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;



namespace Persistence
{ 
    public class GameDataPersistenceManager : IDisposable
    {
        private TextAsset _jsonTextAsset;
        private GameData _savedGameData;
        private  string FILE_PATH = Application.persistentDataPath + "/gameData.json";
        private bool _fileExists;
        public  void Initialise()
        {
            _fileExists = File.Exists(FILE_PATH);
            var gameDataText = ReadSavedGameDataFile();
            ParseJson(gameDataText);
        }

        private void ParseJson(string result)
        {
            _savedGameData = JsonConvert.DeserializeObject<GameData>(result);
            if (_savedGameData == null)
            {
                Debug.LogError("Json Conversion failed.");
            }
        }
        
        #region PUBLIC PERSISTENCE METHODS

        public int GetLatestLevel()
        {
            if (IsContentLoaded())
            {
                return _savedGameData.LatestLevel;
            }

            Debug.LogError("Game Data is not Loaded.");
            return -1;
        }

        public void SetLatestLevel(int level)
        {
            if (!IsContentLoaded())
            {
                Debug.LogError("Game Data is not Loaded.");
                return;  
            }

            _savedGameData.LatestLevel = level;
            UpdateContent();
        }
        
        public int GetSelectedLevel()
        {
            if (IsContentLoaded())
            {
                return _savedGameData.SelectedLevel;
            }

            Debug.LogError("Game Data is not Loaded.");
            return -1;
        }

        public void SetSelectedLevel(int level)
        {
            if (!IsContentLoaded())
            {
                Debug.LogError("Game Data is not Loaded.");
                return;  
            }

            _savedGameData.SelectedLevel = level;
            UpdateContent();
        }
        
        #endregion

        #region PRIVATE UTILITY FUNCTIONS

        private bool IsContentLoaded()
        {
            return _savedGameData != null;
        }

        private void UpdateContent()
        {
            SaveData(_savedGameData);
        }
        
        private void SaveData(GameData gameData)
        {
            _savedGameData = gameData;
            var newJsonString = JsonConvert.SerializeObject(gameData);
            WriteToGameDataJson(newJsonString);
        }

        private string ReadSavedGameDataFile()
        {
            if (!_fileExists)
            {
                _fileExists = true;
                _savedGameData = new GameData(1,1);
                SaveData(_savedGameData);
            }
            
            var reader = new StreamReader(FILE_PATH);
            var text = reader.ReadToEnd();
            reader.Close();
            return text;
        }

        private void WriteToGameDataJson(string newJsonString)
        {
            var writer = new StreamWriter(FILE_PATH, false);
            writer.WriteLine(newJsonString);
            writer.Close();
        }

        #endregion

        public void Dispose()
        {
        }
    }
}