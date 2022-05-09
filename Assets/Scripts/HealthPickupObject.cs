using UnityEngine;

public class HealthPickupObject : PickupObject
{
    public delegate void HealthPickupObjectTaken();
    public static event HealthPickupObjectTaken OnHealthPickupObjectTaken;

    [SerializeField] private PlayerController _player;

    private new void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _player.health < 3)
        {
            base.OnTriggerEnter(other);
            OnHealthPickupObjectTaken?.Invoke();
        }
    }
}
