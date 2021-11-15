using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    [Header("Data")]
    // item inventory
    [SerializeField] private List<GameObject> _itemInventoryData;
    // part inventory
    [SerializeField] private List<GameObject> _partInventoryData;
    // material inventory
    [SerializeField] private List<MaterialData> materialInventory;
    // enchant inventory
    [SerializeField] private List<GameObject> _enchantInventoryData;

    public int insertItemData(GameObject item, int i)
    {
        _itemInventoryData[i] = item;
        return i;
    }
    public int insertPartData(GameObject part, int i)
    {
        _partInventoryData[i] = part;
        return i;
    }
    public int insertEnchantData(GameObject enchant, int i)
    {
        _enchantInventoryData[i] = enchant;
        return i;
    }

    public void removeItem(GameObject item)
    {
        foreach (GameObject go in _itemInventoryData)
        {
            if (item == go)
            {
                int index = _itemInventoryData.IndexOf(go);
                Destroy(item);
                _itemInventoryData[index] = null;
                break;
            }
        }
    }
    public void removePart(GameObject part)
    {
        foreach (GameObject go in _partInventoryData)
        {
            if (part == go)
            {
                int index = _partInventoryData.IndexOf(go);
                _partInventoryData[index] = null;
                break;
            }
        }
    }

    public List<GameObject> ItemInventory { get => _itemInventoryData; }
    public List<GameObject> PartInventory { get => _partInventoryData; }
    public List<MaterialData> MaterialInventory { get => materialInventory; }
    public List<GameObject> EnchantInventory { get => _enchantInventoryData; }
}
