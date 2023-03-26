using System;
using System.Collections;
using CoreGame;
using Gameplay;
using LevelNavigation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popups
{
    public class LevelCompletePopup : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image icon;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Sprite trophySprite;
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private TMP_Text levelStatusText;
        [SerializeField] private TMP_Text levelNumberHeader;
        [SerializeField] private LevelNavigationManager levelNavigationManager;
        private const float ANIMATION_TIME_TUNE = 0.03f;
        private const string LEVEL_CLEARED = "Level Completed";
        private const string LEVEL_FAILED = "Level Failed";
        private const string LEVEL_NUMBER_FORMAT = "Level - {0}";
        private PlayAreaManager _playAreaManager;
        private LevelManager _levelManager;
        
        public void OpenPopup(bool isLevelWon,int levelNumber, PlayAreaManager playAreaManager)
        {
            levelNavigationManager.Initialise(this);
            Initialise(isLevelWon,levelNumber,playAreaManager);
            StartIntroAnimation();
        }

        private void Initialise(bool isLevelWon,int levelNumber, PlayAreaManager playAreaManager)
        {
            _levelManager = DependencyManager.DependencyManager.GetInstance<LevelManager>();
            _playAreaManager = playAreaManager;
            nextButton.onClick.AddListener(OnNextClick);
            retryButton.onClick.AddListener(OnClickRetry);
            SetupVisuals(isLevelWon, levelNumber);
        }

        private void SetupVisuals(bool isLevelWon, int levelNumber)
        {
            canvasGroup.gameObject.transform.localScale = Vector3.zero;
            canvasGroup.alpha = 0;
            nextButton.gameObject.SetActive(isLevelWon);
            retryButton.gameObject.SetActive(!isLevelWon);
            icon.sprite = isLevelWon ? trophySprite : characterSprite;
            levelStatusText.text = isLevelWon ? LEVEL_CLEARED : LEVEL_FAILED;
            levelNumberHeader.text = String.Format(LEVEL_NUMBER_FORMAT,levelNumber);
            levelNavigationManager.PopulateNodes();
        }

        private void OnClickRetry()
        {
            ClosePopup();
            _playAreaManager.StartGamePlayWithTransitionScreen();
        }

        private void ClosePopup()
        {
            StartOutroAnimation();
            Dispose();
        }

        private void OnNextClick()
        {
            _levelManager.SetSelectedLevel(_levelManager.GetSelectedLevel()+1);
            ClosePopup();
            _playAreaManager.StartGamePlayWithTransitionScreen();
        }

        public void OpenLevel(int levelNumber)
        {
            if (levelNumber > _levelManager.GetLatestLevel())
            {
                return;
            }
            _levelManager.SetSelectedLevel(levelNumber);
            ClosePopup();
            _playAreaManager.StartGamePlayWithTransitionScreen();
        }


        #region ANIMATIONS

        private void StartIntroAnimation()
        {
            gameObject.SetActive(true);
            StartCoroutine(nameof(ScaleUp));
            StartCoroutine(nameof(FadeIn));
        }

        private void StartOutroAnimation()
        {
            StartCoroutine(nameof(ScaleDown));
            StartCoroutine(nameof(FadeOut));
        }

        public IEnumerator ScaleUp()
        {
            for (var scale = 0f; scale <= 1f; scale += 0.1f)
            {
                canvasGroup.gameObject.transform.localScale = new Vector3(scale,scale,scale);
                yield return new WaitForSeconds(ANIMATION_TIME_TUNE);
            }
            canvasGroup.gameObject.transform.localScale = Vector3.one;
        }
        
        public IEnumerator ScaleDown()
        {
            for (var scale = 0f; scale <= 1f; scale -= 0.1f)
            {
                canvasGroup.gameObject.transform.localScale = new Vector3(scale,scale,scale);
                yield return new WaitForSeconds(ANIMATION_TIME_TUNE);
            }
            canvasGroup.gameObject.transform.localScale = Vector3.zero;
        }
        
        public IEnumerator FadeIn()
        {
            for (var alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(ANIMATION_TIME_TUNE);
            }
            canvasGroup.alpha = 1;
        }
        
        public IEnumerator FadeOut()
        {
            for (var alpha = 0f; alpha <= 1f; alpha -= 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(ANIMATION_TIME_TUNE);
            }
            canvasGroup.alpha = 0;
        }
        #endregion


        private void Dispose()
        {
            nextButton.onClick.RemoveListener(OnNextClick);
            retryButton.onClick.RemoveListener(OnClickRetry);
            levelNavigationManager.Dispose();
            gameObject.SetActive(false);
        }
        


    }
}