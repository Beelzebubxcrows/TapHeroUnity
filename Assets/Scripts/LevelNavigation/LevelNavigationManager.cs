using System.Collections.Generic;
using CoreGame;
using Popups;
using UnityEngine;

namespace LevelNavigation
{
    public class LevelNavigationManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] nodeSprites;
        [SerializeField] private Transform nodesParent;
        [SerializeField] private Node node;

        private List<Node> _instantiatedNodes;
        private LevelManager _levelManager;
        private LevelCompletePopup _levelCompletePopup;

        public void Initialise(LevelCompletePopup levelCompletePopup)
        {
            _instantiatedNodes = new List<Node>();
            _levelManager = DependencyManager.DependencyManager.GetInstance<LevelManager>();
            _levelCompletePopup = levelCompletePopup;
        }

        public void PopulateNodes()
        {
            var totalLevels = _levelManager.GetEocLevel();
            var latestLevel = _levelManager.GetLatestLevel();
            for (var level = 1; level <= totalLevels; level++)
            {
                var instantiatedNode = Instantiate(node.gameObject, nodesParent).GetComponent<Node>();
                
                var nodeSprite = nodeSprites[0];
                if (level > latestLevel)
                {
                    nodeSprite = nodeSprites[2];
                }
                if (level == latestLevel)
                {
                    nodeSprite = nodeSprites[1];
                }
                
                instantiatedNode.Initialise(level,nodeSprite,this);
                _instantiatedNodes.Add(instantiatedNode);
            }
        }

        public void OpenLevel(int levelNumber)
        {
           _levelCompletePopup.OpenLevel(levelNumber);
        }

        public void Dispose()
        {
            foreach (var instantiatedNode in _instantiatedNodes)
            {
                Destroy(instantiatedNode.gameObject);
            }
        }
    }
}