using UnityEngine;

public class Trap : MonoBehaviour
{
     public int trapDamage = 20; // Lượng sát thương của bẫy
    // Hàm này được gọi khi một đối tượng với Collider va chạm với bẫy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu đối tượng là Player
        if (collision.CompareTag("Player"))
        {
            // Gọi hàm gây sát thương hoặc kích hoạt hiệu ứng
            Debug.Log("Trap activated!");
            // Lấy script PlayerHealth từ đối tượng Player
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(trapDamage); // Gọi hàm gây sát thương
            }
        }
    }
}
