using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _spawnHealth;

    [SerializeField] private int _health;
    [SerializeField] private bool _isAlive;

    public bool IsAlive => _isAlive;
    public int Health => _health;

    public delegate void CharacterDeath();
    public static event CharacterDeath OnCharacterDeath;

    private void OnEnable()
    {
        HealthPickupObject.OnHealthPickupObjectTaken += AddHealth;

        _isAlive = true;
        _health = _spawnHealth;
    }

    private void Update()
    {
        // Character Death
        if (_health == 0)
        {
            ApplyDamage();
            _isAlive = false;
            OnCharacterDeath?.Invoke();
        }
    }

    private void ApplyDamage()
    {
        _health--;

        // Can be modified to apply some amount of damage
    }

    private void AddHealth()
    {
        if (_health < 3 & GetComponent<PlayerController>() != null)
            _health++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Bullet") && gameObject.GetComponent<EnemyControllerPooled>() != null)
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
