using UnityEngine;

using UnityEngine.SceneManagement; // Import SceneManager để tải lại Scene

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Máu tối đa của Player
    private int currentHealth;  // Lượng máu hiện tại của Player
    public Health_Bar health_Bar;

    void Start()
    {
        // Khởi tạo máu hiện tại bằng máu tối đa
        currentHealth = maxHealth;
        health_Bar.SetMaxHealth(maxHealth);
    }

    // Hàm gây sát thương cho Player
    public void TakeDamage(int damage)
    {
        // Giảm lượng máu hiện tại
        currentHealth -= damage;

        health_Bar.SetHealth(currentHealth);

        // Nếu máu <= 0, gọi hàm Die để xử lý khi Player chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Hàm hồi máu cho Player
    public void Heal(int amount)
    {
        // Tăng máu hiện tại nhưng không vượt quá máu tối đa
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        health_Bar.SetHealth(currentHealth);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap")) // Kiểm tra nếu va chạm là với bẫy
        {
            TakeDamage(20); // Gây sát thương cho Player, giảm 20 máu
        }
         if (collision.gameObject.CompareTag("Monster"))// Kiểm tra va chạm monster
        {
            TakeDamage(30);
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // Gọi hàm Respawn sau khi chết
        Respawn();
    }
    void Respawn()
    {
        // Tải lại Scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

