using UnityEngine;

public class Spin : MonoBehaviour
{
    [Header("Spinning Settings")]
    [Tooltip("Kecepatan putaran (derajat per detik)")]
    public float spinSpeed = 180f;

    void Update()
    {
        transform.Rotate(
            0f,
            spinSpeed * Time.deltaTime,
            0f,
            Space.Self
        );
    }
}