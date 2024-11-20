using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderData : MonoBehaviour
{
    // Danh sách các collider cho Attack1 và Attack2
    public List<Vector2[]> attackColliderFrames;

    private void Awake()
    {
        // Không khởi tạo dữ liệu thủ công nữa, chỉ nhận dữ liệu từ Recorder
        if (attackColliderFrames == null || attackColliderFrames.Count == 0)
        {
            Debug.LogWarning("Collider data is not initialized. Waiting for data from Recorder.");
        }
    }

    // Hàm cập nhật lại dữ liệu collider khi nhận được dữ liệu mới từ Recorder
    public void UpdateColliderData(List<Vector2[]> newFrames)
    {
        if (newFrames != null && newFrames.Count > 0)
        {
            attackColliderFrames = new List<Vector2[]>(newFrames);
            Debug.Log($"Updated AttackColliderData with {newFrames.Count} frames.");
        }
        else
        {
            Debug.LogWarning("Invalid collider frames provided.");
        }
    }

    // Lấy các points collider cho một attackType
    public Vector2[] GetColliderPointsForAttack(int attackType)
    {
        // Đảm bảo rằng dữ liệu đã được khởi tạo trước khi truy cập
        if (attackColliderFrames == null || attackColliderFrames.Count == 0)
        {
            Debug.LogError("Collider data is not initialized yet.");
            return null; // Trả về null nếu chưa có dữ liệu
        }

        if (attackType >= 0 && attackType < attackColliderFrames.Count)
        {
            return attackColliderFrames[attackType];
        }
        else
        {
            Debug.LogError("Invalid attack type.");
            return null;
        }
    }
}
