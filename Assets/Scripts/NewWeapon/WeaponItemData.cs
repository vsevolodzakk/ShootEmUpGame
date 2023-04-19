using System.Data;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "WeaponItem", menuName = "ScriptableObject/WeaponItem", order = 1)]
public class WeaponItemData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private int _stockAmmo;
    [SerializeField] private Sprite _weaponIcon;

    public int id => _id;
    public Sprite weaponIcon => _weaponIcon;
}
