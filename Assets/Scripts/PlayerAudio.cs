using System.Collections.Generic;
using UnityEngine;

namespace SwkGDCShortCourse.Player
{
    [RequireComponent(typeof(PlayerController), typeof(AudioSource))]
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField]
        private List<AudioClip> _footsteps;

        [SerializeField]
        private AudioClip _jump;

        [SerializeField]
        private AudioClip _land;

        private PlayerController _playerController;
        private AudioSource _audioSource;

        private int _currentFootstepIndex;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        private void Start()
        {
            _playerController.OnJump += OnJump;
            _playerController.OnGrounded += OnGrounded;
        }

        private void OnDestroy()
        {
            _playerController.OnJump -= OnJump;
            _playerController.OnGrounded -= OnGrounded;
        }

        public void PlayFootsteps()
        {
            int i = 0;
            while (i == _currentFootstepIndex)
            {
                i = Random.Range(0, _footsteps.Count);
            }
            _currentFootstepIndex = i;
            _audioSource.volume = 0.25f;
            _audioSource.PlayOneShot(_footsteps[_currentFootstepIndex]);
        }

        private void OnJump() => PlayJump();

        private void OnGrounded(bool isGrounded)
        {
            if (isGrounded) PlayLand();
        }

        private void PlayJump()
        {
            _audioSource.volume = 1.0f;
            _audioSource.PlayOneShot(_jump);
        }

        private void PlayLand()
        {
            _audioSource.volume = 1.0f;
            _audioSource.PlayOneShot(_land);
        }
    }
}
