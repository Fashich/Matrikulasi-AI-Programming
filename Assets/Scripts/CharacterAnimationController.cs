using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [Header("Body Animation Settings")]
    public float bobAmount = 0.02f;
    public float bobSpeed = 2f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.localPosition = originalPosition + new Vector3(0, newY, 0);
    }
}