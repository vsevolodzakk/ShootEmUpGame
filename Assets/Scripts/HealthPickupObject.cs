using UnityEngine;

public class HealthPickupObject : PickupObject
{
    public delegate void HealthPickupObjectTaken();
    public static event HealthPickupObjectTaken onHealthPickupObjectTaken;

    [SerializeField] private PlayerController _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _player.health < 3)
        {
            onHealthPickupObjectTaken?.Invoke();
        }
    }
}
