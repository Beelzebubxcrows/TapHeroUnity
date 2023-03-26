using System;
using UnityEngine;

namespace CoreGame
{
    public class GameAudioManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip gameWon;
        [SerializeField] private AudioClip gameLoss;
        [SerializeField] private AudioClip clickAudio;
        [SerializeField] private AudioClip escapeAudio;
        public void PlayTeleportAudio()
        {
            audioSource.PlayOneShot(escapeAudio);
        }
        
        public void PlayGameLossAudio()
        {
            audioSource.clip = gameLoss;
            audioSource.Play();
        }
        
        public void PlayGameWonAudio()
        {
            audioSource.clip = gameWon;
            audioSource.Play();
        }
        
        public void PlayClickAudio()
        { ;
            audioSource.PlayOneShot(clickAudio);
        }

        public void Dispose()
        {
        }
    }
}