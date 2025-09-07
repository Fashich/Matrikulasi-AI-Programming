using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableManager : MonoBehaviour
{
    private List<Pickable> _pickableList = new List<Pickable>();

    void Start()
    {
        InitPickableList();
        OnPickablePicked(Pickable pickable);
    }

    private void InitPickableList()
    {
        // Gunakan FindObjectsOfType tanpa parameter (hanya aktifkan objek)
        Pickable[] pickableObjects = GameObject.FindObjectsOfType<Pickable>();

        for (int i = 0; i < pickableObjects.Length; i++)
        {
            _pickableList.Add(pickableObjects[i]);
            pickableObjects[i].OnPicked += OnPickablePicked;
        }

        Debug.Log("Total Pickables: " + _pickableList.Count);
    }
    private void OnPickablePicked(Pickable pickable)
    {
        _pickableList.Remove(pickable);
        if (_pickableList.Count <= 0) {
            Debug.Log("Win");
        }
    }
}