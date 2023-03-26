using System;
using System.Collections;
using System.Threading.Tasks;
using CoreGame;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

namespace Character
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private Image[] clickEffects;
        
        private PlayAreaManager _playAreaManager;
        private GameAudioManager _audioManager;
        private float _characterSpeed;
        private int _continuousSmash;
        
        public void Initialise(PlayAreaManager playAreaManager, float characterSpeed)
        {
            _continuousSmash = 0;
            _characterSpeed = characterSpeed;
            _playAreaManager = playAreaManager;
            _audioManager = DependencyManager.DependencyManager.GetInstance<GameAudioManager>();
            button.onClick.AddListener(OnClick);
            ToggleClickEffect(false);
            SetInteractable(false);
        }

        private void ToggleClickEffect(bool effectState)
        {
            foreach (var effect in clickEffects)
            {
                var calculatedScale = GetScale();
                effect.transform.localScale = new Vector3(calculatedScale,calculatedScale,calculatedScale);
                effect.enabled = effectState;
            }
        }

        private float GetScale()
        {
            return (_continuousSmash - 1f) * 0.5f + 1.0f;
        }

        private void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
            image.color = interactable ? Color.white : Color.grey;
        }

        private void OnClick()
        {
            _continuousSmash++;
            _audioManager.PlayClickAudio();
            _playAreaManager.DeductCharacterHealth();
            StartCoroutine(nameof(OnClickCharacterAnimation));
        }

        private IEnumerator OnClickCharacterAnimation()
        {
            ToggleClickEffect(true);
            yield return new WaitForSeconds(0.15f);
            ToggleClickEffect(false);
        }

        public void DummyClickOnCharacter()
        {
            StartCoroutine(nameof(OnClickCharacterAnimation));
        }

        public async void StartMovingRandomly()
        {
            SetInteractable(true);
            
            var r = new Random(23423);
            while (_playAreaManager.ShouldContinueGame())
            {
                try
                {
                    var x = r.NextInt(-500, 500);
                    var y = r.NextInt(-300, 300);
                    _audioManager.PlayTeleportAudio();
                    gameObject.transform.localPosition = new Vector2(x, y);
                    await Task.Delay(TimeSpan.FromMilliseconds(_characterSpeed));
                    _continuousSmash = 0;
                }
                catch
                {
                    break;
                }
            }
        }

        public void Dispose()
        {
            button.onClick.RemoveListener(OnClick);
        }
    }
}