using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public AudioClip powerUpSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Mainkan efek suara
                if (powerUpSound != null)
                {
                    AudioSource.PlayClipAtPoint(powerUpSound, transform.position);
                }

                // Aktifkan Power-Up
                player.ActivatePowerUp(10f);
            }

            // Hapus Power-Up
            Destroy(gameObject);
        }
    }
}