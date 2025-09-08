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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetupInitialGameState();
        SetupUI();
    }

    private void Start()
    {
        Debug.Log("GameManager started");
        Debug.Log("gameOverCanvas: " + (gameOverCanvas ? "assigned" : "not assigned"));
        Debug.Log("winCanvas: " + (winCanvas ? "assigned" : "not assigned"));
    }

    private void OnEnable()
    {
        SetupUI();
    }

    private void OnLevelWasLoaded(int level)
    {
        SetupUI();
    }

    private void SetupInitialGameState()
    {
        if (gameOverCanvas != null) gameOverCanvas.enabled = false;
        if (winCanvas != null) winCanvas.enabled = false;
        Time.timeScale = 1f;

        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            playerInitialPosition = player.transform.position;
        }
        else
        {
            Debug.LogError("Player not found during initialization.");
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogWarning("No enemies found with 'Enemy' tag.");
        }

        var pickableObjects = FindObjectsByType<PickableNamespace.Pickable>(FindObjectsSortMode.None);
        coins = new GameObject[pickableObjects.Length];
        for (int i = 0; i < pickableObjects.Length; i++)
        {
            coins[i] = pickableObjects[i].gameObject;
        }

        if (coins == null || coins.Length == 0)
        {
            Debug.LogWarning("No pickable items found.");
        }
    }

    private void SetupUI()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else Debug.LogError("Restart button not assigned.");

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        else Debug.LogError("Main menu button not assigned.");

        if (winRestartButton != null)
        {
            winRestartButton.onClick.AddListener(RestartGame);
        }
        else Debug.LogWarning("Win restart button not assigned.");
    }

    public void ShowWinScreen()
    {
        isGameOver = false;
        if (winCanvas != null)
        {
            winCanvas.enabled = true;
            Debug.Log("Win screen displayed.");
        }
        else
        {
            Debug.LogError("WinCanvas is not assigned in GameManager.");
            return;
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowGameOverMenu()
    {
        isGameOver = true;
        if (gameOverCanvas != null)
        {
            gameOverCanvas.enabled = true;
        }
        else
        {
            Debug.LogError("GameOverCanvas is not assigned in GameManager.");
            return;
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowWinScreen()
    {
        isGameOver = false;
        isWinScreenActive = true; // Tandai winscreen aktif

        if (winCanvas != null)
        {
            winCanvas.enabled = true;
            Debug.Log("Win screen displayed successfully.");
        }
        else
        {
            Debug.LogError("WinCanvas is not assigned in GameManager.");
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
        isWinScreenActive = false;

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

    // Tambahkan metode untuk memeriksa apakah semua koin telah terkumpul
    public bool AreAllCoinsCollected()
    {
        if (pickableManager != null)
        {
            return pickableManager.GetRemainingCoins() == 0;
        }

        // Jika tidak ada PickableManager, cek langsung ke array coins
        if (coins == null || coins.Length == 0)
        {
            return true; // Tidak ada koin, dianggap semua terkumpul
        }

        foreach (GameObject coin in coins)
        {
            if (coin != null && coin.activeSelf)
            {
                return false; // Masih ada koin yang aktif
            }
        }

        return true;
    }
}