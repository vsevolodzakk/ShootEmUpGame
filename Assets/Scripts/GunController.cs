using UnityEngine;
using UnityEngine.InputSystem;

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

    private PlayerInputActions _actions;

    public int Ammo => _ammo;

    public delegate void GunFire();
    public static event GunFire OnGunFire;

    private void Awake()
    {
        _actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken += AddAmmo;

        _ammo = _ammoFromPickup;

        _actions.Player.Fire.Enable();

        _actions.Player.Fire.performed += Bang;
    }

    private void Bang(InputAction.CallbackContext context)
    {
        // Player gun fire conditions
        if (_playerHealth.IsAlive && !_sceneController.gameOnPause)
        {
            if (_ammo > 0)
            {
                Fire();
                _shotFiredSound.Play();
                _muzzleFlash.Play();

            }
            else
            {
                _noAmmoSound.Play();
            }
        }
    }

    void Update()
    {
        //if (Input.GetButtonDown("Fire1")
        //        && _playerHealth.IsAlive && _ammo > 0
        //            && _sceneController.gameOnPause == false)
        //{
        //    Fire();
        //    _muzzleFlash.Play();
        //}
        //else if (Input.GetButtonDown("Fire1")
        //        && _playerHealth.IsAlive && _ammo == 0)
        //    _noAmmoSound.Play();
    }   

    /// <summary>
    /// Fire mechanic
    /// </summary>
    private void Fire()
    {
        var shot = _bulletPool.Get();
        shot.transform.rotation = transform.rotation;
        shot.transform.position = transform.position;
        shot.gameObject.SetActive(true);

        _ammo--;

        OnGunFire?.Invoke();
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

        _actions.Player.Fire.Disable();
        _actions.Player.Fire.performed -= Bang;
    }
}
