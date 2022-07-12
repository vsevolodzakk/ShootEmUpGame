using UnityEngine;

public class AmmoPickupObject : PickupObject
{
    public int ammoInBox = 50;

    [SerializeField] private GunController _gun;

    public delegate void AmmoPickupObjectTaken();
    public static event AmmoPickupObjectTaken OnAmmoPickupObjectTaken;

    private new void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _gun.ammo < 100)
        {
            base.OnTriggerEnter(other);
            OnAmmoPickupObjectTaken?.Invoke();
        }
    }
}
