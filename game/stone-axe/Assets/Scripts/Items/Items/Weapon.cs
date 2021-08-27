using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] List<GameObject> weaponGroups;
    int ranWeapon;
    
    public string chooseWeapon()
    {
        ranWeapon = Random.Range(0, weaponGroups.Count);
        return "weapon";
    }
}
