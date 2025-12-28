using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI winText;

    private List<PickableNamespace.Pickable> _pickableList = new List<PickableNamespace.Pickable>();
    private int totalCoins;

    private void Start()
    {
        InitPickableList();
        UpdateCoinText();
    }

    private void InitPickableList()
    {
        // Ganti dengan FindObjectsByType untuk mendapatkan semua objek Pickable
        PickableNamespace.Pickable[] pickables = FindObjectsByType<PickableNamespace.Pickable>(FindObjectsSortMode.None);

        if (pickables != null && pickables.Length > 0)
        {
            foreach (PickableNamespace.Pickable p in pickables)
            {
                _pickableList.Add(p);
                p.OnCollected += HandleCollect; // Tambahkan event untuk setiap objek
            }
            totalCoins = _pickableList.Count;
            UpdateCoinText();
        }
        else
        {
            Debug.LogWarning("Tidak ada objek Pickable ditemukan");
        }
    }

    private void HandleCollect(PickableNamespace.Pickable pickable)
    {
        if (_pickableList.Contains(pickable))
        {
            _pickableList.Remove(pickable);
            UpdateCoinText();
        }

        if (_pickableList.Count == 0)
        {
            OnAllCoinsCollected();
        }
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = $"<sprite name=\"pallets\">: {_pickableList.Count}/{totalCoins}";
        }
    }

    private void OnAllCoinsCollected()
    {
        Debug.Log("Semua koin terkumpul! Menampilkan winscreen...");
        GameManager.Instance.ShowWinScreen();
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