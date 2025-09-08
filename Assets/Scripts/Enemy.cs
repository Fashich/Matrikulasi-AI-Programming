using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector3 initialPosition;
    public AudioClip deathSound;

    [Header("AI Settings")]
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float fleeSpeed = 4f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float wallCheckDistance = 0.5f;

    private Transform playerTransform;
    private bool isInitialized = false;
    private Rigidbody rb;

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

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

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            Player playerScript = playerTransform.GetComponent<Player>();

            if (playerScript != null && playerScript.canEatEnemies)
            {
                FleeFromPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            ReturnToInitialPosition();
        }
    }

    private void ChasePlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Vector3 safeDirection = CheckForWalls(directionToPlayer);

        if (rb != null)
        {
            rb.linearVelocity = safeDirection * chaseSpeed;
        }

        if (safeDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(safeDirection);
        }
    }

    private void FleeFromPlayer()
    {
        Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;
        Vector3 safeDirection = CheckForWalls(directionAwayFromPlayer);

        if (rb != null)
        {
            rb.linearVelocity = safeDirection * fleeSpeed;
        }

        if (safeDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(safeDirection);
        }
    }

    private void ReturnToInitialPosition()
    {
        Vector3 directionToInitial = (initialPosition - transform.position).normalized;
        Vector3 safeDirection = CheckForWalls(directionToInitial);

        if (rb != null)
        {
            rb.linearVelocity = safeDirection * (chaseSpeed * 0.5f);
        }

        if (safeDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(safeDirection);
        }
    }

    private Vector3 CheckForWalls(Vector3 direction)
    {
        if (rb == null) return Vector3.zero;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, wallCheckDistance))
        {
            Vector3 leftDirection = Quaternion.Euler(0, -90, 0) * direction;
            if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, leftDirection, wallCheckDistance))
                return leftDirection;

            Vector3 rightDirection = Quaternion.Euler(0, 90, 0) * direction;
            if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, rightDirection, wallCheckDistance))
                return rightDirection;

            Vector3 backwardDirection = -direction;
            if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, backwardDirection, wallCheckDistance))
                return backwardDirection;
        }

        return direction;
    }

    public void EatenByPlayer()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        transform.position = initialPosition;
        Debug.Log("Enemy respawned to initial position");
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * wallCheckDistance);
        }
    }
}