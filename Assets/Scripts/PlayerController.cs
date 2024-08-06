using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SwkGDCShortCourse.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        public event Action<Vector2> OnMove;
        public event Action OnJump;
        public event Action<bool> OnGrounded;

        [SerializeField]
        private float _grounderDistance = 0.05f;

        [SerializeField]
        private LayerMask _playerLayer;

        [SerializeField]
        private float _groundDeceleration = 60.0f;

        [SerializeField]
        private float _airDeceleration = 30.0f;

        [SerializeField]
        private float _maxSpeed = 14.0f;

        [SerializeField]
        private float _acceleration = 120.0f;

        [SerializeField]
        private float _groundingForce = -1.5f;

        [SerializeField]
        private float _fallAcceleration = 110.0f;

        [SerializeField]
        private float _maxFallSpeed = 40.0f;

        [SerializeField]
        private float _jumpPower = 36.0f;

        private Vector2 _move;

        private Rigidbody2D _rigidbody;
        private CircleCollider2D _collider;
        private Vector2 _velocity;

        private bool _grounded;

        private bool _isJumped;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CircleCollider2D>();
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();

            ApplyMovement();
        }

        public void OnPlayerMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
            OnMove?.Invoke(_move);
        }

        public void OnPlayerJump(InputAction.CallbackContext context)
        {
            if (context.started) _isJumped = true;
        }

        public void OnPlayerPause(InputAction.CallbackContext context)
        {
            if (context.started) GameManager.Instance.SetPause();
        }

        private void CheckCollisions()
        {
            bool groudHit = Physics2D.CircleCast(_collider.bounds.center, _collider.radius, Vector2.down, _grounderDistance, ~_playerLayer);
            bool ceilingHit = Physics2D.CircleCast(_collider.bounds.center, _collider.radius, Vector2.up, _grounderDistance, ~_playerLayer);

            if (ceilingHit) _velocity.y = Mathf.Min(0.0f, _velocity.y);

            if (!_grounded && groudHit)
            {
                _grounded = true;
                OnGrounded?.Invoke(true);
            }
            else if (_grounded && !groudHit)
            {
                _grounded = false;
                OnGrounded?.Invoke(false);
            }
        }

        private void HandleJump()
        {
            if (!_isJumped) return;

            if (_grounded) _velocity.y = _jumpPower;

            OnJump?.Invoke();

            _isJumped = false;
        }

        private void HandleDirection()
        {
            if (_move.x == 0.0f)
            {
                float deceleration = _grounded ? _groundDeceleration : _airDeceleration;
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0.0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, _move.x * _maxSpeed, _acceleration * Time.fixedDeltaTime);
            }
        }

        private void HandleGravity()
        {
            _velocity.y = _grounded && _velocity.y <= 0.0f
                ? _groundingForce
                : Mathf.MoveTowards(_velocity.y, -_maxFallSpeed, _fallAcceleration * Time.fixedDeltaTime);
        }

        private void ApplyMovement() => _rigidbody.velocity = _velocity;
    }
}
