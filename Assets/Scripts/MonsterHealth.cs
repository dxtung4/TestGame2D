using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

	public int maxHealth = 200;
    private int currentHealth; 
    public Health_Bar health_Bar;

        void Start()
    {
        // Khởi tạo máu hiện tại bằng máu tối đa
        currentHealth = maxHealth;
        health_Bar.SetMaxHealth(maxHealth);
    }
	public void TakeDamage(int damage)
	{

		currentHealth -= damage;
        health_Bar.SetHealth(currentHealth);

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	void Die()
	{

		Destroy(gameObject);
	}

}