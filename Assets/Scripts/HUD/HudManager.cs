using System;
using System.Collections;
using DataManager;
using TMPro;
using UnityEngine;


namespace HUD
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private CharacterHealthManager characterHealthManager;
        [SerializeField] private VictimHealthManager victimHealthManager;
        [SerializeField] private TMP_Text levelNumber;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private const string LEVEL_NUMBER_FORMAT = "Level - {0}";
        public void Initialise(LevelData levelData)
        {
            characterHealthManager.Initialise(levelData.CharacterMaxHealth, levelData.CharacterDamagePercentage);
            victimHealthManager.Initialise(levelData.CityMaxHealth,levelData.CityDamagePercentage);
            levelNumber.text = string.Format(LEVEL_NUMBER_FORMAT, levelData.LevelNumber);
            canvasGroup.alpha = 0;
        }

        #region CHARACTER WRAPPER FUNCTIONS
        public float GetCharactersHealth()
        {
            return characterHealthManager.GetCharactersHealth();
        }
        public void DecreaseCharacterHealth()
        {
            characterHealthManager.DecreaseCharacterHealth();
        }

        public void SetCharacterHealth()
        {
            
        }
        #endregion
        
        #region VICTIMS WRAPPER FUNCTIONS
        public float GetVictimsHealth()
        {
            return victimHealthManager.GetVictimsHealth();
        }

        public void RegisterForVictimHealthChanged(Action<float> callback)
        {
            victimHealthManager.RegisterForVictimHealthChanged(callback);
        }
        public void UnregisterForVictimHealthChanged(Action<float> callback)
        {
            victimHealthManager.UnregisterForVictimHealthChanged(callback);
        }
        public void SetVictimHealth()
        {
            
        }
        
        #endregion


        public void Dispose()
        {
            characterHealthManager.Dispose();
            victimHealthManager.Dispose();
        }

        public void EnableGameplay()
        {
            StartCoroutine(nameof(StartFadeInAnimation));
            characterHealthManager.EnableGameplay();
            victimHealthManager.EnableGameplay();
        }

        private IEnumerator StartFadeInAnimation()
        {
            for (var alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(0.05f);
            }
            canvasGroup.alpha = 1;
            
        }
        
        private IEnumerator StartFadeOutAnimation()
        {
            for (var alpha = 1f; alpha >=0f; alpha -= 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(0.05f);
            }
            canvasGroup.alpha = 0; 
            
        }


        public void StopGamePlay()
        {
            StartCoroutine(nameof(StartFadeOutAnimation));
            characterHealthManager.StopGamePlay();
            victimHealthManager.StopGamePlay();
        }

        public float GetMaxVictimHealth()
        {
            return victimHealthManager.GetMaxVictimHealth();
        }
    }
}