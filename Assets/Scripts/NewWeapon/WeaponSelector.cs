using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelector : MonoBehaviour
{
    private PlayerInputActions _actions;

    public delegate void WeaponSwitched();
    public static event WeaponSwitched OnWeaponSwitch;

    private void Awake()
    {
        _actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _actions.Player.SelectWeapon1.Enable();
        _actions.Player.SelectWeapon2.Enable();

        _actions.Player.SelectWeapon1.performed += SelectFirstWeapon;
        _actions.Player.SelectWeapon2.performed += SelectSecondWeapon;
    }

    private void SelectFirstWeapon(InputAction.CallbackContext context)
    { 
        SelectWeapon(0);
    }

    private void SelectSecondWeapon(InputAction.CallbackContext context)
    {
        SelectWeapon(1);
    }

    private void SelectWeapon(int selectedWeapon)
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                OnWeaponSwitch?.Invoke();
            }
            else 
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    private void OnDisable()
    {
        _actions.Player.SelectWeapon1.Disable();
        _actions.Player.SelectWeapon2.Disable();

        _actions.Player.SelectWeapon1.performed -= SelectFirstWeapon;
        _actions.Player.SelectWeapon2.performed -= SelectSecondWeapon;
    }
}
