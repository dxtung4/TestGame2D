using UnityEngine;

public class Trap : MonoBehaviour
{
    // Hàm này được gọi khi một đối tượng với Collider va chạm với bẫy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu đối tượng là Player
        if (collision.CompareTag("Player"))
        {
            // Gọi hàm gây sát thương hoặc kích hoạt hiệu ứng
            Debug.Log("Trap activated!");
            // Gây sát thương hoặc hiệu ứng khác ở đây
        }
    }
}
