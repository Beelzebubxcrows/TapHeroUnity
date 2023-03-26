using System;
using CoreGame;
using DataManager;
using GameBackground;
using HUD;
using Loading;
using Popups;
using UnityEngine;


namespace Gameplay
{
    public class PlayAreaManager : MonoBehaviour
    {
        [SerializeField] private GameObject character;
        [SerializeField] private GameBackgroundHandler gameBackgroundHandler;
        [SerializeField] private HudManager hudManager;
        [SerializeField] private LevelCompletePopup levelCompletePopup;
        
        private Character.Character _spawnedCharacter;
        private GameAudioManager _audioManager;
        private LevelData _levelData;
        private LevelManager _levelManager;

        public void Initialise()
        {
            _audioManager = DependencyManager.DependencyManager.GetInstance<GameAudioManager>();
            _levelManager = DependencyManager.DependencyManager.GetInstance<LevelManager>();
        }
        

        #region GAMEPLAY SEQUENCE
        public void StartGamePlayWithoutTransition()
        {
            LoadLevelData();
        }
        public void StartGamePlayWithTransitionScreen()
        {
            levelCompletePopup.gameObject.SetActive(false);
            ShowLoadingScreen(LoadLevelData);
        }

        private void ShowLoadingScreen(Action callback)
        {
            var loadingScreen = DependencyManager.DependencyManager.GetInstance<LoadingScreen>();
            loadingScreen.StartTransitionAnimation(callback);
        }

        private void LoadLevelData()
        {
            _levelData = _levelManager.GetSelectedLevelData();
            hudManager.Initialise(_levelData);
            SpawnBackground();
        }

        private void SpawnBackground()
        {
            gameBackgroundHandler.Initialise(1,this);
            SpawnCharacter();
        }

        private void SpawnCharacter()
        {
            _spawnedCharacter = Instantiate(character, transform).GetComponent<Character.Character>();
            _spawnedCharacter.Initialise(this,_levelData.CharacterSpeed);
            //Check for tutorial here.
            BeginGameplay();
        }

        private void BeginGameplay()
        {
            hudManager.EnableGameplay();
            _spawnedCharacter.StartMovingRandomly();
        }

        #endregion

        public void DeductCharacterHealth()
        {
            hudManager.DecreaseCharacterHealth();
        }
        
        public void Dispose()
        {
            _spawnedCharacter.Dispose();
        }

        public bool ShouldContinueGame()
        {
            if (hudManager.GetVictimsHealth()>0)
            {
                if (hudManager.GetCharactersHealth() >0)
                {
                    return true;
                }
                else
                {
                    InitiateGameWin();
                    return false;
                }
            }
            else
            {
                InitiateGameLoss();
                return false;
            }
        }

        private void InitiateGameLoss()
        {
            Destroy(_spawnedCharacter.gameObject);
            hudManager.StopGamePlay();
            _audioManager.PlayGameLossAudio();
            
            levelCompletePopup.OpenPopup(false,_levelData.LevelNumber,this);
        }

        private void InitiateGameWin()
        {
            Destroy(_spawnedCharacter.gameObject);
            hudManager.StopGamePlay();
            _audioManager.PlayGameWonAudio();
            _levelManager.IncrementLatestLevel();
            levelCompletePopup.OpenPopup(true,_levelData.LevelNumber,this);
        }
        
        
        public void RegisterForVictimHealthChanged(Action<float> callback)
        {
            hudManager.RegisterForVictimHealthChanged(callback);
        }
        public void UnregisterForVictimHealthChanged(Action<float> callback)
        {
            hudManager.UnregisterForVictimHealthChanged(callback);
        }

        public float GetMaxVictimHealth()
        {
            return hudManager.GetMaxVictimHealth();
        }
    }
}