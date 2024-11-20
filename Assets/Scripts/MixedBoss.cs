using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixedBoss : BossMelee
{
    private int lastAttackType = -1;

    // Thêm tham chiếu tới prefab của đòn tấn công xa (lỗ hổng không gian có bàn tay)
    public GameObject spellPrefabs;
    public float rangedAttackRadius = 3f; // Tầm tấn công xa
    public int maxSpell = 2;       // Số lượng spell tối đa trên scene
    private List<GameObject> activeSpells = new List<GameObject>();  // Danh sách các spell đang tồn tại

    protected override void MoveTowardsPlayer()
    {
        // Tính toán hướng di chuyển
        Vector2 direction = (player.position - transform.position).normalized;

        //đảo ngược hướng di chuyển
        direction.x = -direction.x;

        // Di chuyển về phía Player
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Cập nhật animation
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }
    
    protected override void StartAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown) // Kiểm tra nếu đã hết thời gian hồi chiêu
        {
            isAttacking = true;
            lastAttackTime = Time.time;
            attackCollider.enabled = true;
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);

            // Chọn ngẫu nhiên giữa các kiểu tấn công
            int attackType;
            do
            {
                attackType = Random.Range(0, 2); // Random giữa 0 và 1 (0 là cận chiến, 1 là tấn công xa)
            } while (attackType == lastAttackType);
            lastAttackType = attackType;
            
            animator.SetBool("isAttacking", true);
            // Cập nhật thời gian lần tấn công cuối
            animator.SetInteger("attackType", attackType);

            if (attackType == 0)
            {
                // Cận chiến: Animation được kích hoạt bởi isAttacking và attackType
                Debug.Log("Boss thực hiện Attack 1 (Cận chiến)");
            }
            else  if (attackType == 1)
            {
                // Tấn công xa
                PerformRangedAttack();
                Debug.Log("Boss thực hiện Attack 2 (Tấn công xa)");
            }
        }
        else
        {
            // Nếu đang trong cooldown, không tấn công
            animator.SetBool("isAttacking", false);
        }
    }

    // Phương thức thực hiện đòn tấn công xa
    private void PerformRangedAttack()
    {
        if (activeSpells.Count >= maxSpell)
        {
            Debug.Log("Đã đạt giới hạn tối đa spell, không tạo thêm.");
            return;
        }

        // Chọn một vị trí trong phạm vi tấn công xa của boss
            Vector2 spellPosition = new Vector2(
            player.position.x, // Đạn xuất hiện gần trục x của player
            player.position.y + 0.5f // Nằm phía trên đầu player một khoảng 
        );

        // Tạo prefab spell ở vị trí tính toán
        GameObject SpellInstance = Instantiate(spellPrefabs, spellPosition, Quaternion.identity);

        /// Kiểm tra xem prefab có chứa component Animator không
        Animator SpellAnimator = SpellInstance.GetComponent<Animator>();

        if (Vector2.Distance(transform.position, spellPosition) > rangedAttackRadius)
        {
            Debug.LogWarning("Vị trí spell vượt quá phạm vi tấn công xa.");
            return;
        }

        // Liên kết script Spell với MixedBoss để quản lý
        Spell spellScript = SpellInstance.GetComponent<Spell>();
        if (spellScript != null)
        {
            spellScript.SetMixedBossReference(this);
            activeSpells.Add(SpellInstance); // Thêm vào danh sách spell đang tồn tại
        }
    }

    // Phương thức để loại bỏ đạn khỏi danh sách khi đạn bị hủy
    public void RemoveSpellFromList(GameObject spell)
    {
        if (activeSpells.Contains(spell))
        {
            activeSpells.Remove(spell);
            Debug.Log("Spell đã bị loại bỏ khỏi danh sách.");
        }
    }
}
