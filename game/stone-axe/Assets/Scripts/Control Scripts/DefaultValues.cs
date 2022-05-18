using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValues : MonoBehaviour
{
    private string _playerName = "test";
    private string _shopName = "test";
    private string _playerSpecies = "Elf";
    private int _playerColor = 0;

    public string PlayerDefaultName { get => _playerName; }
    public string ShopDefaultName { get => _shopName; }
    public string PlayerDefaultSpecies { get => _playerSpecies; }
    public int PlayerDefaultColor { get => _playerColor; }
}
