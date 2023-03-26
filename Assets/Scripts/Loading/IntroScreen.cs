using System;
using System.Collections;
using CoreGame;
using UnityEngine;

namespace Loading
{
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject tapSmashText;
        [SerializeField] private GameObject heroSmashText;
        [SerializeField] private GameObject[] hammers;
        
        private Action _callBack;
        
        private GameAudioManager _audioManager;

        public void Initialise()
        {
            _audioManager = DependencyManager.DependencyManager.GetInstance<GameAudioManager>();
        }
        
        public void StartTransitionAnimation(Action callback)
        {
            gameObject.SetActive(true);
            _callBack = callback;
            StartCoroutine(nameof(StartAnimation));
        }

        private IEnumerator StartAnimation()
        {
            
            //Fading in the background.
            for (var alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(0.01f);
            }

            canvasGroup.alpha = 1;

            yield return new WaitForSeconds(0.2f);
            //Popping in the tap and smash text.
            
            StartCoroutine(nameof(FadeInTapText));
            StartCoroutine(nameof(ScaleUpTapText));
            yield return new WaitForSeconds(0.2f);
            
            StartCoroutine(nameof(FadeInHeroText));
            StartCoroutine(nameof(ScaleUpHeroText));
            
            
            yield return new WaitForSeconds(0.5f);
            foreach (var hammer in hammers)
            {
                _audioManager.PlayClickAudio();
                hammer.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitForSeconds(0.1f);
            for (var alpha = 1f; alpha >=0f; alpha -= 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(0.01f);
            }
            canvasGroup.alpha = 0;  
            
            _callBack?.Invoke();
            gameObject.SetActive(false);
        }
        
        
        public IEnumerator FadeInTapText()
        {
            var canvasGroupParam = tapSmashText.GetComponent<CanvasGroup>();
            for (var alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                canvasGroupParam.alpha = alpha;
                yield return new WaitForSeconds(0.01f);
            }
            canvasGroupParam.alpha = 1;
        }
        
        public IEnumerator FadeInHeroText()
        {
            var canvasGroupParam = heroSmashText.GetComponent<CanvasGroup>();
            for (var alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                canvasGroupParam.alpha = alpha;
                yield return new WaitForSeconds(0.01f);
            }
            canvasGroupParam.alpha = 1;
        }
        
        public IEnumerator ScaleUpTapText()
        {
            for (var scale = 0f; scale <= 1f; scale += 0.1f)
            {
                tapSmashText.transform.localScale = new Vector3(scale,scale,scale);
                yield return new WaitForSeconds(0.01f);
            }
            tapSmashText.transform.localScale = Vector3.one;
        }
        
        public IEnumerator ScaleUpHeroText()
        {
            for (var scale = 0f; scale <= 1f; scale += 0.1f)
            {
                heroSmashText.transform.localScale = new Vector3(scale,scale,scale);
                yield return new WaitForSeconds(0.01f);
            }
            heroSmashText.transform.localScale = Vector3.one;
        }
    }
}