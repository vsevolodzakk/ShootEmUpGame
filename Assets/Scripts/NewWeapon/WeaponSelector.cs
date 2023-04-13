using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelector : MonoBehaviour
{

    private int _selectedWeapon = 0;
    private PlayerInputActions _actions;


    private void Awake()
    {
        _actions = new PlayerInputActions();
    }


    void Start()
    {
        //SelectWeapon(_selectedWeapon);
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
        int i = selectedWeapon;
        foreach(Transform weapon in transform)
        {
            if(i == _selectedWeapon)
                weapon.gameObject.SetActive(true);
            else 
                weapon.gameObject.SetActive(false);

            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
