using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Camera _camera;
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        HideAndLockCursor();
    }

    private void HideAndLockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal = a or left (-) & d or right (+)
        float horizontal = Input.GetAxis("Horizontal");
        // Vertical = s or down (-) & w or up (+)
        float vertical = Input.GetAxis("Vertical");

        Vector3 horizontalDirection = horizontal * _camera.transform.right;
        Vector3 verticalDirection = vertical * _camera.transform.forward;
        horizontalDirection.y = 0;
        verticalDirection.y = 0;

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        _rigidbody.linearVelocity = movementDirection * _speed; Debug.Log("Vertical: " + vertical);
    }
}