using UnityEngine;

public class AmmoPickupObject : PickupObject
{
    [SerializeField] private GunController _gun;

    public delegate void AmmoPickupObjectTaken();
    public static event AmmoPickupObjectTaken OnAmmoPickupObjectTaken;

    private new void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _gun.NumberOfClips < 6)
        {
            base.OnTriggerEnter(other);
            OnAmmoPickupObjectTaken?.Invoke();
        }
    }
}
