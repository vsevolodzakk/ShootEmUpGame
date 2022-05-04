using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private PlayerController _player;
    [SerializeField] private ParticleSystem _muzzleFlash;

    [SerializeField] private SceneController _sceneController;
    
    public int ammo;

    [SerializeField] private AmmoPickupObject _ammoBox;
    [SerializeField] private int _ammoFromPickup;

    [SerializeField] private AudioSource _shotFiredSound;
    [SerializeField] private AudioSource _noAmmoSound;
    [SerializeField] private AudioSource _reloadSound;

    public delegate void GunFire();
    public static event GunFire onGunFire;

    private void OnEnable()
    {
        PickupController.onPickupTaken += AddAmmo;

    }

    private void Start()
    {
        _ammoFromPickup = _ammoBox.ammoInBox;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")
                && _player.isAlive && ammo > 0
                    && _sceneController.gameOnPause == false)
        {
            Fire();
            _muzzleFlash.Play();
        }
        else if (Input.GetButtonDown("Fire1")
                && _player.isAlive && ammo == 0)
            _noAmmoSound.Play();
    }   

    // Fire mechanic
    private void Fire()
    {
        var shot = _bulletPool.Get();
        ammo--;
        shot.transform.rotation = transform.rotation;
        shot.transform.position = transform.position;
        shot.gameObject.SetActive(true);
        onGunFire?.Invoke();
        _shotFiredSound.Play();
    }

    private void AddAmmo()
    {

        ammo += _ammoFromPickup;
        if (ammo >= 100)
            ammo = 100;
        _reloadSound.Play();
    }

    private void OnDisable()
    {
        PickupController.onPickupTaken -= AddAmmo;
    }
}
