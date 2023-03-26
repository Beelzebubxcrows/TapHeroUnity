using System;
using System.Threading.Tasks;
using DataManager;
using Persistence;
using UnityEngine;

namespace CoreGame
{
    public class LevelManager  : IDisposable
    {
        private LevelDataLoaderProvider _levelDataLoaderProvider;
        private GameDataPersistenceManager _gameDataPersistenceManager;
        
        public LevelManager()
        {
            _levelDataLoaderProvider = new LevelDataLoaderProvider();
            _gameDataPersistenceManager = new GameDataPersistenceManager();
            
        }
        public async Task Initialise()
        {
            await _levelDataLoaderProvider.LoadLevelData();
            _gameDataPersistenceManager.Initialise();
        }

        #region PLAYER PROGRESSSION

        public int GetSelectedLevel()
        {
            return _gameDataPersistenceManager.GetSelectedLevel();
        }

        public void IncrementLatestLevel()
        {
            var currentLevel = _gameDataPersistenceManager.GetSelectedLevel();
            var latestLevel = GetLatestLevel();
            var incrementedLevel = Math.Max(currentLevel + 1, latestLevel);
            incrementedLevel = Math.Min(incrementedLevel, GetEocLevel());
            _gameDataPersistenceManager.SetLatestLevel(incrementedLevel);
        }
        
        public int GetLatestLevel()
        {
            return _gameDataPersistenceManager.GetLatestLevel();
        }

        public void SetSelectedLevel(int level)
        {
            if (level > GetLatestLevel())
            {
                Debug.LogError("SelectedLevel cannot be more than latest level.");
                return;
            }
            _gameDataPersistenceManager.SetSelectedLevel(Math.Min(level,GetEocLevel()));
        }
        
        #endregion
        
        
        #region PLAYER DATA
        public int GetEocLevel()
        {
            return _levelDataLoaderProvider.GetEOCLevel();
        }
        public LevelData GetSelectedLevelData()
        {
            return _levelDataLoaderProvider.GetLevelData(GetSelectedLevel());
        }
        
        #endregion

        public void Dispose()
        {
            _gameDataPersistenceManager.Dispose();
            _levelDataLoaderProvider.Dispose();
        }
    }
}