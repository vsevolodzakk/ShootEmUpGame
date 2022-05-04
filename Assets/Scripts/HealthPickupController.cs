using UnityEngine;

public class HealthPickupController : MonoBehaviour
{
    public delegate void HealthPickupTaken();
    public static event HealthPickupTaken onHealthPickupTaken;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onHealthPickupTaken?.Invoke();
        }
    }
}
