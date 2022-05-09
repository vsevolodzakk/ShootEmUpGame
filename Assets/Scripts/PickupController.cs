using UnityEngine;

public class PickupController : MonoBehaviour
{
    public delegate void PickupTaken();
    public static event PickupTaken OnPickupTaken;

    [SerializeField] private GunController _gun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _gun.ammo < 100)
        {
            OnPickupTaken?.Invoke();
            Debug.Log("PICKUP TAKEN");
        }
    }
}
