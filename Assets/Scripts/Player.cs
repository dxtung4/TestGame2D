using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Biến tốc độ di chuyển
    public float moveSpeed = 1f;
    // Biến lực nhảy
    public float jumpForce = 2f;

    // Biến kiểm tra có ở trên mặt đất không
    private bool isGrounded;

    // Lấy thành phần Rigidbody2D của nhân vật
    private Rigidbody2D rb;

    // Gán LayerMask cho vật cản (mặt đất)
    public LayerMask groundLayer;
    // Đặt vị trí để kiểm tra mặt đất (ví dụ, dưới chân của nhân vật)
    public Transform groundCheck;
    // Khoảng cách để kiểm tra mặt đất
    public float groundCheckRadius = 0.1f;
    // Lấy Collider của nhân vật
    private BoxCollider2D boxCollider;

    private void Start()
    {
        // Lấy Rigidbody2D và BoxCollider2D của nhân vật
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Gọi hàm di chuyển
        Move();

        // Kiểm tra nếu nhân vật đang đứng trên mặt đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Xử lý nhảy nếu nhấn phím Space và đang đứng trên mặt đất
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

    }

    private void Move()
    {
        // Lấy input di chuyển từ bàn phím theo trục X
        float moveX = Input.GetAxisRaw("Horizontal");

        // Đặt vận tốc di chuyển trên trục X
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Đảo ngược hướng của nhân vật nếu di chuyển sang trái
        if (moveX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Jump()
    {
        // Áp dụng lực nhảy cho nhân vật
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnDrawGizmos()
    {
        // Vẽ bán kính kiểm tra mặt đất trong cửa sổ Scene của Unity
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}