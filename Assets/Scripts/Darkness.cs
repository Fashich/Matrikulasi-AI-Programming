using UnityEngine;

[ExecuteInEditMode] // Agar bisa diuji di Editor tanpa perlu play mode
public class Darkness : MonoBehaviour
{
    [Header("Darkness Settings")]
    [Range(0f, 0.1f)]
    public float ambientIntensity = 0.02f; // Semakin kecil, semakin gelap
    public bool disableDirectionalLights = true; // Nonaktifkan matahari/directional light
    public bool enableFog = true; // Aktifkan fog untuk efek horror
    public Color fogColor = new Color(0.05f, 0.05f, 0.1f, 1f); // Biru-gelap
    public float fogDensity = 0.03f;

    void Start()
    {
        ApplyDarkness();
    }

    void Update()
    {
        // Untuk debugging di Editor (bisa dihapus jika tidak diperlukan)
        #if UNITY_EDITOR
        ApplyDarkness();
        #endif
    }

    void ApplyDarkness()
    {
        // 1. MATIKAN AMBIENT LIGHT (cahaya global)
        RenderSettings.ambientLight = Color.black;
        RenderSettings.ambientIntensity = ambientIntensity;

        // 2. NONAKTIFKAN DIRECTIONAL LIGHT (matahari)
        if (disableDirectionalLights)
        {
            Light[] allLights = FindObjectsOfType<Light>();
            foreach (Light light in allLights)
            {
                if (light.type == LightType.Directional)
                {
                    light.intensity = 0f; // Matikan matahari
                }
            }
        }

        // 3. AKTIFKAN FOG (opsional untuk efek horror)
        if (enableFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogMode = FogMode.Exponential;
        }
        else
        {
            RenderSettings.fog = false;
        }
    }
}