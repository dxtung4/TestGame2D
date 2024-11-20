using UnityEngine;

public class ComplexBoss : BossMelee
{
    private int lastAttackType = -1;

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
                attackType = Random.Range(0, 2); // Random giữa 0 và 1
            } while (attackType == lastAttackType);
            lastAttackType = attackType;
            // Cập nhật thời gian lần tấn công cuối
            
            animator.SetInteger("attackType", attackType);
        }
        else
        {
            // Nếu đang trong cooldown, không tấn công
            animator.SetBool("isAttacking", false);
        }
    }
}
