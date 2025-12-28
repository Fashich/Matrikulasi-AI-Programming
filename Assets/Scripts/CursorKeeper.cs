using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorKeeper : MonoBehaviour
{
    void Start()
    {
        // Pastikan cursor tetap visible selama Main Menu aktif
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Force update cursor state setiap frame
        if (!Cursor.visible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}