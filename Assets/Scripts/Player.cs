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

    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private float _verticalRotation = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // Setup camera
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
    // Dapatkan input horizontal dan vertikal dalam koordinat lokal karakter
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    // Hitung arah gerakan berdasarkan sumbu lokal karakter
    Vector3 movementDirection =
        transform.right * horizontal +
        transform.forward * vertical;

    movementDirection.y = 0; // Hapus komponen vertikal
    movementDirection = movementDirection.normalized;

    if (movementDirection != Vector3.zero)
    {
        // Rotasi karakter sesuai arah gerakan
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
}