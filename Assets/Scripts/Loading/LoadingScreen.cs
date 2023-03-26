using System;
using System.Collections;
using System.Collections.Generic;
using CoreGame;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Loading
{
    public class LoadingScreen : MonoBehaviour,IDisposable
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject smash;
        private Action _callBack;
        private List<GameObject> _spawnedSmashes;
        private GameAudioManager _audioManager;

        public void Initialise()
        {
            _spawnedSmashes = new List<GameObject>();
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
            for (var alpha = 0f; alpha <= 1f; alpha = alpha+0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(0.05f);
            }

            canvasGroup.alpha = 1;
            
            var r = new Random(23423);
            var smashImage = 5;
            while (smashImage-- > 0)
            {
                var go = Instantiate(smash, transform);
                _spawnedSmashes.Add(go);
                var x = r.NextInt(-500, 500);
                var y = r.NextInt(-300, 300);
                go.transform.localPosition = new Vector2(x, y);
                _audioManager.PlayClickAudio();
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitForSeconds(1f);
            _callBack?.Invoke();
            for (var alpha = 1f; alpha >=0f; alpha = alpha - 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(0.05f);
            }
            canvasGroup.alpha = 0;  
            foreach (var go in _spawnedSmashes)
            {
                Destroy(go);
            }
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
        }
    }
}