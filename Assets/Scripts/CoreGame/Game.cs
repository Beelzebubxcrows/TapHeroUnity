using System.Collections;
using Gameplay;
using Loading;
using UnityEngine;
using Utilities;

namespace CoreGame
{
    public class Game : MonoBehaviour
    {
        
        [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private GameAudioManager gameAudioManager;
        [SerializeField] private LoadingScreen loadingScreen;
        [SerializeField] private IntroScreen introScreen;
        
        private TimeManager _timeManager;
        private void Awake()
        {
            BindGameDependencies();
        }

        private async void BindGameDependencies()
        {
            _timeManager = new TimeManager();
            DependencyManager.DependencyManager.CreateNewInstance<LoadingScreen>(loadingScreen);
            DependencyManager.DependencyManager.CreateNewInstance<GameAudioManager>(gameAudioManager);
            DependencyManager.DependencyManager.CreateNewInstance<TimeManager>(_timeManager);
            
            var levelManager = new LevelManager();
            DependencyManager.DependencyManager.CreateNewInstance<LevelManager>(levelManager);
            await levelManager.Initialise();
            levelManager.SetSelectedLevel(levelManager.GetLatestLevel());
            DependencyManager.DependencyManager.CreateNewInstance<GameplayManager>(gameplayManager);
            loadingScreen.Initialise();
            introScreen.Initialise();
            StartCoroutine(nameof(StartTime));
            PostDependenciesBindComplete();
        }
        
        private void PostDependenciesBindComplete()
        {
            ShowIntroScreen();
        }

        private void ShowIntroScreen()
        {
            introScreen.StartTransitionAnimation(PostIntroScreenAnimationFinished);
        }

        private void PostIntroScreenAnimationFinished()
        {
            gameplayManager.Initialise();
            gameplayManager.StartGamePlayWithoutTransition();
        }

        IEnumerator StartTime() {
            while(true) {
                yield return new WaitForSeconds(1f);
                _timeManager.InvokeSecondsElapsed();
            }
        }


        private void OnDestroy()
        {
            DependencyManager.DependencyManager.DestroyInstance<LoadingScreen>();
            DependencyManager.DependencyManager.DestroyInstance<TimeManager>();
            DependencyManager.DependencyManager.DestroyInstance<GameAudioManager>();
            DependencyManager.DependencyManager.DestroyInstance<LevelManager>();
            DependencyManager.DependencyManager.DestroyInstance<GameplayManager>();
            gameplayManager.Dispose();
        }
    }
}