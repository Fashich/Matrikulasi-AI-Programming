// /* This class manages a list of pickable objects in a game, updating a UI text to display the number of
// remaining coins and handles collection events. */
using System.Collections.Generic;
using UnityEngine;

// public class Pickable : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI coinText;
//     [SerializeField] private int totalCoins = 0;
//
//     private List<Pickable> _pickableList = new List<Pickable>();
//
//     void Start()
//     {
//         InitPickableList();
//         UpdateCoinText();
//     }
//
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
//
//     private void HandleCollect(Pickable pickable)
//     {
//         _pickableList.Remove(pickable);
//         UpdateCoinText();
//
//         if (_pickableList.Count == 0)
//         {
//             Debug.Log("Selamat! Semua koin terkumpul!");
//         }
//     }
//
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
                Debug.Log($"[Pickable] Koin terkumpul! ({Type}) - {gameObject.name}");

                // Pastikan event dipicu
                if (OnCollected != null)
                {
                    OnCollected.Invoke(this);
                    Debug.Log($"[Pickable] OnCollected event invoked for {gameObject.name}");
                }
                else
                {
                    Debug.LogWarning($"[Pickable] OnCollected event is null for {gameObject.name}");
                }

                // Mainkan suara dan hapus objek
                if (CollectSound != null)
                {
                    AudioSource.PlayClipAtPoint(CollectSound, transform.position);
                }

                // Hapus koin setelah delay kecil untuk memastikan event terpanggil
                Invoke(nameof(DestroyPickable), 0.05f);
            }
        }

        private void DestroyPickable()
        {
            Destroy(gameObject);
        }
    }
}