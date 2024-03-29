using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDataStorage : MonoBehaviour
{
    [Header("Part Data")]
    [SerializeField] private string _partName;
    [SerializeField] private MaterialData _material;
    [SerializeField] private int _totalValue;
    [SerializeField] private int inventoryIndex;

    [Header("Part Stats")]
    [SerializeField] private int _partStrength;
    [SerializeField] private int _partDextarity;
    [SerializeField] private int _partIntelegence;

    //[SerializeField] private PartData _selfRecipeData;
    [SerializeField] private PartJsonData _selfRecipeJsonData;

    [Header("Enchant Storage")]
    [SerializeField] private bool _isEnchanted;
    [SerializeField] private EnchantDataStorage _enchantStorage;

    public SavePartObject SavePart()
    {
        Debug.Log("PartDataStorage.SavePart(): saving " + _material._materialName + " " + _selfRecipeJsonData.partName);
        SavePartObject saveObject = new SavePartObject
        {
            partName = _partName,
            materialName = _material.Material,
            totalValue = _totalValue,
            partStrength = _partStrength,
            partDextarity = _partDextarity,
            partIntellegence = _partIntelegence,
            partRecipeName = _selfRecipeJsonData.partName,

            isEnchanted = _isEnchanted,
            enchantment = checkEnchant(),
        };
        return saveObject;
    }

    private SaveEnchantObject checkEnchant()
    {
        if (_isEnchanted == true)
            return _enchantStorage.SaveEnchant();
        else return null;
    }

    //public PartData RecipeData { get => _selfRecipeData; }
    public PartJsonData RecipeData { get => _selfRecipeJsonData; }
    //public void setRecipeData(PartData partRecipe) { _selfRecipeData = partRecipe; }

    public PartJsonData PartJsonData { get => _selfRecipeJsonData; set => _selfRecipeJsonData = value; }

    public string PartName { get => _partName; }
    public void setPartName(string name) { _partName = name; }

    public MaterialData Material { get => _material; }
    public void setMaterial(MaterialData mat) { _material = mat; }

    public string MaterialName { get => _material.Material; }

    public int Value { get => _totalValue; }
    public void setValue(int value) { _totalValue = value; }

    public int InventoryIndex { get => inventoryIndex; set => inventoryIndex = value; }

    public int PartStr  { get => _partStrength; }
    public void setPartStr(int value) { _partStrength = value; }

    public int PartDex { get => _partDextarity; }
    public void setPartDex(int value) { _partDextarity = value; }

    public int PartInt { get => _partIntelegence; }
    public void setPartInt(int value) { _partIntelegence = value; }

    public void setIsHoldingEnchanted(bool value) { _isEnchanted = value; }
    public bool IsHoldingEnchant { get => _isEnchanted; }

    public void setEnchantment(EnchantDataStorage enchant) { _enchantStorage = enchant; }
    public EnchantDataStorage Enchantment { get => _enchantStorage; }

}
[System.Serializable]
public class SavePartObject
{
    public string partName;
    public string materialName;
    public int totalValue;

    public int partStrength;
    public int partDextarity;
    public int partIntellegence;

    public string partRecipeName;

    public bool isEnchanted;
    // put json object of enchant here if enchated
    public SaveEnchantObject enchantment;
}
