using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public delegate void PickupObjectTaken();
    public static event PickupObjectTaken onPickupObjectTaken;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            // Pickable method
            onPickupObjectTaken?.Invoke();
            Debug.Log("Message From Parent Class");
            gameObject.SetActive(false);
        }
        
    }
}
