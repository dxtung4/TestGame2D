using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1f;
    public float jumpForce = 2f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    private bool isGrounded;

    [Header("Attack Settings")]
    public int attackDamage = 10; // Sát thương khi tấn công
    public Collider2D attackCollider; // Collider kiểm tra va chạm tấn công
    public float attackDuration = 0.2f; // Thời gian hoạt động của Collider

    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (attackCollider == null)
        {
            Debug.LogError("Attack Collider is not assigned!");
        }
        else
        {
            attackCollider.enabled = false; // Tắt collider khi không tấn công
        }
    }

    private void Update()
    {
        // Chỉ di chuyển nếu không đang tấn công
        if (!isAttacking)
        {
            Move();
            JumpCheck();
        }

        // Gọi tấn công khi nhấn J
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        if (moveX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveX));
    }

    private void JumpCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        animator.SetBool("isJumping", !isGrounded);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private IEnumerator Attack()
    {
        isAttacking = true; // Chặn di chuyển khi tấn công
        animator.SetTrigger("isAttacking"); // Kích hoạt animation tấn công

        attackCollider.enabled = true; // Bật collider để kiểm tra va chạm

        // Đợi thời gian tấn công (theo thời gian duration của attack)
        yield return new WaitForSeconds(attackDuration);
        attackCollider.enabled = false; // Tắt collider sau khi animation kết thúc     
        animator.ResetTrigger("isAttacking");
        isAttacking = false; // Cho phép di chuyển lại
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attackCollider.enabled && other.CompareTag("Monster"))
        {
            MonsterHealth monsterHealth = other.GetComponent<MonsterHealth>();
            if (monsterHealth != null)
            {
                monsterHealth.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
