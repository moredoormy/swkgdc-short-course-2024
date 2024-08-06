using SwkGDCShortCourse.Player;
using System;
using UnityEngine;

namespace SwkGDCShortCourse
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameState State { get; private set; }

        public event Action<PlayerController> OnSpawnedPlayer;
        public event Action<PlayerController> OnChangedLevel;
        public event Action OnSavedSpawnPoint;
        public event Action OnWin;

        [SerializeField]
        private PlayerController _playerController;

        [SerializeField]
        private GameObject _pausePanel;

        [SerializeField]
        private GameObject _winPanel;

        [SerializeField]
        private bool _alwaysResetSpawnPoint;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            Vector2 spawnPoint = Vector2.zero;
            if (_alwaysResetSpawnPoint)
            {
                PlayerPrefs.DeleteAll();
            }
            else
            {
                spawnPoint.x = PlayerPrefs.GetFloat("spawnPointX", spawnPoint.x);
                spawnPoint.y = PlayerPrefs.GetFloat("spawnPointY", spawnPoint.y);
            }
            PlayerController playerController = Instantiate(_playerController, spawnPoint, Quaternion.identity);
            OnSpawnedPlayer?.Invoke(playerController);
            State = GameState.play;
        }

        public void InvokeOnChangedLevel(PlayerController playerController) => OnChangedLevel?.Invoke(playerController);

        public void SaveSpawnPoint(Vector2 point)
        {
            PlayerPrefs.SetFloat("spawnPointX", point.x);
            PlayerPrefs.SetFloat("spawnPointY", point.y);
            OnSavedSpawnPoint?.Invoke();
        }


        public void SetWin()
        {
            if (State == GameState.play)
            {
                _winPanel.SetActive(true);
                Time.timeScale = 0.0f;
                PlayerPrefs.DeleteKey("spawnPointX");
                PlayerPrefs.DeleteKey("spawnPointY");
                State = GameState.win;
                OnWin?.Invoke();
            }
        }

        public void SetPause()
        {
            switch (State)
            {
                case GameState.play:
                    _pausePanel.SetActive(true);
                    Time.timeScale = 0.0f;
                    State = GameState.pause;
                    break;
                case GameState.pause:
                    _pausePanel.SetActive(false);
                    Time.timeScale = 1.0f;
                    State = GameState.play;
                    break;
            }
        }

        public void ResetGame() => Time.timeScale = 1.0f;
    }

    public enum GameState
    {
        play,
        pause,
        win
    }
}
