using UnityEngine;
using System.Collections;

public class ForwardLampCarUnflickering : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float totalDuration = 5f;
    [SerializeField] private float flickerDuration = 1f;
    [SerializeField] private float flickerInterval = 0.05f;

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object belum diassign di Inspector!");
            return;
        }
        StartCoroutine(FlickerLoop());
    }

    private IEnumerator FlickerLoop()
    {
        while (true)
        {
            float startTime = Time.time;

            while (Time.time - startTime < flickerDuration)
            {
                targetObject.SetActive(false);
                yield return new WaitForSeconds(flickerInterval);
                targetObject.SetActive(true);
                yield return new WaitForSeconds(flickerInterval);
            }

            targetObject.SetActive(false);
            yield return new WaitForSeconds(totalDuration - flickerDuration);
        }
    }
}
