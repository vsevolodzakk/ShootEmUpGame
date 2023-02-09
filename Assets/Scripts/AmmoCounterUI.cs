using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : MonoBehaviour
{
    [SerializeField] private GunController _gun;
    private Text _ammoText;

    private void OnEnable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken += CountAmmo;
        GunController.OnGunFire += CountAmmo;
        
        _ammoText = GetComponent<Text>();
        CountAmmo();
    }

    /// <summary>
    /// Update ammo count on UI
    /// </summary>
    private void CountAmmo()
    {
        _ammoText.text = "Ammo: " + _gun.Ammo.ToString();
        //Debug.Log("COUNT_AMMO");
    }

    private void OnDisable()
    {
        AmmoPickupObject.OnAmmoPickupObjectTaken -= CountAmmo;
        GunController.OnGunFire -= CountAmmo;
    }
}
