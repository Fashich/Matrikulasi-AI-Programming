using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Rigidbody _rigidbody;
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        //_rigidbody.MovePosition(transform.position + movementDirection * _speed * Time.deltaTime);
        _rigidbody.velocity = movementDirection * _speed * Time.fixedDeltaTime;
        Debug.Log("Horizontal: " + horizontal);
        Debug.Log("Vertical: " + vertical);
    }
}
