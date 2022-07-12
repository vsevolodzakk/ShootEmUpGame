using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _spawnHealth;

    [SerializeField] private int _health;

    public bool isAlive;
    public int Health => _health;

    public delegate void CharacterDeath();
    public static event CharacterDeath OnCharacterDeath;

    private void OnEnable()
    {
        HealthPickupObject.OnHealthPickupObjectTaken += AddHealth;

        isAlive = true;
        _health = _spawnHealth;
    }

    private void Update()
    {
        // Character Death
        if (_health == 0)
        {
            ApplyDamage();
            isAlive = false;
            OnCharacterDeath?.Invoke();
        }
    }

    private void ApplyDamage()
    {
        _health--;

        // Can be modified to apple some amount of damage
    }

    private void AddHealth()
    {
        if (_health < 3 & GetComponent<PlayerController>() != null)
            _health++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<Bullet>() != null && gameObject.GetComponent<EnemyControllerPooled>() != null)
            || (other.CompareTag("EnemyBullet") && gameObject.GetComponent<PlayerController>() != null))
        {
            ApplyDamage();
        }
    }

    private void OnDestroy()
    {
        HealthPickupObject.OnHealthPickupObjectTaken -= AddHealth;
    }
}
