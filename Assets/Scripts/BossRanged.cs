using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossRanged : MonoBehaviour, IDamageable
{
    public float moveSpeed = 1f;
    public float attackRange = 2f;
    public int maxHealth = 200;
    public float detectionRange = 5f;

    public float minDistanceToPlayer = 0.5f;  // Khoảng cách tối thiểu mà Boss muốn giữ với Player
    public GameObject projectilePrefab;  // Prefab của đạn
    public Transform firePoint;  // Điểm phát ra đạn (vị trí bắn)
    public int maxProjectiles = 3;       // Số lượng đạn tối đa trên scene

    private List<GameObject> activeProjectiles = new List<GameObject>();  // Danh sách các đạn đang tồn tại
    private int currentHealth;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;

    [Header("Attack Cooldown Settings")]
    public float attackCooldown = 3f; // Thời gian hồi chiêu giữa các lần tấn công
    private float lastAttackTime = -Mathf.Infinity; // Thời điểm tấn công cuối cùng

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        MonsterHealth monsterHealth = GetComponent<MonsterHealth>();

        if (monsterHealth != null)
        {
            monsterHealth.InitializeHealth(maxHealth); // Gọi hàm khởi tạo máu
        }
    }

    private void Update()
    {
        // Kiểm tra HP, nếu chết thì xh animation chết
        if (currentHealth <= 0)
        {
            Die();
            return; 
        }

        // Kiểm tra khoảng cách với Player để quyết định hành động
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
        //Debug.Log("Distance to Player: " + distanceToPlayer);  // Debug Log

        // Tùy thuộc vào khoảng cách, quyết định trạng thái và hành động
        if (distanceToPlayer > detectionRange)
        {
            // Nếu Player ngoài phạm vi 5f, Boss sẽ ở trạng thái Idle
            Idle();
        }
        else if (distanceToPlayer > 3f)
        {
            // Nếu Player trong phạm vi từ 3f - 5f, Boss sẽ chạy về phía Player
            RunTowardsPlayer(distanceToPlayer);
        }
        else if (distanceToPlayer > attackRange)
        {
            // Nếu Player trong phạm vi từ 2f - 3f, Boss sẽ đi bộ về phía Player
            WalkTowardsPlayer(distanceToPlayer);
        }
        else if (Time.time >= lastAttackTime + attackCooldown) // Kiểm tra cooldown trước khi tấn công
        {
            // Nếu Player trong phạm vi tấn công, Boss sẽ tấn công
            Attack();
        }
        else
        {
            // Nếu đang trong cooldown, giữ Boss ở trạng thái Idle hoặc di chuyển
            animator.SetBool("isAttacking", false); // Tắt animation tấn công
        }

        // Quay mặt boss về hướng Player
        Flip();
    }


    private void Idle()
    {
        // Boss đứng yên và ở animation Idle
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }

    private void RunTowardsPlayer(float distanceToPlayer)
    {
        // Di chuyển nhanh về phía Player và ở animation Run
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    private void WalkTowardsPlayer(float distanceToPlayer)
    {
        // Di chuyển chậm về phía Player và ở animation Walk
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }

    private void Attack()
    {   
        // Kiểm tra xem Boss có đang ở trong phạm vi tấn công với Player không
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown) // Kiểm tra cooldown
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);

            // Ghi nhận thời gian tấn công cuối
            lastAttackTime = Time.time;

            // Kích hoạt animation tấn công
            animator.SetBool("isAttacking", true);

            // Giữ khoảng cách tối thiểu 0.5f với Player
            Vector2 direction = (player.position - transform.position).normalized;
            
            // Tính toán khoảng cách hiện tại và giữ khoảng cách tối thiểu
            float currentDistance = Vector2.Distance(transform.position, player.position);
            if (currentDistance > minDistanceToPlayer)
            {
                // Nếu Boss quá xa Player, di chuyển lại gần
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
            else if (currentDistance < minDistanceToPlayer)
            {
                // Nếu Boss quá gần Player, di chuyển ra xa một chút
                transform.Translate(-direction * moveSpeed * Time.deltaTime);
            }
            // Tạo đạn và bắn nếu cần thiết
            if (projectilePrefab && firePoint != null && activeProjectiles.Count < maxProjectiles)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                activeProjectiles.Add(projectile);

                // Tính toán hướng tới Player
                Vector2 attackDirection = (player.position - firePoint.position).normalized;

                // Gửi hướng cho Projectile
                projectile.GetComponent<Projectile>().SetDirection(attackDirection);
            }
        }
        else
        {
        // Nếu Boss không còn trong phạm vi tấn công, ngừng animation "Attacking"
        EndAttack();
        }

    // Loại bỏ các đạn đã bị hủy khỏi danh sách
    activeProjectiles.RemoveAll(p => p == null);
    }
    public void EndAttack()
    {
        // Kết thúc tấn công, quay lại trạng thái ban đầu
        animator.SetBool("isAttacking", false);
    }
    
    private void Flip()
    {
        // Kiểm tra xem player ở bên trái hay bên phải của boss và quay mặt boss cho phù hợp
        if (player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            // Player ở bên trái Boss, quay Boss sang trái
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (player.position.x > transform.position.x && transform.localScale.x < 0)
        {
            // Player ở bên phải Boss, quay Boss sang phải
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Die()
    {
        //animation chết
        animator.SetBool("isDead", true);

        Destroy(gameObject, 2f); // Tự hủy sau 2 giây
    }

    public void TakeDamage(int damage)
    {
        // Giảm HP của boss
        currentHealth -= damage;

        // animation bị tấn công 
        if (animator != null)
        {
            animator.SetTrigger("isHit"); // Kích hoạt animation TakeHit
        }
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
        StartCoroutine(ResetTriggerAfterDelay("isHit", 0.1f)); // Reset trigger
        }
    }

    // Coroutine để reset Trigger
    private IEnumerator ResetTriggerAfterDelay(string triggerName, float delay)
    {   
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger(triggerName);
    }

    // Phương thức để loại bỏ đạn khỏi danh sách khi đạn bị hủy
    public void RemoveProjectileFromList(GameObject projectile)
    {
        activeProjectiles.Remove(projectile);
    }
} 