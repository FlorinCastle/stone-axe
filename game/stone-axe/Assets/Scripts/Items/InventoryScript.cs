using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryParent;

    [SerializeField]
    private List<GameObject> _itemInventoryData;
    [SerializeField] 
    private List<GameObject> _itemButtonList;
    [SerializeField]
    private List<GameObject> _partInventoryData;
    [SerializeField] // unused currently - will be used
    private List<GameObject> _partButtonList;
    [SerializeField] 
    private List<MaterialData> materialInventory;
    [SerializeField] // unused currently - will be used
    private List<GameObject> _materialButtonList;

    [Header("UI References")]
    [SerializeField] private Text _descriptionText;
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemDataStoragePrefab;
    [SerializeField] private GameObject _partDataStoragePrefab;
    [SerializeField] private GameObject _itemInfoPrefab;
    [SerializeField] private GameObject _partInfoPrefab;

    private Vector3 prevButtonPos;
    private GameObject tempButtonList;
    public void setupItemInventory()
    {
        prevButtonPos = _inventoryParent.transform.position;
        clearPartButtonList();
        clearItemButtonList();

        foreach (GameObject item in _itemInventoryData)
            if (item != null)
            {
                // get reference to ItemDataStorage script
                ItemDataStorage itemData = item.GetComponent<ItemDataStorage>();
                // instatiate the button prefab
                tempButtonList = Instantiate(_itemInfoPrefab);
                tempButtonList.transform.SetParent(_inventoryParent.transform, false);
                // set up button text
                Text t = tempButtonList.GetComponentInChildren<Text>();
                t.text = itemData.ItemName;
                // set button position
                prevButtonPos.y -= 53f;
                tempButtonList.transform.position = prevButtonPos;
                // add button to list
                InsertItemButton(tempButtonList);
            }
    }

    public void setupPartInventory()
    {
        prevButtonPos = _inventoryParent.transform.position;
        clearItemButtonList();
        clearPartButtonList();

        foreach (GameObject part in _partInventoryData)
            if (part != null)
            {
                // get reference to ItemDataStorage script
                PartDataStorage partData = part.GetComponent<PartDataStorage>();
                // instantiate the button prefab
                tempButtonList = Instantiate(_partInfoPrefab);
                tempButtonList.transform.SetParent(_inventoryParent.transform, false);
                // set up button text
                Text t = tempButtonList.GetComponentInChildren<Text>();
                t.text = partData.PartName;
                // set butto postion
                prevButtonPos.y -= 53f;
                tempButtonList.transform.position = prevButtonPos;
                // add button to list
                InsertPartButton(tempButtonList);
            }
    }

    private string _itemName;
    private string _materials;
    private string _totalStrength;
    private string _totalDex;
    private string _totalInt;
    private string _totalValue;
    public void setItemDetailText(int index)
    {
        if (index != -1)
        {
            if (_itemInventoryData[index] != null)
            {
                // get reference to ItemDataStorage script
                ItemDataStorage itemDataRef = _itemInventoryData[index].GetComponent<ItemDataStorage>();

                // set up text strings
                _itemName = "Item - " + itemDataRef.ItemName;
                _materials = "\n\nMaterials\n" + itemDataRef.Part1.Material.Material
                    + "\n" + itemDataRef.Part2.Material.Material
                    + "\n" + itemDataRef.Part3.Material.Material;
                _totalStrength = "\nStrenght: " + itemDataRef.TotalStrength;
                _totalDex = "\nDextarity: " + itemDataRef.TotalDextarity;
                _totalInt = "\nIntelegence: " + itemDataRef.TotalIntelegence;
                _totalValue = "\n\nValue: " + itemDataRef.TotalValue;

                // organize the texts
                _descriptionText.text = _itemName +
                    "\nStats" + _totalStrength + _totalDex + _totalInt
                    + _materials + _totalValue;
            }
            else
                _descriptionText.text = "new text";
            //Debug.Log("Item Detail Text set for - item index: " + index);
        }
        else
            _descriptionText.text = "new text";
    }

    private string _partName;
    private string _material;
    private string _partStrength;
    private string _partDex;
    private string _partInt;
    private string _partValue;
    public void setPartDetailText(int index)
    {
        if (index != -1)
        {
            if (_partInventoryData[index] != null)
            {
                // get reference to PartDataStorage script
                PartDataStorage partDataRef = _partInventoryData[index].GetComponent<PartDataStorage>();

                // set up text strings
                _partName = "Part - " + partDataRef.PartName;
                _material = "\n\nMaterial\n" + partDataRef.MaterialName;
                _partStrength = "\nStrenght: " + partDataRef.PartStr;
                _partDex = "\nDextarity: " + partDataRef.PartDex;
                _partInt = "\nIntelegence: " + partDataRef.PartInt;
                _partValue = "\n\nValue: " + partDataRef.Value;

                // organize the texts
                _descriptionText.text = _partName +
                    "\nStats" + _partStrength + _partDex + _partInt
                    + _material + _partValue;
            }
            else
                _descriptionText.text = "new text";
        }
        else
            _descriptionText.text = "new text";
    }

    private GameObject itemDataStorageTemp;
    private GameObject part1DataStorageTemp;
    private GameObject part2DataStorageTemp;
    private GameObject part3DataStorageTemp;
    private ItemDataStorage itemDataScriptRef;
    private PartDataStorage part1DataScriptRef;
    private PartDataStorage part2DataScriptRef;
    private PartDataStorage part3DataScriptRef;
    public GameObject convertItemData(ItemData item)
    {
        // get variables set up
        itemDataStorageTemp = Instantiate(_itemDataStoragePrefab);
        itemDataStorageTemp.transform.parent = this.gameObject.transform;
        itemDataScriptRef = itemDataStorageTemp.GetComponent<ItemDataStorage>();

        // convert data from scriptable object to gameobject
        //  stats
        itemDataStorageTemp.name = item.ItemName;
        itemDataScriptRef.setItemName(item.ItemName);
        itemDataScriptRef.setTotalValue(item.TotalValue);
        itemDataScriptRef.setTotalStrenght(item.TotalStrength);
        itemDataScriptRef.setTotalDex(item.TotalDextarity);
        itemDataScriptRef.setTotalInt(item.TotalIntelegence);

        //  parts
        // part 1
        part1DataStorageTemp = Instantiate(_partInfoPrefab);
        part1DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part1DataScriptRef = part1DataStorageTemp.GetComponent<PartDataStorage>();
        // convert data from scriptable object to gameobject
        part1DataStorageTemp.name = item.Part1.Material.Material + " " + item.Part1.PartName;
        part1DataScriptRef.setPartName(item.Part1.PartName);
        part1DataScriptRef.setMaterial(item.Part1.Material);
        part1DataScriptRef.setPartStr(item.Part1.PartStrenght);
        part1DataScriptRef.setPartDex(item.Part1.PartDextarity);
        part1DataScriptRef.setPartInt(item.Part1.PartIntelligence);
        // store ref of part 1 in item script
        itemDataScriptRef.setPart1(part1DataScriptRef);

        // part 2
        part2DataStorageTemp = Instantiate(_partInfoPrefab);
        part2DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part2DataScriptRef = part2DataStorageTemp.GetComponent<PartDataStorage>();
        // convert data from scriptable object to gameobject
        part2DataStorageTemp.name = item.Part2.Material.Material + " " + item.Part2.PartName;
        part2DataScriptRef.setPartName(item.Part2.PartName);
        part2DataScriptRef.setMaterial(item.Part2.Material);
        part2DataScriptRef.setPartStr(item.Part2.PartStrenght);
        part2DataScriptRef.setPartDex(item.Part2.PartDextarity);
        part2DataScriptRef.setPartInt(item.Part2.PartIntelligence);
        // store ref of part 2 in item script
        itemDataScriptRef.setPart2(part2DataScriptRef);

        // part 3
        part3DataStorageTemp = Instantiate(_partInfoPrefab);
        part3DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part3DataScriptRef = part3DataStorageTemp.GetComponent<PartDataStorage>();
        // convert data from scriptable object to gameobject
        part3DataStorageTemp.name = item.Part3.Material.Material + " " + item.Part3.PartName;
        part3DataScriptRef.setPartName(item.Part3.PartName);
        part3DataScriptRef.setMaterial(item.Part3.Material);
        part3DataScriptRef.setPartStr(item.Part3.PartStrenght);
        part3DataScriptRef.setPartDex(item.Part3.PartDextarity);
        part3DataScriptRef.setPartInt(item.Part3.PartIntelligence);
        // store ref of part 3 in item script
        itemDataScriptRef.setPart3(part3DataScriptRef);

        return itemDataStorageTemp;
    }

    private GameObject partDataStorageTemp;
    private PartDataStorage partDataScriptRef;
    public GameObject convertPartData(PartData part)
    {
        // get variables set up
        partDataStorageTemp = Instantiate(_partDataStoragePrefab);
        partDataStorageTemp.transform.parent = this.gameObject.transform;
        partDataScriptRef = partDataStorageTemp.GetComponent<PartDataStorage>();

        // convert data from scriptable object to gameobject
        //  stats
        partDataStorageTemp.name = part.PartName;
        partDataScriptRef.setPartName(part.PartName);
        partDataScriptRef.setMaterial(part.Material);
        partDataScriptRef.setPartStr(part.PartStrenght);
        partDataScriptRef.setPartDex(part.PartDextarity);
        partDataScriptRef.setPartInt(part.PartIntelligence);
        partDataScriptRef.setValue(part.TotalCurrentValue);

        return partDataStorageTemp;
    }

    private bool ItemSlotEmpty(int index)
    {
        if (_itemInventoryData[index] == null)
            return true;

        return false;
    }

    private bool PartSlotEmpty(int index)
    {
        if (_partInventoryData[index] == null)
            return true;

        return false;
    }

    private bool ItemButtonSlotEmpty(int index)
    {
        if (_itemButtonList[index] == null)
            return true;

        return false;
    }
    
    private bool PartButtonSlotEmpty(int index)
    {
        if (_partButtonList[index] == null)
            return true;

        return false;
    }

    private bool MaterialSlotEmpty(int index)
    {
        if (materialInventory[index] == null || materialInventory[index].MaterialCount <= 0)
            return true;

        return false;
    }

    // may or may not need this code
    private bool GetMaterial(int index, out MaterialData mat)
    {
        if (MaterialSlotEmpty(index))
        {
            mat = null;
            return false;
        }
        mat = materialInventory[index];
        return true;
    }

    // inset an item/part/material, return the index where it was inserted. -1 if error.
    public int InsertItem(ItemData item)
    {
        GameObject temp = convertItemData(item);

        for (int i = 0; i < _itemInventoryData.Count; i++)
        {
            if (ItemSlotEmpty(i))
            {
                _itemInventoryData[i] = temp;
                return i;
            }
        }
        return -1;
    }

    public int InsertPart(PartData part)
    {
        GameObject temp = convertPartData(part);

        for (int i = 0; i < _partInventoryData.Count; i++)
            if (PartSlotEmpty(i))
            {
                _partInventoryData[i] = temp;
                return i;
            }

        return -1;
    }

    private int InsertItemButton(GameObject button)
    {
        for (int i = 0; i < _itemButtonList.Count; i++)
            if (ItemButtonSlotEmpty(i))
            {
                _itemButtonList[i] = button;
                button.GetComponent<InventoryButton>().setMyIndex(i);
                return i;
            }
        return -1;
    }

    private int InsertPartButton(GameObject button)
    {
        for (int i = 0; i < _partButtonList.Count; i++)
            if (PartButtonSlotEmpty(i))
            {
                _partButtonList[i] = button;
                button.GetComponent<InventoryButton>().setMyIndex(i);
                return i;
            }
        return -1;
    }
    
    private int InsertMaterial(MaterialData mat)
    {
        for (int m = 0; m < materialInventory.Count; m++)
        {
            if (MaterialSlotEmpty(m))
            {
                materialInventory[m] = mat;
                return m;
            }
        }
        return -1;
    }
    
    private int MaterialInventorySize
    {
        get => materialInventory.Count;
    }

    private void clearItemButtonList()
    {
        foreach (GameObject go in _itemButtonList)
            Destroy(go);

        for (int j = 0; j < _itemButtonList.Count; j++)
            _itemButtonList[j] = null;
    }

    private void clearPartButtonList()
    {
        foreach (GameObject go in _partButtonList)
            Destroy(go);

        for (int j = 0; j < _partButtonList.Count; j++)
            _partButtonList[j] = null;
    }
}
