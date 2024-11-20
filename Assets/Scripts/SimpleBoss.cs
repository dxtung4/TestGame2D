using UnityEngine;

public class SimpleBoss : BossMelee
{
    protected override void StartAttack()
    {
        // Kiểm tra nếu Boss đã hết thời gian hồi chiêu
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            isAttacking = true;
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối
            attackCollider.enabled = true;
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
        }
         else
        {
            // Nếu đang trong cooldown, không tấn công
            animator.SetBool("isAttacking", false);
        }
    }
}
