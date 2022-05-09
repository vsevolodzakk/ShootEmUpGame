using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : MonoBehaviour
{
    [SerializeField] private GunController _gun;
    private Text _ammoText;

    private void OnEnable()
    {
        PickupController.OnPickupTaken += CountAmmo;
        GunController.OnGunFire += CountAmmo;
        
        _ammoText = GetComponent<Text>();
        CountAmmo();
    }

    private void CountAmmo()
    {
        _ammoText.text = "Ammo: " + _gun.ammo.ToString();
    }

    private void OnDisable()
    {
        GunController.OnGunFire -= CountAmmo;
        PickupController.OnPickupTaken -= CountAmmo;
    }
}
