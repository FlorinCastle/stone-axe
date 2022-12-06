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
    //[SerializeField] private List<MaterialData> materialInventory;
    [SerializeField] private List<GameObject> materialInventory;
    // enchant inventory
    [SerializeField] private List<GameObject> _enchantInventoryData;

    [SerializeField] private InventoryScript _invControlRef;

    public SaveMaterialsObject saveMaterials()
    {
        List<SaveMatObject> matObjects = new List<SaveMatObject>();
        /*
        foreach (MaterialData mat in MaterialInventory)
        {
            matObjects.Add(saveMat(mat));
        }
        */
        foreach (GameObject mat in materialInventory)
        {
            matObjects.Add(saveMat(mat.GetComponent<MaterialDataStorage>()));
        }

        SaveMaterialsObject saveObject = new SaveMaterialsObject
        {
            matObjectList = matObjects,
        };
        return saveObject;
    }
    /*
    private SaveMatObject saveMat(MaterialData mat)
    {
        SaveMatObject matObject = new SaveMatObject
        {
            matName = mat.Material,
            matCount = mat.MaterialCount,
        };
        return matObject;
    }
    */
    private SaveMatObject saveMat(MaterialDataStorage mat)
    {
        SaveMatObject matObject = new SaveMatObject
        {
            matName = mat.Material,
            matCount = mat.MaterialCount
        };
        return matObject;
    }

    public void loadMaterials(SaveMaterialsObject savedMats)
    {
        foreach (SaveMatObject mat in savedMats.matObjectList)
        {
            /*
            foreach (MaterialData matData in MaterialInventory)
                if (matData.Material == mat.matName)
                    matData.MaterialCount = mat.matCount;
            */
            foreach (GameObject matObj in materialInventory)
                if (matObj.GetComponent<MaterialDataStorage>().MatDataRef.Material == mat.matName)
                    matObj.GetComponent<MaterialDataStorage>().MaterialCount = mat.matCount;
                    
        }
    }

    public int getMaterialCount(MaterialData mat)
    {
        foreach (GameObject matData in materialInventory)
            if (matData.GetComponent<MaterialDataStorage>().MatDataRef.Material == mat.Material)
                return matData.GetComponent<MaterialDataStorage>().MaterialCount;
        return -1;
    }
    public int getMaterialCount(string mat)
    {
        foreach (GameObject matData in materialInventory)
            if (matData.GetComponent<MaterialDataStorage>().MatDataRef.Material == mat)
                return matData.GetComponent<MaterialDataStorage>().MaterialCount;
        return -1;
    }
    public void getRandomItem()
    {
        int ranItemIndex = Random.Range(0,_itemInventoryData.Count);
        this.gameObject.GetComponent<InventoryScript>().setSelectedItem(ranItemIndex);
    }

    public int insertItemData(GameObject item)
    {
        _itemInventoryData.Add(item);
        item.GetComponent<ItemDataStorage>().setInventoryIndex(_itemInventoryData.IndexOf(item));
        _invControlRef.setupItemInventory();
        return _itemInventoryData.IndexOf(item);
    }
    public int insertPartData(GameObject part)
    {
        _partInventoryData.Add(part);
        _invControlRef.setupPartInventory();
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
        _invControlRef.setupEnchantInventory();
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
        _invControlRef.setupItemInventory();
        correctItemIndex();
    }

    public void correctItemIndex()
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
            if (part.GetComponent<PartDataStorage>().RecipeData == go.GetComponent<PartDataStorage>().RecipeData)
            {
                int index = _partInventoryData.IndexOf(go);
                if (destroy == true)
                    Destroy(part);
                //_partInventoryData[index] = null;
                _partInventoryData.RemoveAt(index);
                break;
            }
        }
        _invControlRef.setupPartInventory();
        correctPartIndex();
    }
    public void correctPartIndex()
    {
        foreach (GameObject go in _partInventoryData)
        {
            int index = _partInventoryData.IndexOf(go);
            go.GetComponent<PartDataStorage>().InventoryIndex = index;
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
    /*public void removeAllQuestItems()
    {
        for (var i = _itemInventoryData.Count - 1; i > -1; i--)
        {
            if (_itemInventoryData[i].GetComponent<ItemDataStorage>().IsForQuest)
            {
                Destroy(_itemInventoryData[i]);
                _itemInventoryData.RemoveAt(i);
            }
        }
        /*foreach(GameObject go in _itemInventoryData)
        {
            if (go.GetComponent<ItemDataStorage>().IsForQuest)
            {
                int index = _itemInventoryData.IndexOf(go);
                Destroy(go);
                _itemInventoryData.RemoveAt(index);
            }
        } 
    }*/
    /*
    public MaterialData getMaterial(string matName)
    {
        foreach(MaterialData mat in materialInventory)
            if (mat.Material == matName)
                return mat;
        return null;
    }
    */

    public MaterialDataStorage getMaterial(string matName)
    {
        foreach (GameObject matObj in materialInventory)
            if (matObj.GetComponent<MaterialDataStorage>().MatDataRef.Material == matName)
                return matObj.GetComponent<MaterialDataStorage>();

        return null;
    }
    public MaterialData getMaterialData(string matName)
    {
        foreach (GameObject matObj in materialInventory)
            if (matObj.GetComponent<MaterialDataStorage>().MatDataRef.Material == matName)
                return matObj.GetComponent<MaterialDataStorage>().MatDataRef;

        return null;
    }
    public List<GameObject> ItemInventory { get => _itemInventoryData; }
    public List<GameObject> PartInventory { get => _partInventoryData; }
    public List<GameObject> MaterialInventory { get => materialInventory; }
    public List<GameObject> EnchantInventory { get => _enchantInventoryData; }
}
[System.Serializable]
public class SaveMaterialsObject
{
    public List<SaveMatObject> matObjectList;
}
[System.Serializable]
public class SaveMatObject
{
    public string matName;
    public int matCount;
}
