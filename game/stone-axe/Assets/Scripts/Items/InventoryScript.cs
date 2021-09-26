using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryParent;
    [SerializeField]
    private GameObject _selectedItem;
    [SerializeField]
    private GameObject _selectedPart;
    [SerializeField]
    private MaterialData _selectedMat;

    private enum removingItemStatusEnum
    {
        NotRemoving,            // int 0
        RemovingToSell,         // int 1
        RemovingToDisassemble,  // int 2
        RemovingToCraft1,       // int 3
        RemovingToCraft2,       // int 4
        RemovingToCraft3,       // int 5
        RemovingToCraftMat      // int 6
    }
    [Header("Data")]
    [SerializeField] private removingItemStatusEnum _removingStatus;

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
    [SerializeField] private Button _headerButtonItems;
    [SerializeField] private Button _headerButtonParts;
    [SerializeField] private Button _headerButtonMaterials;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private GameObject _selectItemButton;
    [SerializeField] private GameObject _selectPartButton;
    [SerializeField] private GameObject _selectMatButton;
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemDataStoragePrefab;
    [SerializeField] private GameObject _partDataStoragePrefab;
    [SerializeField] private GameObject _itemInfoPrefab;
    [SerializeField] private GameObject _partInfoPrefab;
    [SerializeField] private GameObject _matInfoPrefab;

    private GameObject tempButtonList;

    public void setupItemInventory()
    {
        setupItemInventory(false, 0);
    }
    public void setupItemInventory(bool isRemoving, int state)
    {
        _selectItemButton.SetActive(isRemoving);
        _selectPartButton.SetActive(false);
        _selectMatButton.SetActive(false);
        setStatus(state);
        setupHeader();

        clearPartButtonList();
        clearItemButtonList();
        clearMatButtonList();
        int k = 0;
        foreach (GameObject item in _itemInventoryData)
        {
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

                // add button to list
                InsertItemButton(tempButtonList, k);
            }
            k++;
        }
    }

    public void setupPartInventory()
    {
        setupPartInventory(false, 0);
    }
    public void setupPartInventory(bool isRemoving, int state)
    {
        _selectItemButton.SetActive(false);
        _selectPartButton.SetActive(isRemoving);
        _selectMatButton.SetActive(false);
        setStatus(state);
        setupHeader();

        clearItemButtonList();
        clearPartButtonList();
        clearMatButtonList();
        int k = 0;
        foreach (GameObject part in _partInventoryData)
        {
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
                
                // if removing part from inventory
                if (isRemoving == true)
                {
                    if (state == 3 || state == 4 || state == 5)
                    {
                        if (checkIfPartIsValid(state, partData))
                            tempButtonList.GetComponentInChildren<Button>().interactable = true;
                        else
                            tempButtonList.GetComponentInChildren<Button>().interactable = false;
                    }
                }

                // add button to list
                InsertPartButton(tempButtonList, k);
            }
            k++;
        }
    }

    public void setupMatInventory()
    {
        setupMatInventory(false, 0);
    }
    public void setupMatInventory(bool isRemoving, int state)
    {
        _selectItemButton.SetActive(false);
        _selectPartButton.SetActive(false);
        _selectMatButton.SetActive(isRemoving);
        setStatus(state);
        setupHeader();

        clearItemButtonList();
        clearPartButtonList();
        clearMatButtonList();

        int m = 0;
        foreach (MaterialData mat in materialInventory)
        {
            if (mat != null)
            {
                // instantiate the button prefab
                tempButtonList = Instantiate(_matInfoPrefab);
                tempButtonList.transform.SetParent(_inventoryParent.transform, false);

                // set up the button text
                tempButtonList.GetComponentInChildren<MaterialButton>().setMatInfoText(mat);

                // if removing from inventory
                if (isRemoving == true)
                {
                    if (state == 6)
                    {
                        if (checkIfMatIsValid(state, mat))
                            tempButtonList.GetComponentInChildren<Button>().interactable = true;
                        else
                            tempButtonList.GetComponentInChildren<Button>().interactable = false;
                    }
                }
                // add button to list
                InsertMatButton(tempButtonList, m);
            }
            m++;
        }

    }

    private ColorBlock ItemColorBlock;
    private ColorBlock PartColorBlock;
    private ColorBlock MaterialColorBlock; // preping for material inventory
    private void setupHeader()
    {
        //Debug.Log("Setting up Inventory Header - current status: " + _removingStatus.ToString());

        ItemColorBlock = _headerButtonItems.colors;
        PartColorBlock = _headerButtonParts.colors;
        MaterialColorBlock = _headerButtonMaterials.colors;
        if (_removingStatus == removingItemStatusEnum.NotRemoving)
        {
            _headerButtonItems.interactable = true;
            _headerButtonParts.interactable = true;
            _headerButtonMaterials.interactable = true;

            ItemColorBlock.colorMultiplier = 1f;
            PartColorBlock.colorMultiplier = 1f;
            MaterialColorBlock.colorMultiplier = 1f;
        }
        else if (_removingStatus == removingItemStatusEnum.RemovingToSell || _removingStatus == removingItemStatusEnum.RemovingToDisassemble)
        {   // removing item from inventory
            _headerButtonItems.interactable = false;
            _headerButtonParts.interactable = false;
            _headerButtonMaterials.interactable = false;

            ItemColorBlock.colorMultiplier = 2f;
            PartColorBlock.colorMultiplier = 1f;
            MaterialColorBlock.colorMultiplier = 1f;
        }
        else if (_removingStatus == removingItemStatusEnum.RemovingToCraft1 || _removingStatus == removingItemStatusEnum.RemovingToCraft2 || _removingStatus == removingItemStatusEnum.RemovingToCraft3)
        {   // removing part from inventory
            _headerButtonItems.interactable = false;
            _headerButtonParts.interactable = false;
            _headerButtonMaterials.interactable = false;

            ItemColorBlock.colorMultiplier = 1f;
            PartColorBlock.colorMultiplier = 2f;
            MaterialColorBlock.colorMultiplier = 1f;
        }
        else if (_removingStatus == removingItemStatusEnum.RemovingToCraftMat)
        {
            _headerButtonItems.interactable = false;
            _headerButtonParts.interactable = false;
            _headerButtonMaterials.interactable = false;

            ItemColorBlock.colorMultiplier = 1f;
            PartColorBlock.colorMultiplier = 1f;
            MaterialColorBlock.colorMultiplier = 2f;
        }
        _headerButtonItems.colors = ItemColorBlock;
        _headerButtonParts.colors = PartColorBlock;
        _headerButtonMaterials.colors = MaterialColorBlock;
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

    private string _matName;
    private string _matType;
    private string _matStrength;
    private string _matDex;
    private string _matInt;
    private string _matValue;

    public void setMatDetailText(int index)
    {
        if (index != -1)
        {
            if (materialInventory[index] != null)
            {
                MaterialData matDataRef = materialInventory[index];

                _matName = "Material - " + matDataRef.Material;
                _matType = "\nType - " + matDataRef.MaterialType;
                _matStrength = "\nStrength: " + matDataRef.AddedStrength.ToString();
                _matDex = "\nDextarity: " + matDataRef.AddedDextarity.ToString();
                _matInt = "\nIntelegence: " + matDataRef.AddedIntelligence.ToString();
                _matValue = "\nValue: " + matDataRef.BaseCostPerUnit.ToString();

                _descriptionText.text = _matName + _matType + _matValue + "\n" + _matStrength + _matDex + _matInt;
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
        part1DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part1DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part1DataScriptRef = part1DataStorageTemp.GetComponent<PartDataStorage>();
        // store reference to scriptabe object
        part1DataScriptRef.setRecipeData(item.Part1);
        // convert data from scriptable object to gameobject
        part1DataStorageTemp.name = item.Part1.Material.Material + " " + item.Part1.PartName;
        part1DataScriptRef.setPartName(item.Part1.PartName);
        part1DataScriptRef.setMaterial(item.Part1.Material);
        part1DataScriptRef.setPartStr(item.Part1.PartStrenght);
        part1DataScriptRef.setPartDex(item.Part1.PartDextarity);
        part1DataScriptRef.setPartInt(item.Part1.PartIntelligence);
        part1DataScriptRef.setValue(item.Part1.TotalCurrentValue);
        // store ref of part 1 in item script
        itemDataScriptRef.setPart1(part1DataScriptRef);

        // part 2
        part2DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part2DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part2DataScriptRef = part2DataStorageTemp.GetComponent<PartDataStorage>();
        // store reference to scriptabe object
        part2DataScriptRef.setRecipeData(item.Part2);
        // convert data from scriptable object to gameobject
        part2DataStorageTemp.name = item.Part2.Material.Material + " " + item.Part2.PartName;
        part2DataScriptRef.setPartName(item.Part2.PartName);
        part2DataScriptRef.setMaterial(item.Part2.Material);
        part2DataScriptRef.setPartStr(item.Part2.PartStrenght);
        part2DataScriptRef.setPartDex(item.Part2.PartDextarity);
        part2DataScriptRef.setPartInt(item.Part2.PartIntelligence);
        part2DataScriptRef.setValue(item.Part2.TotalCurrentValue);
        // store ref of part 2 in item script
        itemDataScriptRef.setPart2(part2DataScriptRef);

        // part 3
        part3DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part3DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part3DataScriptRef = part3DataStorageTemp.GetComponent<PartDataStorage>();
        // store reference to scriptabe object
        part3DataScriptRef.setRecipeData(item.Part3);
        // convert data from scriptable object to gameobject
        part3DataStorageTemp.name = item.Part3.Material.Material + " " + item.Part3.PartName;
        part3DataScriptRef.setPartName(item.Part3.PartName);
        part3DataScriptRef.setMaterial(item.Part3.Material);
        part3DataScriptRef.setPartStr(item.Part3.PartStrenght);
        part3DataScriptRef.setPartDex(item.Part3.PartDextarity);
        part3DataScriptRef.setPartInt(item.Part3.PartIntelligence);
        part3DataScriptRef.setValue(item.Part3.TotalCurrentValue);
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

    private bool MatButtonSlotEmpty(int index)
    {
        if (_materialButtonList[index] == null)
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
                temp.GetComponent<ItemDataStorage>().setInventoryIndex(i);
                _itemInventoryData[i] = temp;
                return i;
            }
        }
        return -1;
    }

    public int InsertCraftedItem(GameObject item)
    {
        for (int i = 0; i < _itemInventoryData.Count; i++)
        {
            if (ItemSlotEmpty(i))
            {
                item.GetComponent<ItemDataStorage>().setInventoryIndex(i);
                _itemInventoryData[i] = item;
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

    public int InsertPartData(GameObject part)
    {
        //Debug.Log(part.name);
        if (part.GetComponent<PartDataStorage>() != null)
        {
            for (int i = 0; i < _partInventoryData.Count; i++)
                if (PartSlotEmpty(i))
                {
                    _partInventoryData[i] = part;
                    return i;
                }
        }
        else if (part.GetComponent<PartDataStorage>() == null)
            Debug.LogWarning("Part does not contain the PartDataStorage component!");
        return -1;
    }

    private int InsertItemButton(GameObject button, int j)
    {
        for (int i = 0; i < _itemButtonList.Count; i++)
            if (ItemButtonSlotEmpty(i))
            {
                _itemButtonList[i] = button;
                button.GetComponent<InventoryButton>().setMyIndex(i);
                button.GetComponent<InventoryButton>().setItemIndex(j);
                return i;
            }
        return -1;
    }

    private int InsertPartButton(GameObject button, int j)
    {
        for (int i = 0; i < _partButtonList.Count; i++)
            if (PartButtonSlotEmpty(i))
            {
                _partButtonList[i] = button;
                button.GetComponent<InventoryButton>().setMyIndex(i);
                button.GetComponent<InventoryButton>().setPartIndex(j);
                return i;
            }
        return -1;
    }

    private int InsertMatButton(GameObject button, int k)
    {
        for (int i = 0; i < _materialButtonList.Count; i++)
            if (MatButtonSlotEmpty(i))
            {
                _materialButtonList[i] = button;
                button.GetComponent<MaterialButton>().setMyIndex(i);
                button.GetComponent<MaterialButton>().setMatIndex(k);
                return i;
            }

        return -1;
    }

    public void RemoveItem(int index)
    {
        //Debug.Log("removing item at index: " + index);
        GameObject item = _itemInventoryData[index];
        Destroy(item);
        _descriptionText.text = "item text";
        _itemInventoryData[index] = null;
        _selectedItem = null;
        //_descriptionText.text = "item text";
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

    private void clearMatButtonList()
    {
        foreach (GameObject go in _materialButtonList)
            Destroy(go);

        for (int k = 0; k < _materialButtonList.Count; k++)
            _materialButtonList[k] = null;
    }

    public void setSelectedItem(int i)
    {
        if (i != -1)
        {
            _selectedItem = _itemInventoryData[i];
            //Debug.Log("Selected item is: " + _selectedItem.name + " at index: " + i); 
        }
        else
            Debug.Log("example button selected");
    }

    public void setSelectedPart(int i)
    {
        if (i != -1)
        {
            _selectedPart = _partInventoryData[i];
            //Debug.Log("Selected part is: " + _selectedPart.name + " at index: " + i);
        }
        else
            Debug.Log("example button selected");
    }

    public void setSelectedMat(int i)
    {
        if (i != -1)
        {
            _selectedMat = materialInventory[i];
        }
        else
            Debug.Log("example button selected");
    }

    public GameObject getSelectedItem()
    {
        if (_selectedItem != null) 
            return _selectedItem;
         
        return null;
    }

    public GameObject getSelectedPart()
    {
        if (_selectedPart != null)
            return _selectedPart;

        return null;
    }

    public MaterialData getSelectedMat()
    {
        if (_selectedMat != null)
            return _selectedMat;

        return null;
    }

    private void setStatus(int value)
    {
        //Debug.Log("setStatus - value = " + value);
        if (value == 0) _removingStatus = removingItemStatusEnum.NotRemoving;
        else if (value == 1) _removingStatus = removingItemStatusEnum.RemovingToSell;
        else if (value == 2) _removingStatus = removingItemStatusEnum.RemovingToDisassemble;
        else if (value == 3) _removingStatus = removingItemStatusEnum.RemovingToCraft1;
        else if (value == 4) _removingStatus = removingItemStatusEnum.RemovingToCraft2;
        else if (value == 5) _removingStatus = removingItemStatusEnum.RemovingToCraft3;
        else if (value == 6) _removingStatus = removingItemStatusEnum.RemovingToCraftMat;
        else
        {
            _removingStatus = removingItemStatusEnum.NotRemoving;
            Debug.LogWarning("Invalid input status!");
        }
    }

    public void returnSelectedItem()
    {
        //Debug.Log("returning item");
        if (_removingStatus == removingItemStatusEnum.RemovingToSell)
        {
            //Debug.Log("returning item to sell");
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<SellItemControl>().selectItem();
        }
        else if (_removingStatus == removingItemStatusEnum.RemovingToDisassemble)
        {
            //Debug.Log("returning item to disassemble");
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<DisassembleItemControl>().selectItem();
        }
    }

    public void returnSeletedPart()
    {
        if (_removingStatus == removingItemStatusEnum.RemovingToCraft1)
            GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().SelectPart1();
        //SetPart1(_selectedPart);
        else if (_removingStatus == removingItemStatusEnum.RemovingToCraft2)
            GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().SelectPart2();
        //SetPart2(_selectedPart);
        else if (_removingStatus == removingItemStatusEnum.RemovingToCraft3)
            GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().SelectPart3();
                //SetPart3(_selectedPart);

    }

    public void returnSelectedMat()
    {
        if (_removingStatus == removingItemStatusEnum.RemovingToCraftMat)
            GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().SelectMat();
    }

    private bool checkIfPartIsValid(int i, PartDataStorage part) // incoming i should either be 3, 4, or 5
    {
        CraftControl ccRef = GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>();
        PartData validPart = part.RecipeData;

        if (i == 3)
        {
            foreach (PartData validPart1 in ccRef.checkItemRecipe().ValidParts1)
                if (validPart1.PartName == validPart.PartName)
                    return true;
        }
        else if (i == 4)
        {
            foreach (PartData validPart2 in ccRef.checkItemRecipe().ValidParts2)
                if (validPart2.PartName == validPart.PartName)
                    return true;
        }
        else if (i == 5)
        {
            foreach (PartData validPart3 in ccRef.checkItemRecipe().ValidParts3)
                if (validPart3.PartName == validPart.PartName)
                    return true;
        }

        return false;
    }

    private bool checkIfMatIsValid(int m, MaterialData mat) // incoming m should be 6
    {
        CraftControl ccRef = GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>();

        if (m == 6)
        {
            foreach (MaterialData validMat in ccRef.checkPartRecipe().ValidMaterialData)
                if (validMat.Material == mat.Material)
                    return true;
        }

        return false;
    }
}
