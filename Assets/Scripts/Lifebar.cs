﻿using UnityEngine;

public class Lifebar : MonoBehaviour
{
    [SerializeField] private GameObject[] lifebarArray;

    [SerializeField] private PlayerController player;

    [SerializeField] private int _disableIndex;

    private void OnEnable()
    {
        PlayerController.onPlayerHit += SubstractHp;
        HealthPickupObject.onHealthPickupObjectTaken += AddHp;
    }

    private void SubstractHp()
    {
        if(_disableIndex > 0)
        {
            lifebarArray[_disableIndex].SetActive(false);
            _disableIndex--;
        }        
    }

    private void AddHp()
    {
        if(_disableIndex < 2)
        {
            lifebarArray[_disableIndex + 1].SetActive(true);
            _disableIndex++;
        } 
    }

    private void Start()
    {
        _disableIndex = lifebarArray.Length - 1;
        
        foreach (GameObject i in lifebarArray)
            i.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerController.onPlayerHit -= SubstractHp;
        HealthPickupObject.onHealthPickupObjectTaken -= AddHp;
    }
}