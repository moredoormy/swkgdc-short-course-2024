using SwkGDCShortCourse.Player;
using UnityEngine;

namespace SwkGDCShortCourse
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private float _distanceBetweenLevel = 10.0f;

        [SerializeField]
        private float _speed = 24.0f;

        private Camera _camera;

        private Vector2 _targetCenter;
        private bool _isMoving;
        private float _targetCameraPositionY;

        private void Awake() => _camera = Camera.main;

        private void Start()
        {
            _targetCenter = Vector2.zero;
            GameManager.Instance.OnSpawnedPlayer += FollowPlayer;
            GameManager.Instance.OnChangedLevel += FollowPlayer;
        }

        private void FixedUpdate()
        {
           if (_isMoving)
           {
                float currentCameraPositionY = Mathf.MoveTowards(_camera.transform.position.y, _targetCameraPositionY, _speed * Time.fixedDeltaTime);
                _camera.transform.position = new Vector3(_camera.transform.position.x, currentCameraPositionY, _camera.transform.position.z);
                if (currentCameraPositionY == _targetCameraPositionY)
                {
                    _isMoving = false;
                }
           }
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnSpawnedPlayer -= FollowPlayer;
            GameManager.Instance.OnChangedLevel -= FollowPlayer;
        }

        private void FollowPlayer(PlayerController playercontroller)
        {
            bool isInside = false;
            float divided = _distanceBetweenLevel / 2.0f; 
            while (!isInside)
            {
                float upper = _targetCenter.y + divided;
                float lower = _targetCenter.y - divided;
                Vector2 position = playercontroller.transform.position;
                if (position.y >= lower && position.y <= upper)
                {
                    isInside = true;
                    MoveCamera();
                }
                else
                {
                    if (position.y < lower)
                    {
                        _targetCenter = new(_targetCenter.x, _targetCenter.y - _distanceBetweenLevel);
                    }
                    else if (position.y > upper)
                    {
                        _targetCenter = new(_targetCenter.x, _targetCenter.y + _distanceBetweenLevel);
                    }
                }
            }
        }

        private void MoveCamera()
        {
            _targetCameraPositionY = _targetCenter.y;
            _isMoving = true;
        }
    }
}
