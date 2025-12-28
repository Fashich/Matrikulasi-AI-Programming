// // using UnityEngine;
// // using UnityEngine.SceneManagement;
// // using UnityEngine.UI;
// // using System.Collections;

// // public class Play : MonoBehaviour
// // {
// //     [Header("Loading UI References")]
// //     [Tooltip("Parent GameObject containing your loading screen UI")]
// //     public GameObject loadingScreen;

// //     [Tooltip("Text component to display loading status")]
// //     public Text loadingStatusText;

// //     [Tooltip("Optional: Image component for loading bar")]
// //     public Image loadingProgressBar;

// //     [Header("Loading Configuration")]
// //     [Tooltip("Time per loading step in seconds")]
// //     public float stepDuration = 0.8f;

// //     [Tooltip("Scene name to load")]
// //     public string targetSceneName = "GameScreen";

// //     private void OnMouseDown()
// //     {
// //         // Only respond to clicks when not already loading
// //         if (!IsLoading())
// //         {
// //             StartCoroutine(StartGameSequence());
// //         }
// //     }

// //     private IEnumerator StartGameSequence()
// //     {
// //         // 1. Show loading screen
// //         if (loadingScreen != null)
// //             loadingScreen.SetActive(true);

// //         // 2. Show loading steps sequentially
// //         string[] loadingSteps = {
// //             "load initiate",
// //             "envi",
// //             "enemy",
// //             "player",
// //             "all systems online"
// //         };

// //         for (int i = 0; i < loadingSteps.Length; i++)
// //         {
// //             if (loadingStatusText != null)
// //                 loadingStatusText.text = loadingSteps[i];

// //             if (loadingProgressBar != null)
// //                 loadingProgressBar.fillAmount = (i + 1) / (float)loadingSteps.Length;

// //             yield return new WaitForSeconds(stepDuration);
// //         }

// //         // 3. Load the actual scene
// //         AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);

// //         // 4. Monitor loading progress (optional)
// //         while (!asyncLoad.isDone)
// //         {
// //             float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

// //             if (loadingProgressBar != null)
// //                 loadingProgressBar.fillAmount = progress;

// //             if (loadingStatusText != null)
// //                 loadingStatusText.text = $"loading... {Mathf.Round(progress * 100)}%";

// //             yield return null;
// //         }
// //     }

// //     private bool IsLoading()
// //     {
// //         // Check if loading screen is active
// //         return loadingScreen != null && loadingScreen.activeSelf;
// //     }
// // }

// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class Play : MonoBehaviour
// {
//     private void OnMouseDown()
//     {
//         // 1. Pastikan cursor tetap visible
//         Cursor.visible = true;
//         Cursor.lockState = CursorLockMode.None;

//         // 2. HANYA LOAD SCENE BARU (Tanpa unload manual!)
//         SceneManager.LoadScene("GameScreen"); // Mode default = Single (mengganti scene)
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script untuk merespons klik pada objek (Cube/Play Dread Corner) dan memuat scene GameScreen.
/// Cara Pakai:
/// 1. Attach script ini ke GameObject target (misal: Cube atau "Play Dread Corner").
/// 2. Setup Event Trigger dengan memilih fungsi LoadGameScreen().
/// 3. Pastikan scene "GameScreen" ada di Build Settings.
/// </summary>
public class Play : MonoBehaviour
{
    /// <summary>
    /// Fungsi yang dipanggil oleh Event Trigger saat objek diklik.
    /// </summary>
    public void LoadGameScreen()
    {
        // 1. Pastikan cursor tetap terlihat selama transisi
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 2. Debug log untuk verifikasi
        Debug.Log("<color=green>✅ [Play.cs] Memuat scene: GameScreen</color>");

        // 3. Load scene dengan mode default (mengganti scene lama)
        SceneManager.LoadScene("GameScreen");
    }
}