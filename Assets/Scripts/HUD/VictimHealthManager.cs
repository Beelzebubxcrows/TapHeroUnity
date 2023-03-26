using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace HUD
{
    public class VictimHealthManager : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private TimeManager _timeManager;
        private Action<float> _onVictimHealthChanged;
        private float _damage;
        public void Initialise(float maxHealth, float damagePercentage)
        {
            slider.maxValue = maxHealth;
            slider.value = slider.maxValue;
            _damage = (damagePercentage * maxHealth) / 100;
            _timeManager = DependencyManager.DependencyManager.GetInstance<TimeManager>();
        }

        public void EnableGameplay()
        {
            _timeManager.RegisterForEachSecondElapsed(DecreaseVictimsHealth);
        }

        private void DecreaseVictimsHealth()
        {
            var value = Math.Max(slider.value - _damage, 0);
            slider.value = value;
            _onVictimHealthChanged?.Invoke(slider.value);
        }
        public void StopGamePlay()
        {
            _timeManager.UnregisterForEachSecondElapsed(DecreaseVictimsHealth);
        }

        public void RegisterForVictimHealthChanged(Action<float> callback)
        {
            _onVictimHealthChanged += callback;
        }

        
        public void UnregisterForVictimHealthChanged(Action<float> callback)
        {
            _onVictimHealthChanged -= callback;
        }
        
        public float GetVictimsHealth()
        {
            return slider.value;
        }
        
        public void Dispose()
        {
        }

        public float GetMaxVictimHealth()
        {
            return slider.maxValue;
        }
    }
    
}