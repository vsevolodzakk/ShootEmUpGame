using UnityEngine;

public class HealthPickupObject : PickupObject
{
    public delegate void HealthPickupObjectTaken();
    public static event HealthPickupObjectTaken OnHealthPickupObjectTaken;

    [SerializeField] private PlayerController _player;

    private new void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            base.OnTriggerEnter(other);
            OnHealthPickupObjectTaken?.Invoke();
        }
    }
}
