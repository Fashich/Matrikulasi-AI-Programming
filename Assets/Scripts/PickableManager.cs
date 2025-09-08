using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI winText;

    private List<PickableNamespace.Pickable> _pickableList = new List<PickableNamespace.Pickable>();
    private int totalCoins;

    private void Start()
    {
        Debug.Log("[PickableManager] Starting initialization...");
        InitPickableList();
        UpdateCoinText();
    }

    private void InitPickableList()
    {
        Debug.Log("[PickableManager] Finding all pickable objects...");

        // Cari semua objek dengan skrip Pickable
        PickableNamespace.Pickable[] pickables = FindObjectsByType<PickableNamespace.Pickable>(FindObjectsSortMode.None);

        Debug.Log($"[PickableManager] Found {pickables.Length} pickable objects");

        _pickableList = new List<PickableNamespace.Pickable>(pickables);
        totalCoins = _pickableList.Count;

        foreach (PickableNamespace.Pickable p in _pickableList)
        {
            if (p != null)
            {
                p.OnCollected += HandleCollect;
                Debug.Log($"[PickableManager] Registered OnCollected event for {p.gameObject.name}");
            }
            else
            {
                Debug.LogWarning("[PickableManager] Found null pickable object in the list");
            }
        }

        UpdateCoinText();
    }

    private void HandleCollect(PickableNamespace.Pickable pickable)
    {
        Debug.Log($"[PickableManager] Koin terkumpul: {pickable.gameObject.name}");

        if (_pickableList.Contains(pickable))
        {
            _pickableList.Remove(pickable);
            Debug.Log($"[PickableManager] {_pickableList.Count} koin tersisa");
        }
        else
        {
            Debug.LogWarning("[PickableManager] Pickable tidak ditemukan dalam daftar");
        }

        UpdateCoinText();

        if (_pickableList.Count == 0)
        {
            Debug.Log("[PickableManager] SELAMAT! SEMUA KOIN TERKUMPUL!");
            OnAllCoinsCollected();
        }
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins Tersisa: {_pickableList.Count}/{totalCoins}";
            Debug.Log($"[PickableManager] UI updated - {_pickableList.Count}/{totalCoins} coins");
        }
        else
        {
            Debug.LogWarning("[PickableManager] CoinText tidak diassign di Inspector");
        }
    }

    private void OnAllCoinsCollected()
    {
        Debug.Log("[PickableManager] Memanggil ShowWinScreen");

        // Tambahkan delay kecil untuk memastikan semua koin benar-benar dihancurkan
        Invoke(nameof(ShowWinScreenDelayed), 0.1f);
    }

    private void ShowWinScreenDelayed()
    {
        if (GameManager.Instance != null)
        {
            Debug.Log("[PickableManager] GameManager ditemukan, memanggil ShowWinScreen");
            GameManager.Instance.ShowWinScreen();
        }
        else
        {
            Debug.LogError("[PickableManager] GameManager instance not found. Cannot show win screen.");

            // Coba cari GameManager secara manual
            GameManager manager = FindObjectOfType<GameManager>();
            if (manager != null)
            {
                Debug.Log("[PickableManager] Found GameManager via FindObjectOfType");
                manager.ShowWinScreen();
            }
            else
            {
                Debug.LogError("[PickableManager] GameManager tidak ditemukan sama sekali");
            }
        }

        if (winText != null)
        {
            winText.text = "You Win! All coins collected!";
        }
    }

    // Tambahkan metode ini untuk digunakan oleh GameManager
    public int GetRemainingCoins()
    {
        return _pickableList.Count;
    }
}