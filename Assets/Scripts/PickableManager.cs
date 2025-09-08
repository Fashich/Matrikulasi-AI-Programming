using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private List<PickableNamespace.Pickable> _pickableList = new List<PickableNamespace.Pickable>();
    private int totalCoins;

    void Start()
    {
        InitPickableList();
        UpdateCoinText();
    }

    private void InitPickableList()
    {
        // Cari semua objek dengan skrip Pickable
        PickableNamespace.Pickable[] pickables = GameObject.FindObjectsByType<PickableNamespace.Pickable>(FindObjectsSortMode.None);
        foreach (PickableNamespace.Pickable p in pickables)
        {
            _pickableList.Add(p);
            p.OnCollected += HandleCollect;
        }
        totalCoins = _pickableList.Count;
    }

    private void HandleCollect(PickableNamespace.Pickable pickable)
    {
        _pickableList.Remove(pickable);
        UpdateCoinText();

        if (_pickableList.Count == 0)
        {
            Debug.Log("Selamat! Semua koin terkumpul!");
        }
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins Tersisa: {_pickableList.Count}/{totalCoins}";
        }
    }
}