using UnityEngine;

[CreateAssetMenu (fileName = "WeaponItem", menuName = "ScriptableObject/WeaponItem", order = 1)]
public class WeaponItemData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private int _stockAmmo;
}
