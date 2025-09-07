using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Vector3 initialPosition;
    public AudioClip deathSound;

    // AI Settings
    [Header("AI Settings")]
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float fleeSpeed = 4f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float wallCheckDistance = 0.5f;

    private Transform playerTransform;
    private bool isInitialized = false;
    private bool isChasing = true;
    private Rigidbody rb;
    private Vector3 lastDirection = Vector3.forward;

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        // Cari player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            isInitialized = true;
        }
        else
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
        }
    }

    private void Update()
    {
        if (!isInitialized) return;

        // Periksa apakah ada player dalam jarak deteksi
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Tentukan perilaku berdasarkan status power-up player
            Player playerScript = playerTransform.GetComponent<Player>();

            if (playerScript != null && playerScript.canEatEnemies)
            {
                // Player dalam keadaan power-up, kabur!
                FleeFromPlayer();
                isChasing = false;
            }
            else
            {
                // Player tidak dalam keadaan power-up, kejar!
                ChasePlayer();
                isChasing = true;
            }
        }
        else
        {
            // Jika tidak ada player dalam jarak, kembali ke posisi awal
            ReturnToInitialPosition();
        }
    }

    private void ChasePlayer()
    {
        // Hitung arah ke player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Cek tabrakan dengan tembok
        Vector3 safeDirection = CheckForWalls(directionToPlayer);

        // Gerakkan musuh
        rb.velocity = safeDirection * chaseSpeed;
        lastDirection = safeDirection;

        // Rotasi musuh menghadap arah gerakan
        if (safeDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(safeDirection);
        }
    }

    private void FleeFromPlayer()
    {
        // Hitung arah menjauh dari player
        Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;

        // Cek tabrakan dengan tembok
        Vector3 safeDirection = CheckForWalls(directionAwayFromPlayer);

        // Gerakkan musuh
        rb.velocity = safeDirection * fleeSpeed;
        lastDirection = safeDirection;

        // Rotasi musuh menghadap arah gerakan
        if (safeDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(safeDirection);
        }
    }

    private void ReturnToInitialPosition()
    {
        // Hitung arah ke posisi awal
        Vector3 directionToInitial = (initialPosition - transform.position).normalized;

        // Cek tabrakan dengan tembok
        Vector3 safeDirection = CheckForWalls(directionToInitial);

        // Gerakkan musuh
        rb.velocity = safeDirection * chaseSpeed * 0.5f; // Lebih lambat saat kembali

        // Rotasi musuh menghadap arah gerakan
        if (safeDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(safeDirection);
        }
    }

    private Vector3 CheckForWalls(Vector3 direction)
    {
        // Simpan arah asli
        Vector3 originalDirection = direction;

        // Cek tabrakan dengan tembok di depan
        if (Physics.Raycast(transform.position, direction, wallCheckDistance))
        {
            // Coba belok ke kiri
            Vector3 leftDirection = Quaternion.Euler(0, -90, 0) * direction;
            if (!Physics.Raycast(transform.position, leftDirection, wallCheckDistance))
            {
                return leftDirection;
            }

            // Coba belok ke kanan
            Vector3 rightDirection = Quaternion.Euler(0, 90, 0) * direction;
            if (!Physics.Raycast(transform.position, rightDirection, wallCheckDistance))
            {
                return rightDirection;
            }

            // Coba belok ke belakang
            Vector3 backwardDirection = -direction;
            if (!Physics.Raycast(transform.position, backwardDirection, wallCheckDistance))
            {
                return backwardDirection;
            }

            // Jika semua arah terhalang, tetap gunakan arah asli (akan tetap tabrakan)
            return originalDirection;
        }

        return direction;
    }

    // Metode yang dipanggil ketika dimakan oleh player selama power-up
    public void EatenByPlayer()
    {
        // Hentikan gerakan musuh
        rb.velocity = Vector3.zero;

        // Mainkan efek suara kematian
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // Respawn musuh ke posisi awal
        transform.position = initialPosition;
        Debug.Log("Enemy respawned to initial position");
    }

    // Visualisasi untuk debugging
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Tampilkan radius deteksi
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            // Tampilkan cek tembok
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * wallCheckDistance);
        }
    }
}