using UnityEngine;

public class PickupController : MonoBehaviour
{
    public delegate void PickupTaken();
    public static event  PickupTaken onPickupTaken;

    [SerializeField] private GunController _gun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _gun.ammo < 99)
        {
            onPickupTaken?.Invoke();
            Debug.Log("PICKUP TAKEN");
        }
    }
}
