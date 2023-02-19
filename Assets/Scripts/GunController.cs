using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    // Bullet pool object reference
    [SerializeField] private BulletPool _bulletPool;

    // Player Health component reference
    [SerializeField] private HealthComponent _playerHealth;

    // Gun Fire VFX
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private Light _muzzleLight;

    // Scene Controller reference
    [SerializeField] private SceneController _sceneController;

    // Amount ammo in clip
    [SerializeField] private int _stockClipAmmo;

    // Amount ammo clips in Ammo box PowerUp
    [SerializeField] private int _clipsInAmmoBox;

    //Ammo count in clip
    [SerializeField] private int _clipAmmo;

    // Number of ammo clips carriyng
    [SerializeField] private int _numberOfClips;

    // Shot VFX
    [SerializeField] private AudioSource _shotFiredSound;
    [SerializeField] private AudioSource _noAmmoSound;
    [SerializeField] private AudioSource _reloadSound;

    [SerializeField] private Animator _playerAnimator;
    private bool _isReloading;

    // Input actions
    private PlayerInputActions _actions;

    // Ammo properties
    public int ClipAmmo => _clipAmmo;
    public int NumberOfClips => _numberOfClips;

    // gun fire event
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

        _actions.Player.Fire.Enable();
        _actions.Player.ReloadWeapon.Enable();

        _actions.Player.Fire.performed += Bang;
        _actions.Player.ReloadWeapon.performed += Reload;

        _isReloading = false;
    }

    private void Bang(InputAction.CallbackContext context)
    {
        // Player gun fire conditions
        if (_playerHealth.IsAlive && !_sceneController.GameOnPause && !_isReloading)
        {
            if (_clipAmmo > 0 && _numberOfClips >= 0)
            {
                Fire(transform);
                _shotFiredSound.Play();
                _muzzleFlash.Play();
                StartCoroutine(FlashingMuzzleLight());
            }
            else if(_clipAmmo == 0 && _numberOfClips > 0)
            {
                StartCoroutine(ReloadWeapon());
                // RELOAD
            }
            else
            {
                _noAmmoSound.Play();
            }
        }
    }

    /// <summary>
    /// Reload Mechanics
    /// </summary>
    private void Reload(InputAction.CallbackContext context)
    {
        if(_numberOfClips > 0)
            StartCoroutine(ReloadWeapon());
    }

    private IEnumerator ReloadWeapon()
    {
        _isReloading= true;
        _playerAnimator.SetBool("IsReloading", _isReloading);
        _clipAmmo = _stockClipAmmo;
        _numberOfClips--;
        _reloadSound.Play();
        Debug.Log("GUN_RELOADED");

        OnGunFire?.Invoke();
        yield return new WaitForSeconds(2.3f);

        _isReloading = false;
        _playerAnimator.SetBool("IsReloading", _isReloading);
    }

    private IEnumerator FlashingMuzzleLight()
    {
        _muzzleLight.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _muzzleLight.enabled = false;
    }

    /// <summary>
    /// Fire mechanic
    /// </summary>
    //private void Fire()
    //{
    //    var shot = _bulletPool.Get();
    //    shot.transform.rotation = transform.rotation;
    //    shot.transform.position = transform.position;
    //    shot.gameObject.SetActive(true);

    //    _clipAmmo--;

    //    OnGunFire?.Invoke();
    //}

    private void Fire(Transform firePoint)
    {
        var shot = _bulletPool.Get();
        shot.transform.rotation = firePoint.transform.rotation;
        shot.transform.position = firePoint.transform.position;
        shot.gameObject.SetActive(true);

        _clipAmmo--;

        OnGunFire?.Invoke();
    }

    /// <summary>
    /// Add ammo by event
    /// </summary>
    private void AddAmmo()
    {
        _numberOfClips += _clipsInAmmoBox; // Magic number?
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
