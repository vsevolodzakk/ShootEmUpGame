using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : MonoBehaviour
{
    [SerializeField] private GunController _gun;
    private Text _ammoText;

    [SerializeField] private Transform _weaponSelector;

    private void OnEnable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken += CountAmmo;
        GunController.OnGunFire += CountAmmo;

        WeaponSelector.OnWeaponSwitch += CountAmmo;
        
        _ammoText = GetComponent<Text>();
        CountAmmo();
    }

    /// <summary>
    /// Update ammo count on UI
    /// </summary>
    private void CountAmmo()
    {
        GunController ammoCount = _weaponSelector.GetComponentInChildren<GunController>();

        _ammoText.text = "Ammo:" + ammoCount.ClipAmmo.ToString() + "-" + ammoCount.NumberOfClips.ToString();

        ////_ammoText.text = "Ammo: " + _gun.Ammo.ToString();
        //_ammoText.text = "Ammo:" + _gun.ClipAmmo.ToString() 
        //                    + "-" + _gun.NumberOfClips.ToString();
        ////Debug.Log("COUNT_AMMO");
    }

    private void OnDisable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken -= CountAmmo;
        GunController.OnGunFire -= CountAmmo;
    }
}
