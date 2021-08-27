using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Item : MonoBehaviour
{
    [SerializeField] string itemName;
    [SerializeField] int level;
    [Header("Materials")]
    [SerializeField] GameObject material1;
    [SerializeField] GameObject material2;
    [SerializeField] GameObject material3;

    private void Awake()
    {
        itemName = gameObject.name;
    }

    public string getM1Name()
    {
        return material1.name;
    }
}
