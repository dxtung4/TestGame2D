using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    private int currentHealth; 
    private int maxHealth; // Không khai báo cố định
    public Health_Bar health_Bar; // Tham chiếu đến thanh máu

    public void InitializeHealth(int maxHealthValue)
    {
        maxHealth = maxHealthValue; // Gán giá trị tối đa máu
        currentHealth = maxHealth;

        if (health_Bar != null)
        {
            // Khởi tạo thanh máu cho Monster (không gradient)
            health_Bar.InitializeHealthBar(maxHealth, false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (health_Bar != null)
        {
            // Cập nhật giá trị máu trên thanh
            health_Bar.SetHealth(currentHealth);
        }

        // Kiểm tra xem đối tượng có triển khai IDamageable không
        IDamageable damageable = GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        IDamageable damageable = GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Die();
        }
        else
        {
        Destroy(gameObject); // Hủy đối tượng Monster
        }
    }
}
