using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager để tải lại Scene

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Máu tối đa của Player
    private int currentHealth;  // Lượng máu hiện tại của Player
    public Health_Bar health_Bar; // Tham chiếu đến thanh máu
    private Animator animator; // Tham chiếu đến Animator

    void Start()
    {
        currentHealth = maxHealth; // Khởi tạo máu tối đa

        if (health_Bar != null)
        {
            // Khởi tạo thanh máu cho Player (có gradient)
            health_Bar.InitializeHealthBar(maxHealth, true);
        }

        animator = GetComponent<Animator>();
    }

    // Hàm gây sát thương cho Player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (health_Bar != null)
        {
            health_Bar.SetHealth(currentHealth); // Cập nhật thanh máu
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Hàm hồi máu cho Player
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (health_Bar != null)
        {
            health_Bar.SetHealth(currentHealth); // Cập nhật thanh máu
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");

        // Kích hoạt trạng thái chết trong Animator
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        // Gọi hàm Respawn sau khi chết
        Invoke(nameof(Respawn), 2f); // Chờ 2 giây trước khi hồi sinh
    }

    void Respawn()
    {
        // Tải lại Scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
