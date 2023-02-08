using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private HealthComponent _playerHealth;
    [SerializeField] private ParticleSystem _muzzleFlash;

    [SerializeField] private SceneController _sceneController;
    
    [SerializeField] private int _ammo;
    [SerializeField] private int _ammoFromPickup;

    [SerializeField] private AudioSource _shotFiredSound;
    [SerializeField] private AudioSource _noAmmoSound;
    [SerializeField] private AudioSource _reloadSound;

    public int Ammo => _ammo;

    public delegate void GunFire();
    public static event GunFire OnGunFire;

    private void OnEnable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken += AddAmmo;
        _ammo = _ammoFromPickup;
    }

    void Update()
    {
        // Player gun fire conditions
        if (Input.GetButtonDown("Fire1")
                && _playerHealth.IsAlive && _ammo > 0
                    && _sceneController.gameOnPause == false)
        {
            Fire();
            _muzzleFlash.Play();
        }
        else if (Input.GetButtonDown("Fire1")
                && _playerHealth.IsAlive && _ammo == 0)
            _noAmmoSound.Play();
    }   

    /// <summary>
    /// Fire mechanic
    /// </summary>
    private void Fire()
    {
        var shot = _bulletPool.Get();
        _ammo--;
        shot.transform.rotation = transform.rotation;
        shot.transform.position = transform.position;
        shot.gameObject.SetActive(true);
        OnGunFire?.Invoke();
        _shotFiredSound.Play();
    }

    /// <summary>
    /// Add ammo by event
    /// </summary>
    private void AddAmmo()
    {
        _ammo += _ammoFromPickup;
        if (_ammo >= 100)
            _ammo = 100;
        _reloadSound.Play();
    }

    private void OnDisable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken -= AddAmmo;
    }
}
