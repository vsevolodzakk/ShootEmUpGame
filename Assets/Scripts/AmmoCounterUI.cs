using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : MonoBehaviour
{
    [SerializeField] private GunController _gun;
    private Text _ammoText;

    [SerializeField] private Transform _weaponSelector;
    [SerializeField] private Transform _weaponIconUI;

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
        var ammoCount = _weaponSelector.GetComponentInChildren<GunController>();
        var weaponIcon = _weaponIconUI.GetComponent<Image>();

        _ammoText.text = "Ammo:" + ammoCount.ClipAmmo.ToString() + "-" + ammoCount.NumberOfClips.ToString();
        weaponIcon.sprite = ammoCount.weaponData.weaponIcon;

        Debug.Log(ammoCount.weaponData.id.ToString());

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
