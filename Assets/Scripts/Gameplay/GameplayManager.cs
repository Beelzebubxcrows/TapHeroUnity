using System;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour, IDisposable
    {
        
        [SerializeField] private PlayAreaManager playAreaManager;

        public void Initialise()
        {
            playAreaManager.Initialise();
        }

        public void Dispose()
        {
            playAreaManager.Dispose();
        }

        public void StartGamePlayWithoutTransition()
        {
            playAreaManager.StartGamePlayWithoutTransition();
        }

        public void StartGamePlay()
        {
            playAreaManager.StartGamePlayWithTransitionScreen();
        }
    }
}
