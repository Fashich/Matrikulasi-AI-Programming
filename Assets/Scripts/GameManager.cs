using PickableNamespace;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public Canvas gameOverCanvas;
    public Canvas winCanvas;
    public Button restartButton;
    public Button mainMenuButton;
    public Button winRestartButton;

    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;

    [Header("Enemy Settings")]
    public GameObject[] enemies;

    [Header("Coin Settings")]
    public GameObject[] coins;

    private bool isGameOver = false;
    private Vector3 playerInitialPosition;

    private void Awake()
    {
        // Pastikan hanya ada satu instance GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Setup awal
        SetupInitialGameState();

        // Setup UI
        SetupUI();
    }

    private void SetupInitialGameState()
    {
        // Setup awal
        if (gameOverCanvas != null)
        {
            gameOverCanvas.enabled = false;
        }

        // Setup Win Canvas
        if (winCanvas != null)
        {
            winCanvas.enabled = false;
        }

        Time.timeScale = 1f;

        // Simpan posisi awal player
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            // Pastikan initialPosition sudah diset di Player
            playerInitialPosition = player.initialPosition;
        }
        else
        {
            Debug.LogError("Player not found during initialization. Make sure player exists in the scene.");
        }

        // Simpan referensi semua enemy
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogWarning("No enemies found with 'Enemy' tag. Make sure enemies are properly tagged.");
        }

        // Perbaiki: Gunakan FindObjectsByType untuk mencari objek dengan komponen Pickable
        var pickableObjects = FindObjectsByType<PickableNamespace.Pickable>(FindObjectsSortMode.None);
        coins = new GameObject[pickableObjects.Length];
        for (int i = 0; i < pickableObjects.Length; i++)
        {
            coins[i] = pickableObjects[i].gameObject;
        }

        if (coins == null || coins.Length == 0)
        {
            Debug.LogWarning("No pickable items found with Pickable component. Make sure coins/power-ups have the Pickable script.");
        }
    }

    private void SetupUI()
    {
        // Setup tombol restart untuk Game Over
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogError("Restart button not assigned in GameManager. Restart functionality will not work.");
        }

        // Setup tombol main menu
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        else
        {
            Debug.LogError("Main menu button not assigned in GameManager. Main menu functionality will not work.");
        }

        // Setup tombol restart untuk Win Screen
        if (winRestartButton != null)
        {
            winRestartButton.onClick.RemoveAllListeners();
            winRestartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogWarning("Win restart button not assigned. Win screen restart functionality may not work.");
        }
    }

    public void ShowGameOverMenu()
    {
        isGameOver = true;

        // Pastikan canvas ada sebelum mengaktifkannya
        if (gameOverCanvas != null)
        {
            gameOverCanvas.enabled = true;
        }
        else
        {
            Debug.LogError("GameOverCanvas is not assigned. Cannot show game over menu.");
            return;
        }

        Time.timeScale = 0f;

        // Pastikan kursor terlihat
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Game Over! Player has been defeated.");
    }

    // HANYA ADA SATU DEFINISI ShowWinScreen()
    public void ShowWinScreen()
    {
        isGameOver = false;
        if (winCanvas != null)
        {
            winCanvas.enabled = true;
            Debug.Log("Win screen displayed successfully.");
        }
        else
        {
            Debug.LogError("WinCanvas is not assigned in GameManager. Please assign it in the Inspector.");
            return;
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Player wins! All coins collected!");
    }

    public void RestartGame()
    {
        Debug.Log("Restart button pressed");

        // Reset semua objek
        ResetPlayer();
        ResetEnemies();
        ResetCoins();

        // Kembalikan ke keadaan awal
        isGameOver = false;

        // Nonaktifkan canvas game over dan win screen
        if (gameOverCanvas != null)
        {
            gameOverCanvas.enabled = false;
        }

        if (winCanvas != null)
        {
            winCanvas.enabled = false;
        }

        Time.timeScale = 1f;

        // Kunci kursor kembali
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Muat ulang scene
        Debug.Log("Loading scene: " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Game restarted.");
    }

    private void ResetPlayer()
    {
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            Debug.Log("Resetting player position to: " + playerInitialPosition);

            // Reset posisi
            player.transform.position = playerInitialPosition;

            // Reset status
            player.canEatEnemies = false;
            player.powerUpTimer = 0f;

            // Pastikan rigidbody aktif
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Pastikan player terlihat
            Renderer renderer = player.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }
        else
        {
            Debug.LogError("Player not found during reset. Cannot reset player.");
        }
    }

    private void ResetEnemies()
    {
        Debug.Log("Resetting enemies");

        if (enemies == null || enemies.Length == 0)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }

        if (enemies != null && enemies.Length > 0)
        {
            foreach (GameObject enemyObj in enemies)
            {
                if (enemyObj == null) continue;

                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ResetPosition();
                    Debug.Log("Enemy reset: " + enemyObj.name);
                }
            }
        }
    }

    private void ResetCoins()
    {
        Debug.Log("Resetting coins");

        if (coins == null || coins.Length == 0)
        {
            var pickableObjects = FindObjectsByType<PickableNamespace.Pickable>(FindObjectsSortMode.None);
            coins = new GameObject[pickableObjects.Length];
            for (int i = 0; i < pickableObjects.Length; i++)
            {
                coins[i] = pickableObjects[i].gameObject;
            }
        }

        if (coins != null && coins.Length > 0)
        {
            foreach (GameObject coinObj in coins)
            {
                if (coinObj == null) continue;

                PickableNamespace.Pickable pickable = coinObj.GetComponent<PickableNamespace.Pickable>();
                if (pickable != null && !coinObj.activeSelf)
                {
                    coinObj.SetActive(true);
                    Debug.Log("Coin reactivated: " + coinObj.name);
                }
                else if (pickable == null)
                {
                    Debug.LogWarning($"GameObject {coinObj.name} has no Pickable component.");
                }
            }
        }
    }

    public void GoToMainMenu()
    {
        Debug.Log("Going to main menu");

        // Reset semua referensi sebelum pindah scene
        Instance = null;

        // Implementasi untuk pindah ke main menu
        if (SceneManager.GetSceneByName("MainMenu").IsValid())
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogWarning("MainMenu scene not found. Loading first scene instead.");
            SceneManager.LoadScene(0);
        }

        Time.timeScale = 1f;
    }

    // Update is called once per frame
    private void Update()
    {
        // Cek jika ingin restart dengan tombol R
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key pressed for restart");
            RestartGame();
        }

        // Cek jika ingin kembali ke main menu dengan tombol M
        if (isGameOver && Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M key pressed for main menu");
            GoToMainMenu();
        }
    }
}