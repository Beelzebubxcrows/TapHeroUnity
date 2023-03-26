using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelNavigation
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text levelNumberTextComponent;
        
        private  int _levelNumber;
        private  Sprite _sprite;
        private LevelNavigationManager _levelNavigationManager;

        public void  Initialise(int levelNumber, Sprite sprite, LevelNavigationManager levelNavigationManager)
        {
            _levelNavigationManager = levelNavigationManager;
            _levelNumber = levelNumber;
            _sprite = sprite;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            levelNumberTextComponent.text = _levelNumber.ToString();
            image.sprite = _sprite;
        }

        public void OnClick()
        {
            _levelNavigationManager.OpenLevel(_levelNumber);
        }
    }
}