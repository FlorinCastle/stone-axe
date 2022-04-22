using UnityEngine;

public class ItemDataStorage : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private string _itemName;
    [SerializeField] private int _totalValue;
    [SerializeField] private int inventoryIndex;
    [SerializeField] private ItemData _receipeRef;
    [SerializeField] private bool _isNew;
    [SerializeField] private bool _forQuest;

    [Header("Item Stats")]
    [SerializeField] private int _totalStrength;
    [SerializeField] private int _totalDextarity;
    [SerializeField] private int _totalIntelegence;

    [Header("Parts")]
    [SerializeField] private PartDataStorage _part1;
    [SerializeField] private PartDataStorage _part2;
    [SerializeField] private PartDataStorage _part3;

    [Header("Enchantment")]
    [SerializeField] private bool _isEnchanted;
    [SerializeField] private EnchantDataStorage _enchantment;

    public SaveItemObject SaveItem()
    {
        SaveItemObject saveObject = new SaveItemObject
        {
            itemName = _itemName,
            totalValue = _totalValue,
            invIndex = inventoryIndex,
            recipeName = _receipeRef.ItemName,
            totalStrenght = _totalStrength,
            totalDextarity = _totalDextarity,
            totalIntellegence = _totalIntelegence,

            part1 = _part1.SavePart(),
            part2 = _part2.SavePart(),
            part3 = _part3.SavePart(),

            isEnchanted = _isEnchanted,
            enchantment = checkEnchant(),
        };
        return saveObject;
    }
    private SaveEnchantObject checkEnchant()
    {
        if (_isEnchanted == true)
            return _enchantment.SaveEnchant();
        return null;
    }

    public void setItemName(string name) { _itemName = name; }
    public string ItemName { get => _itemName; }

    public void setTotalValue(int value) { _totalValue = value; }
    public int TotalValue { get => _totalValue; }

    public ItemData ItemRecipeRef { get => _receipeRef; set => _receipeRef = value; }

    public bool IsNew { get => _isNew; set => _isNew = value; }
    public bool IsForQuest { get => _forQuest; set => _forQuest = value; }

    public void setTotalStrenght(int value) { _totalStrength = value; }
    public int TotalStrength{ get => _totalStrength; }

    public void setTotalDex(int value) { _totalDextarity = value; }
    public int TotalDextarity { get => _totalDextarity; }

    public void setTotalInt(int value) { _totalIntelegence = value; }
    public int TotalIntelegence { get => _totalIntelegence; }

    public void setPart1(PartDataStorage value) { _part1 = value; }
    public PartDataStorage Part1 { get => _part1; }

    public void setPart2(PartDataStorage value) { _part2 = value; }
    public PartDataStorage Part2 { get => _part2; }

    public void setPart3(PartDataStorage value) { _part3 = value; }
    public PartDataStorage Part3 { get => _part3; }

    public void setInventoryIndex(int value) { inventoryIndex = value; }
    public int InventoryIndex { get => inventoryIndex; }

    public void setIsEnchanted(bool value) { _isEnchanted = value; }
    public bool IsEnchanted { get => _isEnchanted; }

    public void setEnchantment(EnchantDataStorage enchant) { _enchantment = enchant; }
    public EnchantDataStorage Enchantment { get => _enchantment; }
}
[System.Serializable]
public class SaveItemObject
{
    public string itemName;
    public int totalValue;
    public int invIndex;
    public string recipeName;
    public int totalStrenght;
    public int totalDextarity;
    public int totalIntellegence;

    // put json objects of parts here
    public SavePartObject part1;
    public SavePartObject part2;
    public SavePartObject part3;

    public bool isEnchanted;
    // put json object of enchant here if enchated
    public SaveEnchantObject enchantment;
}
