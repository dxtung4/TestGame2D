using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public int damage = 20; // Sát thương của đòn tấn công
    public float lifespan = 5f; // Thời gian tồn tại của đòn tấn công trước khi biến mất
    private float lifeTimeCounter = 0f; // Biến đếm thời gian sống của đòn tấn công
    private Animator animator;        

    private MixedBoss MixedBoss;           // Lưu trữ tham chiếu đến Boss
     private void Start()
    {
    //Lấy Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator không được gán cho Spell");
        }
    }

    void Update()
    {
        // Tính toán thời gian sống của đòn tấn công
        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter >= lifespan)
        {
            DestroySpell(); // Hủy prefab nếu đã tồn tại quá lâu
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Nếu va chạm với Player, gây sát thương
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            DestroySpell(); // Hủy spell sau khi va chạm
        }
    }

        public void DestroySpell()
    {
        // Gọi hàm từ MixedBoss để loại bỏ khỏi danh sách
        if (MixedBoss != null)
        {
            MixedBoss.RemoveSpellFromList(gameObject);
        }

        Destroy(gameObject); // Hủy Spell
    }

    public void SetMixedBossReference(MixedBoss boss)
    {
        MixedBoss = boss; // Gán tham chiếu MixedBoss
    }
}
