using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    private PolygonCollider2D attackCollider;  // Collider cho tấn công
    private AttackColliderData attackColliderData;  // Tham chiếu tới dữ liệu collider
    public ColliderPointRecorder colliderRecorder;

    private void Start()
    {
        // Lấy PolygonCollider2D gắn với GameObject AttackCollider
        attackCollider = transform.Find("AttackCollider")?.GetComponent<PolygonCollider2D>();

        // Kiểm tra nếu không tìm thấy collider
        if (attackCollider == null)
        {
            Debug.LogError("PolygonCollider2D not found on AttackCollider object.");
            return;
        }

        // Ban đầu tắt collider
        attackCollider.enabled = false;

        // Lấy dữ liệu từ AttackColliderData
        attackColliderData = GetComponent<AttackColliderData>();

        if (attackColliderData == null)
        {
            Debug.LogError("AttackColliderData not found on " + gameObject.name);
        }

        // Đảm bảo rằng ColliderPointRecorder đã được gán
        if (colliderRecorder == null)
        {
            Debug.LogError("ColliderPointRecorder not assigned.");
            return;
        }

        // Đăng ký sự kiện nhận dữ liệu sau khi recorder ghi xong
        colliderRecorder.DataRecorded += OnColliderDataRecorded;
    }

    // Khi dữ liệu từ ColliderPointRecorder được ghi xong, cập nhật vào AttackColliderData
    private void OnColliderDataRecorded(List<Vector2[]> frames)
    {
        if (attackColliderData != null)
        {
            attackColliderData.UpdateColliderData(frames);
        }
        else
        {
            Debug.LogError("AttackColliderData is not found.");
        }
    }

    // Hàm để thay đổi collider theo loại tấn công
    public void ChangeColliderForAttack(int attackType)
    {
        if (attackColliderData == null)
        {
            Debug.LogError("AttackColliderData is not initialized.");
            return;
        }

        // Kiểm tra xem dữ liệu đã được khởi tạo chưa
        if (attackColliderData.attackColliderFrames == null || attackColliderData.attackColliderFrames.Count == 0)
        {
            Debug.LogError("Collider data is not initialized yet.");
            return;
        }
        Vector2[] colliderPoints = attackColliderData.GetColliderPointsForAttack(attackType);

        if (colliderPoints != null)
        {
            attackCollider.points = colliderPoints; // Cập nhật points của collider
            attackCollider.enabled = true;  // Bật collider
        }
        else
        {
            Debug.LogWarning("No collider points found for attack type " + attackType);
        }
    }

    // Hàm tắt collider sau khi tấn công xong
    public void EndAttack()
    {
        attackCollider.enabled = false;  // Tắt collider
    }

    // Đảm bảo hủy đăng ký sự kiện khi không còn sử dụng
    private void OnDestroy()
    {
        if (colliderRecorder != null)
        {
            colliderRecorder.DataRecorded -= OnColliderDataRecorded;
        }
    }
}
