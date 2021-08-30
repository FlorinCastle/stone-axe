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
    [Header("UI")]
    [SerializeField] Dropdown _itemDropdown;
    [SerializeField] Dropdown _part1Select;
    [SerializeField] Dropdown _part1Material;
    [SerializeField] Dropdown _part2Select;
    [SerializeField] Dropdown _part2Material;
    [SerializeField] Dropdown _part3Select;
    [SerializeField] Dropdown _part3Material;
    [SerializeField] Text _craftedItem;


    [SerializeField] List<string> itemDropOptions;
    [SerializeField] List<string> part1PartDropOptions;
    [SerializeField] List<string> part2PartDropOptions;
    [SerializeField] List<string> part3PartDropOptions;
    [SerializeField] List<string> part1MaterialDropOptions;
    [SerializeField] List<string> part2MaterialDropOptions;
    [SerializeField] List<string> part3MaterialDropOptions;
    List<ItemData> itemData;
    ItemData chosenItem;
    PartData part1;
    PartData part2;
    PartData part3;
    private void Awake()
    {
        // item dropdown
        _itemDropdown.ClearOptions();
        itemData = _itemScriptReference.getItemDataRef();
        itemDropOptions.Add("Choose Item"); // store this somewhere to allow for localizaion??
        foreach (ItemData item in itemData)
            itemDropOptions.Add(item.ItemName);
        _itemDropdown.AddOptions(itemDropOptions);

        // clear dropdowns
        clearPartOptions();
        clearMaterialOptions();

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
            _part2Material.options[_part3Material.value].text != "Choose Material")
        {
            _itemName = "Item - " + chosenItem.ItemName;
            _materials = "\n\nMaterials\n" + _part1Material.options[_part1Material.value].text 
                + "\n" + _part2Material.options[_part2Material.value].text
                + "\n" + _part2Material.options[_part3Material.value].text;
            //Debug.Log("code is wip");
            _craftedItem.text = _itemName +
                "\nStats" + _materials;
        }
        else
        {
            _craftedItem.text = "new text";
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
            {
                if (item.ItemName == _itemDropdown.options[_itemDropdown.value].text)
                {
                    chosenItem = item;
                }
            }

            //  TODO: set up code so only certain parts can be selected
            setAllPartOptions(_itemDropdown.options[_itemDropdown.value].text);

            //  TODO: set up code so only certain materials can be selected

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
                    part1 = part;
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
                    part2 = part;
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
                    part3 = part;
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

}
