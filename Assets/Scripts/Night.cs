// using UnityEngine;

// [RequireComponent(typeof(Light))]
// public class Night : MonoBehaviour
// {
//     [Header("Night Settings")]
//     [Tooltip("Intensitas cahaya saat malam (0 = gelap total)")]
//     [Range(0f, 1f)]
//     public float nightIntensity = 0f;

//     [Tooltip("Sudut rotasi matahari saat malam (0-360)")]
//     public float nightAngle = 270f;

//     void Start()
//     {
//         DayNightCycles dayCycle = GetComponent<DayNightCycles>();
//         if (dayCycle != null)
//         {
//             dayCycle.enabled = false;
//             Debug.Log("DayNightCycles di-nonaktifkan untuk mode malam permanen");
//         }

//         Light sunLight = GetComponent<Light>();
//         sunLight.intensity = nightIntensity;
//         Debug.Log($"Intensitas cahaya diatur ke {nightIntensity}");

//         transform.rotation = Quaternion.Euler(nightAngle, 0, 0);
//         Debug.Log($"Posisi matahari diatur ke sudut {nightAngle}°");
//     }
// }

using UnityEngine;

public class Night : MonoBehaviour
{
    void Start()
    {
        float radius = Vector3.Distance(transform.position, Vector3.zero);

        transform.position = new Vector3(0f, -radius, 0f);

        transform.LookAt(Vector3.zero);
    }
}