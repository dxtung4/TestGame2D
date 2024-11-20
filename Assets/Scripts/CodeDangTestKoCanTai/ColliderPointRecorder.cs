using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class ColliderPointRecorder : MonoBehaviour
{
    public Animator animator; // Animator để đồng bộ frame
    public List<Vector2[]> recordedFrames = new List<Vector2[]>(); // Lưu các frame

    private PolygonCollider2D polygonCollider;
    public delegate void OnDataRecorded(List<Vector2[]> frames);
    public event OnDataRecorded DataRecorded;

    private void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();

        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }

        // Gọi Coroutine để ghi lại các points của tất cả animation
        StartCoroutine(RecordAllColliderPoints());
    }

    private IEnumerator RecordAllColliderPoints()
    {
        // Lấy tất cả các animation clips từ RuntimeAnimatorController
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        if (controller == null)
        {
            Debug.LogError("Animator Controller is not assigned!");
            yield break;
        }

        // Lặp qua tất cả các animation clips
        foreach (AnimationClip clip in controller.animationClips)
        {
            Debug.Log($"Recording collider points for animation: {clip.name}");

            // Ghi lại các điểm cho từng frame của mỗi animation clip
            float frameRate = clip.frameRate;
            int totalFrames = Mathf.CeilToInt(clip.length * frameRate); // Tổng số frame trong clip

            for (int i = 0; i < totalFrames; i++)
            {
                // Tính thời gian tương ứng với frame
                float normalizedTime = (float)i / totalFrames;
                animator.Play(clip.name, 0, normalizedTime); // Chuyển đến frame cụ thể
                yield return null; // Chờ một frame để cập nhật collider

                // Ghi lại các points của PolygonCollider2D
                if (polygonCollider != null)
                {
                    Vector2[] points = polygonCollider.points;
                    recordedFrames.Add(points);
                }
            }
        }

        // Sau khi ghi dữ liệu collider từ tất cả các animation
        Debug.Log($"Recorded {recordedFrames.Count} frames from all animations.");

        // Gửi sự kiện khi dữ liệu đã được ghi xong
        if (DataRecorded != null)
        {
            DataRecorded(recordedFrames);
        }
    }
}
