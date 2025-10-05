using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts
{
    public class Health : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float invincibilityTime = 1f;
        
        [Header("Events")]
        public UnityEvent OnHealthChanged;
        public UnityEvent OnDeath;
        
        private int currentHealth;
        private bool isInvincible = false;
        private float invincibilityTimer = 0f;
        
        public bool IsDead => currentHealth <= 0;
        public int CurrentHealth => currentHealth;
        
        void Start()
        {
            currentHealth = maxHealth;
            //OnHealthChanged?.Invoke(currentHealth);
        }
        
        void Update()
        {
            // Таймер неуязвимости после получения урона
            if (isInvincible)
            {
                invincibilityTimer -= Time.deltaTime;
                if (invincibilityTimer <= 0f)
                {
                    isInvincible = false;
                }
            }
        }
        
        public void TakeDamage(int damage)
        {
            if (IsDead || isInvincible) return;
            
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            
            //OnHealthChanged?.Invoke(currentHealth);
            
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                // Активируем неуязвимость
                isInvincible = true;
                invincibilityTimer = invincibilityTime;
                
                // Можно добавить мигание спрайта
                // StartCoroutine(FlashSprite());
            }
        }
        
        public void Heal(int healAmount)
        {
            if (IsDead) return;
            
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            
            //OnHealthChanged?.Invoke(currentHealth);
        }
        
        private void Die()
        {
            OnDeath?.Invoke();
            
            // Отключаем управление и физику
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
            
            // Можно добавить анимацию смерти
            // animator.SetTrigger("Die");
            
            // Уведомляем GameManager о смерти игрока
            //GameManager.Instance.PlayerDied();
        }
    }
}