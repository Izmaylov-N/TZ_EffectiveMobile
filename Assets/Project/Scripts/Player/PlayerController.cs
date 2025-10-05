using System;
using Project.Scripts;
using UnityEngine;

namespace Project.Scripts
{
    [Serializable]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float _moveSpeed = 4f;

        [SerializeField] private float _jumpForce = 4f;
        [SerializeField] private float _groundCheckDistance = 0.1f;

        [Header("Components")] [SerializeField]
        private LayerMask _groundLayerMask;

        [SerializeField] private Transform _groundCheckPoint;

        [SerializeField] private Rigidbody2D _rigidbody2D;

        // Компоненты
        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        // Состояние игрока
        private bool _isGrounded;
        private bool _isFacingRight = true;
        private float _moveDirection;

        // Геттеры для других скриптов
        public float MoveDirection => _moveDirection;
        public bool IsGrounded => _isGrounded;
        public bool IsFacingRight => _isFacingRight;

        void Start()
        {
            // Получаем компоненты
            _rigidbody2D = GetComponent<Rigidbody2D>();

            // Проверяем обязательные компоненты
            if (_rigidbody2D == null)
                Debug.LogError("Rigidbody2D не найден на игроке!");
            if (_groundCheckPoint == null)
                Debug.LogError("GroundCheckPoint не назначен в инспекторе!");
        }

        void Update()
        {
            GetInput();
            CheckGrounded();

            // Прыжок обрабатываем в Update для большей отзывчивости
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                Jump();
            }
        }

        void FixedUpdate()
        {
            Move();
        }

        /// <summary>
        /// Получаем ввод от игрока
        /// </summary>
        private void GetInput()
        {
            _moveDirection = Input.GetAxisRaw("Horizontal");
        }

        /// <summary>
        /// Обрабатываем движение игрока
        /// </summary>
        private void Move()
        {
            // Устанавливаем velocity для движения
            _rigidbody2D.linearVelocity = new Vector2(_moveDirection * _moveSpeed, _rigidbody2D.linearVelocity.y);

            // Поворачиваем персонажа в направлении движения
            if (_moveDirection > 0 && !_isFacingRight)
            {
                //FlipSprite();
            }
            else if (_moveDirection < 0 && _isFacingRight)
            {
                //FlipSprite();
            }
        }

        /// <summary>
        /// Обрабатываем прыжок
        /// </summary>
        private void Jump()
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, _jumpForce);

            // Можно добавить звук прыжка
            // AudioManager.Instance.PlaySound("Jump");
        }

        /// <summary>
        /// Проверяем, стоит ли игрок на земле
        /// </summary>
        private void CheckGrounded()
        {
            // Raycast для проверки земли
            RaycastHit2D hit = Physics2D.Raycast(
                _groundCheckPoint.position,
                Vector2.down,
                _groundCheckDistance,
                _groundLayerMask
            );

            _isGrounded = hit.collider != null;

            // Визуализация луча в редакторе
            Debug.DrawRay(_groundCheckPoint.position, Vector2.down * _groundCheckDistance,
                _isGrounded ? Color.green : Color.red);
        }

        /// <summary>
        /// Поворачиваем персонажа
        /// </summary>

        /// <summary>
        /// Проверяем, наступаем ли мы на врага
        /// </summary>
        private bool CheckIfStompingEnemy(Collision2D collision)
        {
            // Проверяем все точки контакта
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Если точка контакта находится сверху от врага и игрок движется вниз
                if (contact.normal.y > 0.7f && _rigidbody2D.linearVelocity.y < 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
