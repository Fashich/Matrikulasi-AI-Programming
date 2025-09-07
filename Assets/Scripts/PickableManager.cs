using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PickableNamespace;

public class PickableManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private List<Pickable> _pickableList = new List<Pickable>();
    private int totalCoins;

    void Start()
    {
        InitPickableList();
        UpdateCoinText();
    }

    private void InitPickableList()
    {
        Pickable[] pickables = GameObject.FindObjectsByType<Pickable>(FindObjectsSortMode.None);
        foreach (Pickable p in pickables)
        {
            _pickableList.Add(p);
            p.OnCollected += HandleCollect;
        }
        totalCoins = _pickableList.Count;
    }

    private void HandleCollect(Pickable pickable)
    {
        _pickableList.Remove(pickable);
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins Tersisa: {_pickableList.Count}/{totalCoins}";
        }
    }
}