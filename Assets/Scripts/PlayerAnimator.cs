using UnityEngine;

namespace SwkGDCShortCourse.Player
{
    [RequireComponent(typeof(PlayerController), typeof(SpriteRenderer), typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerController _playerController;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _playerController.OnMove += OnMove;
            _playerController.OnGrounded += OnGrounded;
        }

        private void OnDestroy()
        {
            _playerController.OnMove -= OnMove;
            _playerController.OnGrounded -= OnGrounded;
        }

        private void OnMove(Vector2 value)
        {
            if (value.x != 0.0f) _spriteRenderer.flipX = value.x < 0.0f;
            _animator.SetFloat("Speed", Mathf.Abs(value.x));
        }

        private void OnGrounded(bool _isGrounded) => _animator.SetBool("Jump", !_isGrounded);
    }
}
