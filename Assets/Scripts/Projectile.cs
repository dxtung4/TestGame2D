using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;             // Tốc độ đạn
    public int damage = 20;             // Sát thương của đạn
    public float maxRange = 2f;       // Tầm bắn tối đa của đạn
    private BossRanged bossScript;            // Lưu trữ tham chiếu đến Boss
    private Vector3 spawnPosition;      // Vị trí ban đầu của đạn
    private Vector2 moveDirection;      // Hướng di chuyển
    private Animator animator;          // Animator của đạn
    private bool hasExploded = false;   // Đảm bảo chỉ nổ 1 lần

    private void Start()
    {
        // Lấy tham chiếu đến Boss
        GameObject boss = GameObject.FindGameObjectWithTag("Monster");
        if (boss != null)
        {
            bossScript = boss.GetComponent<BossRanged>();
        }

        // Lưu vị trí ban đầu của đạn
        spawnPosition = transform.position;

        // Gán vận tốc di chuyển
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = moveDirection * speed;
        }

        // Lấy Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator không được gán cho Projectile!");
        }
    }

    private void Update()
    {
        // Kiểm tra nếu đạn vượt quá khoảng cách tối đa
        if (Vector3.Distance(spawnPosition, transform.position) > maxRange)
        {
            Explode();
        }
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized; // Đảm bảo hướng được chuẩn hóa

        // Tính toán góc xoay của đạn
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return; // Ngăn đạn nổ lại nhiều lần

        if (other.CompareTag("Player"))
        {
            // Nếu va chạm với Player, gây sát thương
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Explode();
        }
        else if (other.CompareTag("Ground"))
        {
            // Nếu va chạm với đất
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return; // Đảm bảo chỉ nổ một lần
        hasExploded = true;

        // Ngừng di chuyển
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // Kích hoạt animation nổ
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }
        else
        {
            Destroy(gameObject); // Nếu không có Animator, hủy trực tiếp
        }
    }

    // Gọi từ Animation Event
    public void DestroyProjectile()
    {
        Destroy(gameObject); // Hủy đạn khi animation nổ kết thúc
    }

    private void OnDestroy()
    {
        // Khi đạn bị hủy, loại bỏ khỏi danh sách trong Boss
        if (bossScript != null)
        {
            bossScript.RemoveProjectileFromList(gameObject);
        }
    }
}