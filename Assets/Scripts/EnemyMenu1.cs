using UnityEngine;

[RequireComponent(typeof(Transform))]
public class EnemyMenu1 : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Total duration of one full cycle (12 seconds)")]
    public float totalDuration = 12f;

    [Header("Keyframes")]
    [SerializeField] private Vector3 startPos = new Vector3(2.61f, 10.93f, -7.51f);
    [SerializeField] private Vector3 midPos = new Vector3(2.84f, 9.19f, -7.51f);
    [SerializeField] private Vector3 startRot = new Vector3(0f, 0f, 180f);
    [SerializeField] private Vector3 midRot = new Vector3(0f, 0f, 42.993f);

    void Update()
    {
        float cycleTime = Time.time % totalDuration;

        if (cycleTime < 2f)
        {
            // 0-2 detik: Transisi ke posisi tengah
            float progress = cycleTime / 2f;
            transform.position = Vector3.Lerp(startPos, midPos, progress);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, midRot, progress));
        }
        else if (cycleTime < 6f)
        {
            // 2-6 detik: Tahan di posisi tengah
            transform.position = midPos;
            transform.rotation = Quaternion.Euler(midRot);
        }
        else
        {
            // 6-12 detik: Kembali ke posisi awal
            float progress = (cycleTime - 6f) / 6f;
            transform.position = Vector3.Lerp(midPos, startPos, progress);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(midRot, startRot, progress));
        }
    }
}