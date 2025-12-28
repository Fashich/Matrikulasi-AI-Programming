using UnityEngine;
using System.Collections;

public class Back : MonoBehaviour
{
    [Header("Kamera Target")]
    public Camera targetCamera;
    [Header("Animasi")]
    public float animationDuration = 1.5f;

    private bool hasAnimated = false;

    private Vector3 startPosition = new Vector3(8f, 4.08f, -13.94f);
    private Quaternion startRotation = Quaternion.Euler(-10.934f, 180.709f, 0.172f);
    private Vector3 endPosition = new Vector3(8f, 1.31f, -24.8f);
    private Quaternion endRotation = Quaternion.Euler(-23.774f, 0.761f, 0f);

    public void ResetAnimation()
    {
        hasAnimated = false;
        Debug.Log("Animasi direset!");
    }

    public void StartBackAnimation()
    {
        if (hasAnimated || targetCamera == null)
        {
            Debug.LogWarning("Animasi tidak dimulai (hasAnimated = " + hasAnimated + ")");
            return;
        }

        hasAnimated = true;
        StartCoroutine(AnimateCameraBack());
    }

    private IEnumerator AnimateCameraBack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            targetCamera.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
            targetCamera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetCamera.transform.position = endPosition;
        targetCamera.transform.rotation = endRotation;
    }
}