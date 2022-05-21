using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValues : MonoBehaviour
{
    private string _playerName = "test";
    private string _shopName = "test";
    private string _playerSpecies = "Elf";
    private int _playerColor = 0;
    private List<string> badNameInputs = new List<string>();

    private bool buy;
    private bool sell;
    private bool disassemble;
    private bool craft;

    private void Awake()
    {
        setupBadNameList();
    }

    private void setupBadNameList()
    {
        badNameInputs.Add("\n");
        badNameInputs.Add("\t");
        badNameInputs.Add("\v");
        badNameInputs.Add("\b");
        badNameInputs.Add("\r");
        badNameInputs.Add("\f");
        badNameInputs.Add("\\");
        badNameInputs.Add("\'");
        badNameInputs.Add("\"");
        badNameInputs.Add("\xd");
    }

    public string PlayerDefaultName { get => _playerName; }
    public string ShopDefaultName { get => _shopName; }
    public string PlayerDefaultSpecies { get => _playerSpecies; }
    public int PlayerDefaultColor { get => _playerColor; }
    public List<string> BadNameInputs { get => badNameInputs; }

    public bool BuyAvailable { get => buy; }
    public bool SellAvailable { get => sell; }
    public bool DisassembleAvailable { get => disassemble; }
    public bool CraftAvailable { get => craft; }
}
