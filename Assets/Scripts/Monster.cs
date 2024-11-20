using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Monster : MonoBehaviour
{

     public int attackDamage = 10; // Lượng sát thương của quái vật
    public float moveSpeed = 2f; // Tốc độ di chuyển của quái vật
    public Transform groundDetection; // Điểm phát tia để kiểm tra tường hoặc mép
    private bool movingRight = true; // Đang di chuyển về bên phải hay không
    public int maxHealth = 100;

    private void Start()
    {
        MonsterHealth monsterHealth = GetComponent<MonsterHealth>();

        if (monsterHealth != null)
        {
            monsterHealth.InitializeHealth(maxHealth); // Gọi hàm khởi tạo máu
        }
    }
    
    void Update()
    {
        Move(); // Gọi hàm di chuyển quái vật
    }

    void Move()
    {
        // Di chuyển quái vật theo hướng hiện tại
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
        // Di chuyển về bên trái
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.5f);

         // Nếu không có gì phía trước (tường hoặc không có nền), quái sẽ quay lại
        if (groundInfo.collider == false)
        {
                Flip();
        }
    }
    
    // Hàm để lật hướng của sprite quái vật khi đổi chiều
    void Flip()
    {
        movingRight = !movingRight; // Đổi hướng
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Đảo trục X
        transform.localScale = localScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu va chạm với Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lấy script PlayerHealth từ đối tượng Player
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage); // Gọi hàm gây sát thương
            }
        }
    }
}
