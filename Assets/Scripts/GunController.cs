using System.Collections;
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

    #region Тест режима обойм

    [SerializeField] private int _stockClipAmmo;
    [SerializeField] private int _clipAmmo;
    [SerializeField] private int _numberOfClips;

    #endregion

    [SerializeField] private AudioSource _shotFiredSound;
    [SerializeField] private AudioSource _noAmmoSound;
    [SerializeField] private AudioSource _reloadSound;

    private PlayerInputActions _actions;

    public int Ammo => _ammo;

    public int ClipAmmo => _clipAmmo;
    public int NumberOfClips => _numberOfClips;

    public delegate void GunFire();
    public static event GunFire OnGunFire;

    private void Awake()
    {
        _actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken += AddAmmo;

        _clipAmmo = _stockClipAmmo;
        _ammo = _clipAmmo + (_numberOfClips - 1) * _stockClipAmmo;

        _actions.Player.Fire.Enable();

        _actions.Player.Fire.performed += Bang;
    }

    private void Update()
    {
        _ammo = _clipAmmo + _numberOfClips * _stockClipAmmo;
    }

    private void Bang(InputAction.CallbackContext context)
    {
        // Player gun fire conditions
        if (_playerHealth.IsAlive && !_sceneController.gameOnPause)
        {
            if (_clipAmmo > 0 && _numberOfClips >= 0)
            {
                Fire();
                _shotFiredSound.Play();
                _muzzleFlash.Play();
            }
            else if(_clipAmmo == 0 && _numberOfClips > 0)
            {
                Reload();
                // RELOAD
            }
            else
            {
                _noAmmoSound.Play();
            }
        }
    }

    #region RELOAD
    private void Reload()
    {
        //var RELOAD_TIME = 1f;
        //while (RELOAD_TIME > 0)
        //{
        //    RELOAD_TIME -= Time.deltaTime;

        //}
        _clipAmmo = _stockClipAmmo;
        _numberOfClips--;
        _reloadSound.Play();
        Debug.Log("GUN_RELOADED");

        OnGunFire?.Invoke();
    }
    #endregion

    /// <summary>
    /// Fire mechanic
    /// </summary>
    private void Fire()
    {
        var shot = _bulletPool.Get();
        shot.transform.rotation = transform.rotation;
        shot.transform.position = transform.position;
        shot.gameObject.SetActive(true);

        //_ammo--;
        _clipAmmo--;

        OnGunFire?.Invoke();
    }

    /// <summary>
    /// Add ammo by event
    /// </summary>
    private void AddAmmo()
    {
        // OLD AMMO SYSTEM
        //_ammo += _ammoFromPickup;
        //if (_ammo >= 100)
        //    _ammo = 100;

        // NEW AMMO SYSTEM
        _numberOfClips += 2; // Magic number?
        if (_numberOfClips > 6)
            _numberOfClips = 6;            

        _reloadSound.Play();
    }

    private void OnDisable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken -= AddAmmo;

        _actions.Player.Fire.Disable();
        _actions.Player.Fire.performed -= Bang;
    }
}
