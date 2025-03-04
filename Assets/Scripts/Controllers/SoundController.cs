using UnityEngine;

namespace Runner.Player
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private GameObject _soundController;
        [SerializeField] private AudioClip _menuClip;
        [SerializeField] private AudioClip _levelClip;

        public bool IsLevelRestarted = false;

        private void Awake()
        {
            DontDestroyOnLoad(_soundController);
        }

        private void Play()
        {
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void PlayInMenu()
        {
            _audioSource.clip = _menuClip;
            Play();
        }

        public void PlayInLevel()
        {
            if(!IsLevelRestarted)
            {
                _audioSource.clip = _levelClip;
                Play();
            }
        }

        public bool IsPlayed()
        {
            return _audioSource.isPlaying;
        }
    }
}