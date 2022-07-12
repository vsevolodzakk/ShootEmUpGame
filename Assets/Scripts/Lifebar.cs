using UnityEngine;

public class Lifebar : MonoBehaviour
{
    [SerializeField] private GameObject[] lifebarArray;

    [SerializeField] private int _disableIndex;

    private void OnEnable()
    {
        PlayerController.OnPlayerHit += SubstractHp;
        HealthPickupObject.OnHealthPickupObjectTaken += AddHp;
    }

    /// <summary>
    /// Remove 1 HP from Lifebar UI
    /// </summary>
    private void SubstractHp()
    {
        if(_disableIndex > 0)
        {
            lifebarArray[_disableIndex].SetActive(false);
            _disableIndex--;
        }        
    }

    /// <summary>
    /// Add 1 HP to Lifebar UI
    /// </summary>
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
        PlayerController.OnPlayerHit -= SubstractHp;
        HealthPickupObject.OnHealthPickupObjectTaken -= AddHp;
    }
}
