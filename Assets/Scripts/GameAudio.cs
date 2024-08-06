using UnityEngine;

namespace SwkGDCShortCourse.Point
{
    [RequireComponent(typeof(AudioSource))]
    public class GameAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _savedSpawnPoint;

        [SerializeField]
        private AudioClip _win;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        private void Start()
        {
            GameManager.Instance.OnSavedSpawnPoint += PlaySavedSpawnPoint;
            GameManager.Instance.OnWin += PlayWin;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnSavedSpawnPoint -= PlaySavedSpawnPoint;
            GameManager.Instance.OnWin -= PlayWin;
        }

        private void PlaySavedSpawnPoint() => _audioSource.PlayOneShot(_savedSpawnPoint);

        private void PlayWin() => _audioSource.PlayOneShot(_win);
    }
}
