using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private HealthComponent _playerHealth;
    [SerializeField] private ParticleSystem _muzzleFlash;

    [SerializeField] private SceneController _sceneController;
    
    public int ammo;

    [SerializeField] private AmmoPickupObject _ammoBox;
    [SerializeField] private int _ammoFromPickup;

    [SerializeField] private AudioSource _shotFiredSound;
    [SerializeField] private AudioSource _noAmmoSound;
    [SerializeField] private AudioSource _reloadSound;

    public delegate void GunFire();
    public static event GunFire OnGunFire;

    private void OnEnable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken += AddAmmo;
    }

    private void Start()
    {
        _ammoFromPickup = _ammoBox.ammoInBox;
    }

    void Update()
    {
        // Player gun fire conditions
        if (Input.GetButtonDown("Fire1")
                && _playerHealth.isAlive && ammo > 0
                    && _sceneController.gameOnPause == false)
        {
            Fire();
            _muzzleFlash.Play();
        }
        else if (Input.GetButtonDown("Fire1")
                && _playerHealth.isAlive && ammo == 0)
            _noAmmoSound.Play();
    }   

    /// <summary>
    /// Fire mechanic
    /// </summary>
    private void Fire()
    {
        var shot = _bulletPool.Get();
        ammo--;
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
        ammo += _ammoFromPickup;
        if (ammo >= 100)
            ammo = 100;
        _reloadSound.Play();
    }

    private void OnDisable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken -= AddAmmo;
    }
}
