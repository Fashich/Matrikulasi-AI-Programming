// /* This class manages a list of pickable objects in a game, updating a UI text to display the number of
// remaining coins and handling collection events. */
using System.Collections.Generic;
using UnityEngine;

// public class Pickable : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI coinText;
//     [SerializeField] private int totalCoins = 0;

//     private List<Pickable> _pickableList = new List<Pickable>();

//     void Start()
//     {
//         InitPickableList();
//         UpdateCoinText();
//     }

//     private void InitPickableList()
//     {
//         // Cari semua objek dengan skrip Pickable
//         Pickable[] pickables = GameObject.FindObjectsOfType<Pickable>();
//         foreach (Pickable p in pickables)
//         {
//             _pickableList.Add(p);
//             p.OnCollected += HandleCollect;
//         }
//         totalCoins = _pickableList.Count;
//     }

//     private void HandleCollect(Pickable pickable)
//     {
//         _pickableList.Remove(pickable);
//         UpdateCoinText();

//         if (_pickableList.Count == 0)
//         {
//             Debug.Log("Selamat! Semua koin terkumpul!");
//         }
//     }

//     private void UpdateCoinText()
//     {
//         if (coinText != null)
//         {
//             coinText.text = $"Coins Tersisa: {_pickableList.Count}/{totalCoins}";
//         }
//     }
// }

namespace PickableNamespace {
public enum PickableType { Coin, PowerUp }

public class Pickable : MonoBehaviour
{
    public PickableType Type;
    public AudioClip CollectSound;

    public event System.Action<Pickable> OnCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Koin terkumpul! ({Type})");
            OnCollected?.Invoke(this);
            AudioSource.PlayClipAtPoint(CollectSound, transform.position);
            Destroy(gameObject);
        }
    }
}
}