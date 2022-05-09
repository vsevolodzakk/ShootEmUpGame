using UnityEngine;

public class PickupObject: MonoBehaviour
{
    public delegate void PickupObjectTaken(Transform takenObject);
    public static event PickupObjectTaken OnPickupObjectTaken;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Pickable method
            OnPickupObjectTaken?.Invoke(gameObject.transform);
        }
    }
}
