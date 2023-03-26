using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace GameBackground
{
    [Serializable]
    public class BackgroundData
    {
        public int backgroundId;
        public List<Sprite> backgroundSprites;
        public Sprite endImage;
    }
    
    public class GameBackgroundHandler : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private List<BackgroundData> gameBackgrounds;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private int _currentSetIndex;
        private BackgroundData _currentBackgroundData;
        private PlayAreaManager _playAreaManager;
        private float _maxVictimHealth;
        private Sprite _targetSprite;

        public void Initialise(int backgroundId, PlayAreaManager playAreaManager)
        {
            _currentBackgroundData = FindDataForBackgroundId(backgroundId);
            _playAreaManager = playAreaManager;
            _maxVictimHealth = playAreaManager.GetMaxVictimHealth();
            SetInitialBackground();
            _playAreaManager.RegisterForVictimHealthChanged(OnVictimHealthChanged);
        }

        private void SetInitialBackground()
        {
            var lastIndex=_currentBackgroundData.backgroundSprites.Count - 1;
            backgroundImage.sprite = _currentBackgroundData.backgroundSprites[lastIndex];
            _currentSetIndex = lastIndex;
        }

        private BackgroundData FindDataForBackgroundId(int backgroundId)
        {
            foreach (var bgData in gameBackgrounds)
            {
                if (bgData.backgroundId == backgroundId)
                {
                    return bgData;
                }
            }
            Debug.LogError("NO BACKGROUND DATA WITH THIS ID : "+backgroundId);
            return null;
        }

        private void OnVictimHealthChanged(float obj)
        {
            var currentHealth = obj;
            var percentageRemaining = (currentHealth / _maxVictimHealth)*100;
            var index = GetIndexOfBackground(percentageRemaining);
            SetBackground(index);
        }

        private void SetBackground(int index)
        {
            if (index == _currentSetIndex)
            {
                return;
            }

            if (index < 0 )
            {
                backgroundImage.sprite = _currentBackgroundData.endImage;
            }
            else
            {
                _targetSprite = _currentBackgroundData.backgroundSprites[index];
                StartCoroutine(nameof(ChangeBackgroundWithAnimation));
            }
            _currentSetIndex = index;
        }

        private IEnumerator ChangeBackgroundWithAnimation()
        {
            //Fading out the background.
            for (var alpha = 1f; alpha >=0f; alpha -= 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(0.02f);
            }
            canvasGroup.alpha = 0;
            
            //Setting the sprite.
            backgroundImage.sprite = _targetSprite;
            
            //Fading in the background.
            for (var alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(0.02f);
            }
            canvasGroup.alpha = 1;
        }

        private int GetIndexOfBackground(float percentageRemaining)
        {
            var range = 100 * 1.0f / _currentBackgroundData.backgroundSprites.Count;
            var index = Math.Ceiling(percentageRemaining / range);
            return (int)index - 1;
        }


        public void Dispose()
        {
            _playAreaManager.UnregisterForVictimHealthChanged(OnVictimHealthChanged);
        }
    }
}