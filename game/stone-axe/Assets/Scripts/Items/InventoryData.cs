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
    /*
    public int insertItemData(GameObject item, int i)
    {
        _itemInventoryData[i] = item;
        return i;
    }
    */
    public int insertItemData(GameObject item)
    {
        _itemInventoryData.Add(item);
        item.GetComponent<ItemDataStorage>().setInventoryIndex(_itemInventoryData.IndexOf(item));
        return _itemInventoryData.IndexOf(item);
    }
    /*
    public int insertPartData(GameObject part, int i)
    {
        _partInventoryData[i] = part;
        return i;
    }
    */
    public int insertPartData(GameObject part)
    {
        _partInventoryData.Add(part);
        return _partInventoryData.IndexOf(part);
    }
    public int insertEnchantData(GameObject enchant, int i)
    {
        _enchantInventoryData[i] = enchant;
        return i;
    }
    public int insertEnchantData(GameObject enchant)
    {
        _enchantInventoryData.Add(enchant);
        return _enchantInventoryData.IndexOf(enchant);
    }

    public void removeItem(GameObject item)
    {
        foreach (GameObject go in _itemInventoryData)
        {
            if (item == go)
            {
                int index = _itemInventoryData.IndexOf(go);
                Destroy(item);
                //_itemInventoryData[index] = null;
                _itemInventoryData.RemoveAt(index);
                break;
            }
        }
        correctItemIndex();
    }

    private void correctItemIndex()
    {
        foreach (GameObject go in _itemInventoryData)
        {
            int index = _itemInventoryData.IndexOf(go);
            go.GetComponent<ItemDataStorage>().setInventoryIndex(index);
        }
    }
    public void removePart(GameObject part, bool destroy)
    {
        foreach (GameObject go in _partInventoryData)
        {
            if (part == go)
            {
                int index = _partInventoryData.IndexOf(go);
                if (destroy == true)
                    Destroy(part);
                //_partInventoryData[index] = null;
                _partInventoryData.RemoveAt(index);
                break;
            }
        }
    }

    public void removeAllItems()
    {
        foreach(GameObject go in _itemInventoryData)
        {
            int index = _itemInventoryData.IndexOf(go);
            Destroy(go);
        }
        _itemInventoryData.Clear();
    }
    public void removeAllParts()
    {
        foreach(GameObject go in _partInventoryData)
        {
            int index = _partInventoryData.IndexOf(go);
            Destroy(go);
        }
        _partInventoryData.Clear();
    }
    public void removeAllEnchants()
    {
        foreach(GameObject go in _partInventoryData)
        {
            int index = _partInventoryData.IndexOf(go);
            Destroy(go);
        }
        _enchantInventoryData.Clear();
    }

    public MaterialData getMaterial(string matName)
    {
        foreach(MaterialData mat in materialInventory)
            if (mat.Material == matName)
                return mat;
        return null;
    }

    public List<GameObject> ItemInventory { get => _itemInventoryData; }
    public List<GameObject> PartInventory { get => _partInventoryData; }
    public List<MaterialData> MaterialInventory { get => materialInventory; }
    public List<GameObject> EnchantInventory { get => _enchantInventoryData; }
}
