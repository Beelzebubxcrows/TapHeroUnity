using System;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class CharacterHealthManager : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private float _healthDamage;
        public void Initialise(float levelDataCharacterMaxHealth, float levelDataCharacterDamagePercentage)
        {
            slider.maxValue = levelDataCharacterMaxHealth;
            _healthDamage = (levelDataCharacterDamagePercentage * levelDataCharacterMaxHealth) / 100;
            slider.value = slider.maxValue;
        }
        public void EnableGameplay()
        {
            
        }
        public void DecreaseCharacterHealth()
        {
            var value = Math.Max(slider.value - _healthDamage, 0);
            slider.value = value;
        }
        
        public void Dispose()
        {
            
        }

        public float GetCharactersHealth()
        {
            return slider.value;
        }

        public void StopGamePlay()
        {
        }
        
    }
}