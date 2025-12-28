using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class Player : MonoBehaviour
// {
//     [SerializeField]
//     private float _speed;
//     private Rigidbody _rigidbody;
//     private void Awake() {
//         _rigidbody = GetComponent<Rigidbody>();
//     }

//     void FixedUpdate()
//     {
//         float horizontal = Input.GetAxis("Horizontal");
//         float vertical = Input.GetAxis("Vertical");
//         Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
//         //_rigidbody.MovePosition(transform.position + movementDirection * _speed * Time.deltaTime);
//         _rigidbody.linearVelocity = movementDirection * _speed * Time.fixedDeltaTime;
//         Debug.Log("Horizontal: " + horizontal);
//         Debug.Log("Vertical: " + vertical);
//         if (!Physics.Raycast(transform.position, movementDirection, 0.5f))
//         {
//             _rigidbody.MovePosition(
//                 transform.position +
//                 movementDirection * _speed * Time.fixedDeltaTime
//             );
//         }
//     }
// }
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    // [SerializeField] private float _rotationSmoothness = 10f;

    [Header("Camera Settings")]
    [SerializeField] private float _mouseSensitivity = 10f;
    [SerializeField] private float _minVerticalAngle = -90f;
    [SerializeField] private float _maxVerticalAngle = 90f;

    // Tambahkan variabel untuk Power-Up
    public bool canEatEnemies = false;
    public float powerUpTimer = 0f; // Ubah menjadi public
    private float powerUpDuration = 10f;
    public AudioClip powerUpSound;
    public AudioClip eatEnemySound;

    public Vector3 initialPosition;

    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private float _verticalRotation = 0f;
    private bool isDying = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        initialPosition = transform.position;

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            _cameraTransform = mainCamera.transform;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Debug.LogError("MainCamera not found! Please tag your camera with 'MainCamera'");
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection =
            transform.right * horizontal +
            transform.forward * vertical;

        movementDirection.y = 0;
        movementDirection = movementDirection.normalized;

        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            // transform.rotation = Quaternion.Slerp(
            //     transform.rotation,
            //     targetRotation,
            //     Time.fixedDeltaTime * _rotationSmoothness
            // );

            // Cek tabrakan dengan dinding sebelum bergerak
            if (!Physics.Raycast(transform.position, movementDirection, 0.5f))
            {
                _rigidbody.MovePosition(
                    transform.position +
                    movementDirection * _speed * Time.fixedDeltaTime
                );
            }
        }
    }

    private void HandleCameraRotation()
    {
        if (_cameraTransform == null) return;

        // Dapatkan input mouse
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        // Hitung rotasi vertikal kamera
        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalAngle, _maxVerticalAngle);

        // Terapkan rotasi
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // Debug untuk visualisasi raycast
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Tampilkan raycast untuk deteksi dinding
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 0.5f);
        }
    }

    // Metode baru untuk mengaktifkan Power-Up
    public void ActivatePowerUp(float duration)
    {
        canEatEnemies = true;
        powerUpDuration = duration;
        powerUpTimer = 0f;

        // Mainkan suara power-up
        if (powerUpSound != null)
        {
            AudioSource.PlayClipAtPoint(powerUpSound, transform.position);
        }

        Debug.Log("Power-Up activated! You can eat enemies for " + duration + " seconds!");
    }

    private void Update()
    {
        // Update timer Power-Up
        if (canEatEnemies)
        {
            powerUpTimer += Time.deltaTime;
            if (powerUpTimer >= powerUpDuration)
            {
                canEatEnemies = false;
                powerUpTimer = 0f;
                Debug.Log("Power-Up expired");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (canEatEnemies)
                {
                    // Mainkan suara makan musuh
                    if (eatEnemySound != null)
                    {
                        AudioSource.PlayClipAtPoint(eatEnemySound, transform.position);
                    }

                    // Makan musuh dan respawn ke posisi awal
                    enemy.EatenByPlayer();
                    Debug.Log("Enemy eaten and respawned!");
                }
                else
                {
                    // Player mati jika tidak dalam keadaan Power-Up
                    Debug.Log("Player died!");
                    Cursor.visible = true;

                    // Panggil death sequence
                    StartCoroutine(HandleDeathSequence());
                }
            }
        }
    }

    private IEnumerator HandleDeathSequence()
    {
        if (isDying) yield break;

        isDying = true;

        // Efek kematian (misalnya: player berkedip atau berubah warna)
        Renderer playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            for (int i = 0; i < 5; i++)
            {
                playerRenderer.enabled = false;
                yield return new WaitForSeconds(0.1f);
                playerRenderer.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }

        // Setelah efek kematian selesai, baru tampilkan GameOverCanvas
        GameManager.Instance.ShowGameOverMenu();

        isDying = false;
    }
}