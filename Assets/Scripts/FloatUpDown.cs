using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    [Header("Floating Settings")]
    [Tooltip("Jarak maksimal gerakan (dalam satuan Unity)")]
    public float amplitude = 1.5f;

    [Tooltip("Kecepatan gerakan (semakin besar, semakin cepat)")]
    public float speed = 2f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }
}