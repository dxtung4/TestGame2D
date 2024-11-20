using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossMelee : MonoBehaviour, IDamageable
{
    [Header("Boss Stats")]
    public float moveSpeed = 1f;
    public int maxHealth = 300;
    public int attackDamage = 20;  // Sát thương tấn công của boss
    public float attackRange = 1f;
    public float detectionRange = 3f;

    [Header("Animation")]
    public Animator animator; 
    protected Transform player;
    protected int currentHealth;
    protected bool isAttacking = false;
    public Collider2D attackCollider; // Tham chiếu tới Collider2D tấn công


    [Header("Attack Cooldown Settings")]
    public float attackCooldown = 1f; // Thời gian hồi chiêu giữa các lần tấn công
    protected float lastAttackTime = -Mathf.Infinity; // Thời điểm tấn công cuối cùng

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (attackCollider == null)
        {
            Debug.LogError("Attack Collider is not assigned!");
        }
        else
        {
            attackCollider.enabled = false; // Tắt collider khi không tấn công
        }

        MonsterHealth monsterHealth = GetComponent<MonsterHealth>();

        if (monsterHealth != null)
        {
            monsterHealth.InitializeHealth(maxHealth); // Gọi hàm khởi tạo máu
        }
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0) return;

        // Kiểm tra khoảng cách với Player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)// Chỉ tấn công khi hết cooldown
        {
            if (distanceToPlayer <= attackRange) 
            {
                // Trong phạm vi tấn công
                StartAttack();
            }
            else if (distanceToPlayer <= detectionRange)
            {
                // Trong phạm vi phát hiện nhưng ngoài tầm tấn công
                MoveTowardsPlayer();
            }
            else
            {
                // Ngoài phạm vi phát hiện
                Idle();
            }
        }

        // Xoay mặt Boss về phía Player
        FlipTowardsPlayer();
    }

    protected void MoveTowardsPlayer()
    {
        // Di chuyển về phía Player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Cập nhật animation
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    protected void Idle()
    {
        // Dừng di chuyển
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }

    protected abstract void StartAttack();

   // Thực hiện khi Collider va chạm với Player
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu va chạm với Player
        if (other.CompareTag("Player"))
        {
            // Lấy script PlayerHealth từ đối tượng Player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage); // Gọi hàm gây sát thương
            }
        }
    }

    public void EndAttack()
    {
        // Kết thúc tấn công, quay lại trạng thái ban đầu
        isAttacking = false;
        animator.SetBool("isAttacking", false);

        // Tắt collider tấn công
        attackCollider.enabled = false;
    }

    protected void FlipTowardsPlayer()
    {
        if (player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            // Quay mặt trái
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (player.position.x > transform.position.x && transform.localScale.x < 0)
        {
            // Quay mặt phải
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void TakeDamage(int damage)
    {
        // Giảm máu Boss
        currentHealth -= damage;

        if (animator != null)
        {
            animator.SetTrigger("getHit"); // Kích hoạt animation TakeHit
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ResetTriggerAfterDelay("getHit", 0.1f)); // Reset trigger
        }
    }

    // Coroutine để reset Trigger
    private IEnumerator ResetTriggerAfterDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger(triggerName);
    }

    public virtual void Die()
    {
        // Chết
        animator.SetBool("isDead", true);
        Destroy(gameObject, 2f); // Hủy đối tượng sau 2 giây
    }
}
