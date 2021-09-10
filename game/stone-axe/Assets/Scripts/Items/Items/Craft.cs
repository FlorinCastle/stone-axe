using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Item _itemScriptReference;
    [SerializeField] Part _partScriptReference;
    [SerializeField] Material _materialScriptReference;
    //[SerializeField] Inventory _InventoryScriptReference;
    [SerializeField] InventoryScript _inventoryControlReference;
    [Header("UI")]
    [SerializeField] Dropdown _itemDropdown;
    [SerializeField] Dropdown _part1Select;
    [SerializeField] Dropdown _part1Material;
    [SerializeField] Dropdown _part2Select;
    [SerializeField] Dropdown _part2Material;
    [SerializeField] Dropdown _part3Select;
    [SerializeField] Dropdown _part3Material;
    [SerializeField] Dropdown _partDropdown;
    [SerializeField] Dropdown _materialSelect;
    [SerializeField] Text _craftedItem;


    [SerializeField] List<string> itemDropOptions;
    [SerializeField] List<string> part1PartDropOptions;
    [SerializeField] List<string> part2PartDropOptions;
    [SerializeField] List<string> part3PartDropOptions;
    [SerializeField] List<string> part1MaterialDropOptions;
    [SerializeField] List<string> part2MaterialDropOptions;
    [SerializeField] List<string> part3MaterialDropOptions;
    [SerializeField] List<string> partDropOptions;
    [SerializeField] List<string> partMaterialOptions;
    List<ItemData> itemData;
    List<PartData> partData;
    ItemData chosenItem;
    PartData part1;
    PartData part2;
    PartData part3;
    MaterialData mat1;
    MaterialData mat2;
    MaterialData mat3;
    PartData chosenPart;
    MaterialData chosenMat;
    private void Awake()
    {
        // item dropdown
        _itemDropdown.ClearOptions();
        itemData = _itemScriptReference.getItemDataRef();
        itemDropOptions.Add("Choose Item");
        // set up craftable items
        foreach (ItemData item in itemData)
            itemDropOptions.Add(item.ItemName);
        _itemDropdown.AddOptions(itemDropOptions);

        // part dropdown
        _partDropdown.ClearOptions();
        partData = _partScriptReference.getAllParts();
        partDropOptions.Add("Choose Part");

        // set up craftable parts
        foreach (PartData part in partData)
            partDropOptions.Add(part.PartName);
        _partDropdown.AddOptions(partDropOptions);

        // clear dropdowns
        clearPartOptions();
        clearMaterialOptions();
        clearPartMatOptions();

    }

    private string _itemName;
    private string _materials;
    private string _totalStrength;
    private string _totalDex;
    private string _totalInt;
    private string _totalValue;
    public void craftItem()
    {
        //Debug.Log("code is wip");
        if (_itemDropdown.options[_itemDropdown.value].text != "Choose Item" &&
            _part1Select.options[_part1Select.value].text != "Choose Item" &&
            _part2Select.options[_part2Select.value].text != "Choose Item" &&
            _part3Select.options[_part3Select.value].text != "Choose Item" &&
            _part1Material.options[_part1Material.value].text != "Choose Material" &&
            _part2Material.options[_part2Material.value].text != "Choose Material" &&
            _part3Material.options[_part3Material.value].text != "Choose Material")
        {
            
            _itemName = "Item - " + chosenItem.ItemName;
            _materials = "\n\nMaterials\n" + _part1Material.options[_part1Material.value].text 
                + "\n" + _part2Material.options[_part2Material.value].text
                + "\n" + _part3Material.options[_part3Material.value].text;
            _totalStrength = "\nStrength: " + chosenItem.TotalStrength;
            _totalDex = "\nDextarity: " + chosenItem.TotalDextarity;
            _totalInt = "\nIntelegence: " + chosenItem.TotalIntelegence;
            _totalValue = "\n\nValue: " + chosenItem.TotalValue;
            //Debug.Log("code is wip");
            _craftedItem.text = _itemName +
                "\nStats" + _totalStrength
                + _totalDex
                + _totalInt
                + _materials
                + _totalValue;
            int i = _inventoryControlReference.InsertItem(chosenItem);
            if (i == -1)
                Debug.LogWarning("Could not insert item into inventory!");
            else
            {

            }
            //Debug.Log("inserted item - " + chosenItem.ItemName + " at index: " + i);
        }
        else
        {
            _craftedItem.text = "placeholder text";
        }
    }

    public void checkItemSelection()
    {
        // Debug.Log(_itemDropdown.options[_itemDropdown.value].text);
        if (_itemDropdown.options[_itemDropdown.value].text == "Choose Item")
        {
            // Debug.Log(_itemDropdown.options[_itemDropdown.value].text);
            _part1Select.interactable = false;
            _part1Material.interactable = false;
            _part2Select.interactable = false;
            _part2Material.interactable = false;
            _part3Select.interactable = false;
            _part3Material.interactable = false;

            // clear dropdowns
            clearPartOptions();
            clearMaterialOptions();
        }
        else
        {
            //Debug.Log(_itemDropdown.options[_itemDropdown.value].text);
            foreach (ItemData item in itemData)
                if (item.ItemName == _itemDropdown.options[_itemDropdown.value].text)
                    chosenItem = item;

            //  TODO: set up code so only certain parts can be selected. Done?
            setAllPartOptions(_itemDropdown.options[_itemDropdown.value].text);
            //  TODO: set up code so only certain materials can be selected. Done?

            _part1Select.value = 0;
            _part1Select.interactable = true;
            _part1Material.value = 0;
            _part1Material.interactable = false;
            _part2Select.value = 0;
            _part2Select.interactable = true;
            _part2Material.value = 0;
            _part2Material.interactable = false;
            _part3Select.value = 0;
            _part3Select.interactable = true;
            _part3Material.value = 0;
            _part3Material.interactable = false;
        }
    }

    public void checkPartSelection()
    {
        if (_partDropdown.options[_partDropdown.value].text == "Choose Part")
        {
            _materialSelect.interactable = false;

            clearPartMatOptions();
        }
        else
        {
            foreach (PartData part in partData)
                if (part.PartName == _partDropdown.options[_partDropdown.value].text)
                    chosenPart = part;

            setMaterialOptions(_partDropdown.options[_partDropdown.value].text);

            _materialSelect.value = 0;
            _materialSelect.interactable = true;
        }
    }

    public void checkPart1Selection()
    {
        if (_part1Select.options[_part1Select.value].text != "Choose Item")
        {
            // Debug.Log("chosen part: " + _part1Select.options[_part1Select.value].text);
            // set chosen part
            foreach (PartData part in chosenItem.ValidParts1)
            {
                if (part.PartName == _part1Select.options[_part1Select.value].text)
                {
                    // create instance of part; store in this script and store in chosenItem (instance)
                    //part1 = ScriptableObject.CreateInstance("PartData") as PartData;
                    part1 = part;
                    chosenItem.Part1 = part1;
                }
            }
            // setting material options
            _part1Material.ClearOptions();
            part1MaterialDropOptions.Clear();
            part1MaterialDropOptions.Add("Choose Material");
            foreach (MaterialData material in part1.ValidMaterialData)
            {
                part1MaterialDropOptions.Add(material.Material);
            }
            _part1Material.AddOptions(part1MaterialDropOptions);
            _part1Material.interactable = true;
        }
        else
        {
            _part1Material.interactable = false;
        }
    }

    public void checkPart2Selection()
    {
        if (_part2Select.options[_part2Select.value].text != "Choose Item")
        {
            // Debug.Log("chosen part: " + _part1Select.options[_part1Select.value].text);
            // set chosen part
            foreach (PartData part in chosenItem.ValidParts2)
            {
                if (part.PartName == _part2Select.options[_part2Select.value].text)
                {
                    // create instance of part; store in this script and store in chosenItem (instance)
                    //part2 = ScriptableObject.CreateInstance("PartData") as PartData;
                    part2 = part;
                    chosenItem.Part2 = part2;
                }
            }
            // setting material options
            _part2Material.ClearOptions();
            part2MaterialDropOptions.Clear();
            part2MaterialDropOptions.Add("Choose Material");
            foreach (MaterialData material in part2.ValidMaterialData)
            {
                part2MaterialDropOptions.Add(material.Material);
            }
            _part2Material.AddOptions(part2MaterialDropOptions);
            _part2Material.interactable = true;
        }
        else
        {
            _part2Material.interactable = false;
        }
    }

    public void checkPart3Selection()
    {
        if (_part3Select.options[_part3Select.value].text != "Choose Item")
        {
            // Debug.Log("chosen part: " + _part1Select.options[_part1Select.value].text);
            // set chosen part
            foreach (PartData part in chosenItem.ValidParts3)
            {
                if (part.PartName == _part3Select.options[_part3Select.value].text)
                {
                    // create instance of part; store in this script and store in chosenItem (instance)
                    //part3 = ScriptableObject.CreateInstance("PartData") as PartData;
                    part3 = part;
                    chosenItem.Part3 = part3;
                }
            }
            // setting material options
            _part3Material.ClearOptions();
            part3MaterialDropOptions.Clear();
            part3MaterialDropOptions.Add("Choose Material");
            foreach (MaterialData material in part3.ValidMaterialData)
            {
                part3MaterialDropOptions.Add(material.Material);
            }
            _part3Material.AddOptions(part3MaterialDropOptions);
            _part3Material.interactable = true;
        }
        else
        {
            _part3Material.interactable = false;
        }
    }

    public void checkMat1Selection()
    {
        if (_part1Material.options[_part1Material.value].text != "Choose Item")
        {
            foreach (MaterialData mat in part1.ValidMaterialData)
            {
                if (mat.Material == _part1Material.options[_part1Material.value].text)
                {
                    mat1 = mat;
                    part1.Material = mat1;
                }
            }
        }
    }

    public void checkMat2Selection()
    {
        if (_part2Material.options[_part2Material.value].text != "Choose Item")
        {
            foreach (MaterialData mat in part2.ValidMaterialData)
            {
                if (mat.Material == _part2Material.options[_part2Material.value].text)
                {
                    mat2 = mat;
                    part2.Material = mat2;
                }
            }
        }
    }

    public void checkMat3Selection()
    {
        if (_part3Material.options[_part3Material.value].text != "Choose Item")
        {
            foreach (MaterialData mat in part3.ValidMaterialData)
            {
                if (mat.Material == _part3Material.options[_part3Material.value].text)
                {
                    mat3 = mat;
                    part3.Material = mat3;
                }
            }
        }
    }

    public void checkMaterialSelection()
    {
        if (_materialSelect.options[_materialSelect.value]. text != "Choose Item")
        {
            foreach(MaterialData mat in chosenPart.ValidMaterialData)
            {
                if (mat.Material == _materialSelect.options[_materialSelect.value].text)
                {
                    chosenMat = mat;
                    chosenPart.Material = chosenMat;
                }
            }
        }
    }

    private void setAllPartOptions(string chosenItem)
    {
        // part 1 options
        _part1Select.ClearOptions();
        part1PartDropOptions.Clear();
        part1PartDropOptions.Add("Choose Item");
        foreach (ItemData item in itemData)
        {
            if (item.ItemName == chosenItem)
            {
                List<PartData> valid1Parts = item.ValidParts1;
                foreach (PartData part in valid1Parts)
                    part1PartDropOptions.Add(part.PartName);
            }
        }
        _part1Select.AddOptions(part1PartDropOptions);

        // part 2 options
        _part2Select.ClearOptions();
        part2PartDropOptions.Clear();
        part2PartDropOptions.Add("Choose Item");
        foreach (ItemData item in itemData)
        {
            if (item.ItemName == chosenItem)
            {
                List<PartData> valid2Parts = item.ValidParts2;
                foreach (PartData part in valid2Parts)
                    part2PartDropOptions.Add(part.PartName);
            }
        }
        _part2Select.AddOptions(part2PartDropOptions);

        // part 3 options
        _part3Select.ClearOptions();
        part3PartDropOptions.Clear();
        part3PartDropOptions.Add("Choose Item");
        foreach (ItemData item in itemData)
        {
            if (item.ItemName == chosenItem)
            {
                List<PartData> valid3Parts = item.ValidParts3;
                foreach (PartData part in valid3Parts)
                    part3PartDropOptions.Add(part.PartName);
            }
        }
        _part3Select.AddOptions(part3PartDropOptions);
    }

    private void setMaterialOptions(string chosenPart)
    {
        _materialSelect.ClearOptions();
        partMaterialOptions.Clear();
        partMaterialOptions.Add("Choose Material");
        foreach (PartData part in partData)
        {
            if (part.PartName == chosenPart)
            {
                List<MaterialData> validMaterial = part.ValidMaterialData;
                foreach (MaterialData mat in validMaterial)
                {
                    partMaterialOptions.Add(mat.Material);
                }
            }
        }
        _materialSelect.AddOptions(partMaterialOptions);
    }

    private void clearPartOptions()
    {
        //Debug.Log("Clear Part Options has been called");
        // part 1 options
        _part1Select.ClearOptions();
        part1PartDropOptions.Clear();
        part1PartDropOptions.Add("Choose Item");
        _part1Select.AddOptions(part1PartDropOptions);

        // part 2 options
        _part2Select.ClearOptions();
        part2PartDropOptions.Clear();
        part2PartDropOptions.Add("Choose Item");
        _part2Select.AddOptions(part2PartDropOptions);

        // part 3 options
        _part3Select.ClearOptions();
        part3PartDropOptions.Clear();
        part3PartDropOptions.Add("Choose Item");
        _part3Select.AddOptions(part3PartDropOptions);

    }

    private void clearMaterialOptions()
    {
        //Debug.Log("Clear Material Options has been called");
        // material 1 options
        _part1Material.ClearOptions();
        part1MaterialDropOptions.Clear();
        part1MaterialDropOptions.Add("Choose Material");
        _part1Material.AddOptions(part1MaterialDropOptions);

        // material 2 options
        _part2Material.ClearOptions();
        part2MaterialDropOptions.Clear();
        part2MaterialDropOptions.Add("Choose Material");
        _part2Material.AddOptions(part2MaterialDropOptions);

        // material 3 options
        _part3Material.ClearOptions();
        part3MaterialDropOptions.Clear();
        part3MaterialDropOptions.Add("Choose Material");
        _part3Material.AddOptions(part3MaterialDropOptions);

    }

    private void clearPartMatOptions()
    {
        // material options
        _materialSelect.ClearOptions();
        partMaterialOptions.Clear();
        partMaterialOptions.Add("Choose Material");
        _materialSelect.AddOptions(partMaterialOptions);
    }
}
