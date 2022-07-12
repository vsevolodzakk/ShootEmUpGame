using UnityEngine;

public class PickupObject: MonoBehaviour
{
    public delegate void PickupObjectTaken(Transform takenObject);
    public static event PickupObjectTaken OnPickupObjectTaken;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            // Pickable method
            OnPickupObjectTaken?.Invoke(gameObject.transform);
        }
    }
}
