using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    public PickableType PickableType;
    public Action<Pickable> OnPicked;
    private void OnTriggerEnter(Collider other) {
        // if (other.CompareTag("Player")) {
        //     Debug.Log("Picked up " + gameObject.name);
        //     Destroy(gameObject);
        // }
        if(other.CompareTag("Player")){
            Debug.Log("Pick Up:" + PickableType);
            OnPicked(this);
            Destroy(gameObject);
        }
    }
    // private void OnTriggerStay(Collider other) {
    //     if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) {
    //         Debug.Log("Picked up " + gameObject.name);
    //         Destroy(gameObject);
    //     }
    //     Debug.Log("Trigger Staying");
    // }
    // private void OnTriggerExit(Collider other) {
    //     if (other.CompareTag("Player")) {
    //         Debug.Log("Player left the trigger of " + gameObject.name);
    //     }
    //     Debug.Log("Trigger Exited");
    // }
}
